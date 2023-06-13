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

        int length = 100;
        int width = 100;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {               
                if (mapArray[x,y] == 1)
                {
                    Instantiate(waterTile, new Vector3Int(x, y, 0), Quaternion.identity);
                }
                else if (mapArray[x, y] == 2)
                {
                    Instantiate(sandTile, new Vector3Int(x, y, 0), Quaternion.identity);
                }
                else if (mapArray[x, y] == 3)
                {
                    Instantiate(grassTile, new Vector3Int(x, y, 0), Quaternion.identity);
                }
                else if (mapArray[x, y] == 4)
                {
                    Instantiate(dirtTile, new Vector3Int(x, y, 0), Quaternion.identity);
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
}

