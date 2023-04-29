using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class Startup
{
    static Startup()
    {
        typeof(System.ComponentModel.INotifyPropertyChanging).GetHashCode();
        typeof(System.ComponentModel.INotifyPropertyChanged).GetHashCode();
    }
}

public class WorldDataCollector : NetworkBehaviour
{
    public Tilemap[] tilemaps;
    public RuleTile[] ruleTiles;
    public MapGeneration mapGenerator;

    public GameObject createWorld1Menu;
    public GameObject SingleplayerMenu;
    public TMP_InputField world1InputField;
    public TMP_Text slot1Text;
    public TMP_Text slot1HostText;

    public Grid mapGrid;
    private string world1name;

    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private Tilemap carpetTilemap;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap waterTilemap;
    private List<TileData> tilesOnMap = new List<TileData>();

    public string file1 = "/WorldData1.json";

    public void Start()
    {
        if (File.Exists(Application.persistentDataPath + file1)) //If there is a save file in slot 1
        {
            string json = File.ReadAllText(Application.persistentDataPath + file1);
            WorldData worldData = JsonConvert.DeserializeObject<WorldData>(json);
            string worldName1 = worldData.WorldName;
            slot1Text.text = worldName1.ToString();
            slot1HostText.text = worldName1.ToString();
        }
        else Debug.Log("No Data Found");
    }
    public void SelectSlot1()
    {
        if (File.Exists(Application.persistentDataPath + file1)) //If there is a save file in slot 1
        {
            SceneManager.LoadScene(1); //Load into the game! // 0 = Main Menu, 1 = Game Scene
        }
        else //If there is NO save file... (Empty Slot)
        {
            SingleplayerMenu.SetActive(false);
            createWorld1Menu.SetActive(true);         
        }
    }

    public void CreateWorld1() //This function runs when you click Generate on make world screen.
    {
        world1name = world1InputField.text; //Set world 1 name to be what player entered
        mapGenerator.GenerateForestSurface(); //Generate World
        SaveWorldData(file1); //When world generates, save it to slot 1
        slot1Text.text = world1name; 
        SingleplayerMenu.SetActive(true);
        createWorld1Menu.SetActive(false);
    }

    public void DeleteWorld1()
    {
        DeleteWorldData(Application.persistentDataPath + file1);
    }
    
    private void SaveWorldData(string saveFile)
    {
        foreach (Tilemap tilemap in tilemaps) //Go through tilemaps
        {
            for (int x = tilemap.cellBounds.min.x; x < tilemap.cellBounds.max.x; x++)
            {
                for (int y = tilemap.cellBounds.min.y; y < tilemap.cellBounds.max.y; y++) //Loop through each spot
                {
                    var currentTile = tilemap.GetTile(new Vector3Int(x, y, 0)); //Get tile at that spot
                    if (currentTile != null)
                    {
                        var tile = new TileData
                        {
                            tileLocation = new Vector3Int(x, y, 0),
                            tileType = TileDataComparision(currentTile.name.ToString()),
                            tilemapType = TilemapDecider(tilemap.name.ToString())
                        };
                        tilesOnMap.Add(tile);
                    }                   
                }
            }
        }

        var CurrentWorldData = new WorldData
        {
            WorldName = world1name,
            tilesOnMapList = tilesOnMap
        };

        string json = JsonConvert.SerializeObject(CurrentWorldData, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        System.IO.File.WriteAllText(Application.persistentDataPath + saveFile, json);
    }

    public void LoadWorldData(string saveFile)
    {
        string json = File.ReadAllText(Application.persistentDataPath + saveFile);
        WorldData worldData = JsonConvert.DeserializeObject<WorldData>(json);
        List<TileData> loadedTiles = new List<TileData>();

        foreach (TileData tile in worldData.tilesOnMapList) //For each tile in saved list, add to loaded tiles list
        {
            loadedTiles.Add(tile);
        }

        foreach (TileData tile in loadedTiles) //For each tile in loaded tiles list, do this stuff
        {
            Tilemap tilemap = TilemapChecker(tile.tilemapType);
            RuleTile tile1 = TileReturner(tile.tileType);
            tilemap.SetTile(tile.tileLocation, tile1);
        }
    }

    public void LoadClientWorldData(string saveFile) //This is an alternate load function, that only loads tiles without a game object. This is for clients, as the server only needs to load and spawn object tiles
    {
        string json = File.ReadAllText(Application.persistentDataPath + saveFile);
        if (json == null) return;
        WorldData worldData = JsonConvert.DeserializeObject<WorldData>(json);
        List<TileData> loadedTiles = new List<TileData>();

        foreach (TileData tile in worldData.tilesOnMapList) //For each tile in saved list, add to loaded tiles list
        {           
            loadedTiles.Add(tile); //If the tile doesn't have a game object, add it to the list
        }

        foreach (TileData tile in loadedTiles) //For each tile in loaded tiles list, do this stuff
        {
            var tile1 = TileReturner(tile.tileType);
            if (tile1.m_DefaultGameObject == null)  //If the tile has NO game object
            {
                var theTile = tile.tileType; //Get tile enum
                var theTilemap = tile.tilemapType; //Get tilemap enum
                var theLocation = tile.tileLocation; //Get tile location
                SetTileClientRpc(theTile, theTilemap, theLocation); //Send info for client to load it
            }
            else //if tile DOES have a game object
            {
                Debug.Log("Do nothing, object found!");
            }
        }
    }

    [ClientRpc] //Fired by server, executed on client
    private void SetTileClientRpc(TileType tileType, TilemapType tilemapType, Vector3Int tileLocation)
    {
        Tilemap tilemap = TilemapChecker(tilemapType); 
        RuleTile tile1 = TileReturner(tileType);
        tilemap.SetTile(tileLocation, tile1); 
    }

    //private void SpawnAllObjectsOnNetwork()
    //{
    //    GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
    //    foreach (GameObject gameObject in allObjects)
    //    {
    //        if (gameObject.GetComponent<NetworkObject>()) //If the object has a network object component
    //        {
    //            gameObject.GetComponent<NetworkObject>().Spawn(); //Spawn that object
    //        }
    //    }
    //}

    private void DeleteWorldData(string saveFilePath)
    {
        if (File.Exists(saveFilePath)) File.Delete(saveFilePath); //If the file exists, delete it
    }

    public TileType TileDataComparision(string tileName)
    {
        switch (tileName.ToLower())
        {
            default:
            case ("blueberrytile"):
            return TileType.Blueberry;

            case ("brownshroomtile"):
                return TileType.BrownShroom;

            case ("caveentrancetile"):
                return TileType.CaveEntrance;

            case ("craftingtabletile"):
                return TileType.CraftingTable;

            case ("dirttile"):
                return TileType.Dirt;

            case ("dryfarmtile"):
                return TileType.DryFarm;

            case ("wetfarmtile"):
                return TileType.WetFarm;

            case ("grasstile"):
                return TileType.Grass;

            case ("grasswalltile"):
                return TileType.GrassWall;

            case ("redshroomtile"):
                return TileType.RedShroom;

            case ("sandtile"):
                return TileType.Sand;

            case ("tinoretile"):
                return TileType.TinOre;

            case ("treetile"):
                return TileType.Tree;

            case ("watertile"):
                return TileType.Water;

            case ("wheatcroptile"):
                return TileType.WheatCrop;

            case ("woodwalltile"):
                return TileType.WoodWall;

            case ("flinttile"):
                return TileType.Flint;

            case ("stonenodetile"):
                return TileType.StoneNode;
        }
    }

    private TilemapType TilemapDecider(string tilemapName)
    {
        switch (tilemapName.ToLower())
        {
            default:
            case ("groundtilemap"):
            return TilemapType.Ground;

            case ("watertilemap"):
                return TilemapType.Water;

            case ("carpettilemap"):
                return TilemapType.Carpet;

            case ("walltilemap"):
                return TilemapType.Wall;
        }
    }

    private Tilemap TilemapChecker(TilemapType tilemapType)
    {
        switch (tilemapType)
        {
            default:
            case (TilemapType.Ground):
                return tilemaps[0];

            case (TilemapType.Water):
                return tilemaps[1];

            case (TilemapType.Carpet):
                return tilemaps[3];

            case (TilemapType.Wall):
                return tilemaps[2];
        }
    }

    public RuleTile TileReturner(TileType tiletype)
    {
        switch (tiletype)
        {
            default:
            case (TileType.Blueberry):
                return ruleTiles[0];

            case (TileType.BrownShroom):
                return ruleTiles[1];

            case (TileType.CaveEntrance):
                return ruleTiles[2];

            case (TileType.CraftingTable):
                return ruleTiles[3];

            case (TileType.Dirt):
                return ruleTiles[4];

            case (TileType.DryFarm):
                return ruleTiles[5];

            case (TileType.Grass):
                return ruleTiles[6];

            case (TileType.GrassWall):
                return ruleTiles[7];

            case (TileType.RedShroom):
                return ruleTiles[8];

            case (TileType.Sand):
                return ruleTiles[9];

            case (TileType.TinOre):
                return ruleTiles[10];

            case (TileType.Tree):
                return ruleTiles[11];

            case (TileType.Water):
                return ruleTiles[12];

            case (TileType.WetFarm):
                return ruleTiles[13];

            case (TileType.WheatCrop):
                return ruleTiles[14];

            case (TileType.WoodWall):
                return ruleTiles[15];

            case (TileType.Flint):
                return ruleTiles[16];

            case (TileType.StoneNode):
                return ruleTiles[17];
        }
    }
}

