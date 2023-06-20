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
    //public Tilemap[] tilemaps;
    //public RuleTile[] ruleTiles;
    public MapGeneration mapGenerator;
    public GameObject chunkManagerObject;
    public ChunkManager chunkManager;

    public GameObject createWorld1Menu;
    public GameObject SingleplayerMenu;
    public TMP_InputField world1InputField;
    public TMP_Text slot1Text;
    public TMP_Text slot1HostText;

    public Grid mapGrid;
    private string world1name;
    [Header("Tile Objects")]
    public GameObject waterTile;
    public GameObject dirtTile;
    public GameObject grassTile;
    public GameObject sandTile;
    public GameObject[] worldChunks;

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
        //mapGenerator.GenerateForestUnderground();
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
        var CurrentWorldData = new WorldData
        {
            WorldName = world1name,
            mapGridArray = mapGenerator.GridMap
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

        int[,] mapArray = worldData.mapGridArray;

        int length = 512;
        int width = 512;
        GenerateChunks();
        chunkManager.GetChunks();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {               
                if (mapArray[x,y] == 1)
                {
                    var go = Instantiate(waterTile, new Vector3Int(x, y, 0), Quaternion.identity);
                    ChunkCheck(x, y, go);
                }
                else if (mapArray[x, y] == 2)
                {
                    var go = Instantiate(sandTile, new Vector3Int(x, y, 0), Quaternion.identity);
                    ChunkCheck(x, y, go);
                }
                else if (mapArray[x, y] == 3)
                {
                    var go = Instantiate (grassTile, new Vector3Int(x, y, 0), Quaternion.identity);
                    ChunkCheck(x, y, go);
                }
                else if (mapArray[x, y] == 4)
                {
                    var go = Instantiate(dirtTile, new Vector3Int(x, y, 0), Quaternion.identity);
                    ChunkCheck(x, y, go);
                }
            }
        }     
    }

    public void LoadClientWorldData(string saveFile) //This is an alternate load function, that only loads tiles without a game object. This is for clients, as the server only needs to load and spawn object tiles
    {
        string json = File.ReadAllText(Application.persistentDataPath + saveFile);
        if (json == null) return;
        WorldData worldData = JsonConvert.DeserializeObject<WorldData>(json);
        List<TileData> loadedTiles = new List<TileData>();                    
    }

    private void DeleteWorldData(string saveFilePath)
    {
        if (File.Exists(saveFilePath)) File.Delete(saveFilePath); //If the file exists, delete it
    }

    private void GenerateChunks()
    {
        var chunkAmount = 64;
        worldChunks = new GameObject[chunkAmount]; //Array size is equal to amount of chunks
        for (int i = 0; i < chunkAmount; i++)
        {
            GameObject chunk = new GameObject();
            chunk.transform.parent = chunkManagerObject.transform;
            chunk.name = i.ToString();
            worldChunks[i] = chunk; //Set chunk created to position in array
        }
    }

    private void ChunkCheck(int x, int y, GameObject tile)
    {
        if (x >= 0 && x <= 64 && y >= 0 && y <= 64)
        {
            tile.transform.parent = worldChunks[0].transform;
        }
        else if (x >= 65 && x <= 128 && y >= 0 && y <= 64)
        {
            tile.transform.parent = worldChunks[1].transform;
        }
        else if (x >= 129 && x <= 192 && y >= 0 && y <= 64)
        {
            tile.transform.parent = worldChunks[2].transform;
        }
        else if (x >= 193 && x <= 256 && y >= 0 && y <= 64)
        {
            tile.transform.parent = worldChunks[3].transform;
        }
        else if (x >= 257 && x <= 320 && y >= 0 && y <= 64)
        {
            tile.transform.parent = worldChunks[4].transform;
        }
        else if (x >= 321 && x <= 384 && y >= 0 && y <= 64)
        {
            tile.transform.parent = worldChunks[5].transform;
        }
        else if (x >= 385 && x <= 448 && y >= 0 && y <= 64)
        {
            tile.transform.parent = worldChunks[6].transform;
        }
        else if (x >= 449 && x <= 512 && y >= 0 && y <= 64)
        {
            tile.transform.parent = worldChunks[7].transform;
        }

        else if (x >= 0 && x <= 64 && y >= 65 && y <= 128)
        {
            tile.transform.parent = worldChunks[8].transform;
        }
        else if (x >= 65 && x <= 128 && y >= 65 && y <= 128)
        {
            tile.transform.parent = worldChunks[9].transform;
        }
        else if (x >= 129 && x <= 192 && y >= 65 && y <= 128)
        {
            tile.transform.parent = worldChunks[10].transform;
        }
        else if (x >= 193 && x <= 256 && y >= 65 && y <= 128)
        {
            tile.transform.parent = worldChunks[11].transform;
        }
        else if (x >= 257 && x <= 320 && y >= 65 && y <= 128)
        {
            tile.transform.parent = worldChunks[12].transform;
        }
        else if (x >= 321 && x <= 384 && y >= 65 && y <= 128)
        {
            tile.transform.parent = worldChunks[13].transform;
        }
        else if (x >= 385 && x <= 448 && y >= 65 && y <= 128)
        {
            tile.transform.parent = worldChunks[14].transform;
        }
        else if (x >= 449 && x <= 512 && y >= 65 && y <= 128)
        {
            tile.transform.parent = worldChunks[15].transform;
        }

        else if (x >= 0 && x <= 64 && y >= 129 && y <= 192)
        {
            tile.transform.parent = worldChunks[16].transform;
        }
        else if (x >= 65 && x <= 128 && y >= 129 && y <= 192)
        {
            tile.transform.parent = worldChunks[17].transform;
        }
        else if (x >= 129 && x <= 192 && y >= 129 && y <= 192)
        {
            tile.transform.parent = worldChunks[18].transform;
        }
        else if (x >= 193 && x <= 256 && y >= 129 && y <= 192)
        {
            tile.transform.parent = worldChunks[19].transform;
        }
        else if (x >= 257 && x <= 320 && y >= 129 && y <= 192)
        {
            tile.transform.parent = worldChunks[20].transform;
        }
        else if (x >= 321 && x <= 384 && y >= 129 && y <= 192)
        {
            tile.transform.parent = worldChunks[21].transform;
        }
        else if (x >= 385 && x <= 448 && y >= 129 && y <= 192)
        {
            tile.transform.parent = worldChunks[22].transform;
        }
        else if (x >= 449 && x <= 512 && y >= 129 && y <= 192)
        {
            tile.transform.parent = worldChunks[23].transform;
        }

        else if (x >= 0 && x <= 64 && y >= 193 && y <= 256)
        {
            tile.transform.parent = worldChunks[24].transform;
        }
        else if (x >= 65 && x <= 128 && y >= 193 && y <= 256)
        {
            tile.transform.parent = worldChunks[25].transform;
        }
        else if (x >= 129 && x <= 192 && y >= 193 && y <= 256)
        {
            tile.transform.parent = worldChunks[26].transform;
        }
        else if (x >= 193 && x <= 256 && y >= 193 && y <= 256)
        {
            tile.transform.parent = worldChunks[27].transform;
        }
        else if (x >= 257 && x <= 320 && y >= 193 && y <= 256)
        {
            tile.transform.parent = worldChunks[28].transform;
        }
        else if (x >= 321 && x <= 384 && y >= 193 && y <= 256)
        {
            tile.transform.parent = worldChunks[29].transform;
        }
        else if (x >= 385 && x <= 448 && y >= 193 && y <= 256)
        {
            tile.transform.parent = worldChunks[30].transform;
        }
        else if (x >= 449 && x <= 512 && y >= 193 && y <= 256)
        {
            tile.transform.parent = worldChunks[31].transform;
        }

        else if (x >= 0 && x <= 64 && y >= 257 && y <= 320)
        {
            tile.transform.parent = worldChunks[32].transform;
        }
        else if (x >= 65 && x <= 128 && y >= 257 && y <= 320)
        {
            tile.transform.parent = worldChunks[33].transform;
        }
        else if (x >= 129 && x <= 192 && y >= 257 && y <= 320)
        {
            tile.transform.parent = worldChunks[34].transform;
        }
        else if (x >= 193 && x <= 256 && y >= 257 && y <= 320)
        {
            tile.transform.parent = worldChunks[35].transform;
        }
        else if (x >= 257 && x <= 320 && y >= 257 && y <= 320)
        {
            tile.transform.parent = worldChunks[36].transform;
        }
        else if (x >= 321 && x <= 384 && y >= 257 && y <= 320)
        {
            tile.transform.parent = worldChunks[37].transform;
        }
        else if (x >= 385 && x <= 448 && y >= 257 && y <= 320)
        {
            tile.transform.parent = worldChunks[38].transform;
        }
        else if (x >= 449 && x <= 512 && y >= 257 && y <= 320)
        {
            tile.transform.parent = worldChunks[39].transform;
        }

        else if (x >= 0 && x <= 64 && y >= 321 && y <= 384)
        {
            tile.transform.parent = worldChunks[40].transform;
        }
        else if (x >= 65 && x <= 128 && y >= 321 && y <= 384)
        {
            tile.transform.parent = worldChunks[41].transform;
        }
        else if (x >= 129 && x <= 192 && y >= 321 && y <= 384)
        {
            tile.transform.parent = worldChunks[42].transform;
        }
        else if (x >= 193 && x <= 256 && y >= 321 && y <= 384)
        {
            tile.transform.parent = worldChunks[43].transform;
        }
        else if (x >= 257 && x <= 320 && y >= 321 && y <= 384)
        {
            tile.transform.parent = worldChunks[44].transform;
        }
        else if (x >= 321 && x <= 384 && y >= 321 && y <= 384)
        {
            tile.transform.parent = worldChunks[45].transform;
        }
        else if (x >= 385 && x <= 448 && y >= 321 && y <= 384)
        {
            tile.transform.parent = worldChunks[46].transform;
        }
        else if (x >= 449 && x <= 512 && y >= 321 && y <= 384)
        {
            tile.transform.parent = worldChunks[47].transform;
        }

        else if (x >= 0 && x <= 64 && y >= 385 && y <= 448)
        {
            tile.transform.parent = worldChunks[48].transform;
        }
        else if (x >= 65 && x <= 128 && y >= 385 && y <= 448)
        {
            tile.transform.parent = worldChunks[49].transform;
        }
        else if (x >= 129 && x <= 192 && y >= 385 && y <= 448)
        {
            tile.transform.parent = worldChunks[50].transform;
        }
        else if (x >= 193 && x <= 256 && y >= 385 && y <= 448)
        {
            tile.transform.parent = worldChunks[51].transform;
        }
        else if (x >= 257 && x <= 320 && y >= 385 && y <= 448)
        {
            tile.transform.parent = worldChunks[52].transform;
        }
        else if (x >= 321 && x <= 384 && y >= 385 && y <= 448)
        {
            tile.transform.parent = worldChunks[53].transform;
        }
        else if (x >= 385 && x <= 448 && y >= 385 && y <= 448)
        {
            tile.transform.parent = worldChunks[54].transform;
        }
        else if (x >= 449 && x <= 512 && y >= 385 && y <= 448)
        {
            tile.transform.parent = worldChunks[55].transform;
        }

        else if (x >= 0 && x <= 64 && y >= 449 && y <= 512)
        {
            tile.transform.parent = worldChunks[56].transform;
        }
        else if (x >= 65 && x <= 128 && y >= 449 && y <= 512)
        {
            tile.transform.parent = worldChunks[57].transform;
        }
        else if (x >= 129 && x <= 192 && y >= 449 && y <= 512)
        {
            tile.transform.parent = worldChunks[58].transform;
        }
        else if (x >= 193 && x <= 256 && y >= 449 && y <= 512)
        {
            tile.transform.parent = worldChunks[59].transform;
        }
        else if (x >= 257 && x <= 320 && y >= 449 && y <= 512)
        {
            tile.transform.parent = worldChunks[60].transform;
        }
        else if (x >= 321 && x <= 384 && y >= 449 && y <= 512)
        {
            tile.transform.parent = worldChunks[61].transform;
        }
        else if (x >= 385 && x <= 448 && y >= 449 && y <= 512)
        {
            tile.transform.parent = worldChunks[62].transform;
        }
        else if (x >= 449 && x <= 512 && y >= 449 && y <= 512)
        {
            tile.transform.parent = worldChunks[63].transform;
        }
    }
}

