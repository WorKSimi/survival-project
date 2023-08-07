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
    private float seed;

    [Header("Tile Objects")]
    public GameObject water_Tile;
    public GameObject dirt_Tile;
    public GameObject grass_Tile;
    public GameObject sand_Tile;
    public GameObject grass_Wall;
    public GameObject tree_Tile;
    public GameObject tin_Wall;
    
    public GameObject redShroom_Tile;
    public GameObject brownShroom_Tile;
    public GameObject blueberry_Tile;

    public GameObject[] worldChunks;
    public GameObject chunkHolder;
    public GameObject chunkPrefab;
    public ChunkController chunkController;

    private WorldData worldDataStorage; //This stores the world data when it is loaded for the first time. Easier to send to client this way.
    private int chunkSize = 32;

    private int width = 512;
    private int height = 512;
    

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
        var seed2 = Random.Range(-9999f, 9999f); //seed for world gen
        seed = seed2;
        Debug.Log("Seed on creation: " + seed);
        world1name = world1InputField.text; //Set world 1 name to be what player entered
        //mapGenerator.GenerateForestSurface(seed2); //Generate World
        StartCoroutine(mapGenerator.GenerateForestSurface(seed2));
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
        Debug.Log("Seed on saving: " + seed);
        var CurrentWorldData = new WorldData
        {
            WorldName = world1name,
            mapGridArray = mapGenerator.GridMap,
            WorldSeed = seed,
        };

        string json = JsonConvert.SerializeObject(CurrentWorldData, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        System.IO.File.WriteAllText(Application.persistentDataPath + saveFile, json);
    }

    //float progress;
    //public int ProgressPercentage { get { return (int)(100 * progress); } }

    public void LoadWorldData(string saveFile)
    {
        string json = File.ReadAllText(Application.persistentDataPath + saveFile);
        WorldData worldData = JsonConvert.DeserializeObject<WorldData>(json);
        worldDataStorage = worldData; //Store the world data in the variable on script.
        List<TileData> loadedTiles = new List<TileData>();

        //int[,] mapArray = worldData.mapGridArray;
        float seedler = worldData.WorldSeed;
        Debug.Log("Seed on Load: " + seedler);

        StartCoroutine(mapGenerator.GenerateForestSurface(seedler));
        chunkController.chunksLoaded = true;
    }

    public void LoadClientWorldData(string saveFile) //This is an alternate load function, that only loads tiles without a game object. This is for clients, as the server only needs to load and spawn object tiles
    {
        //string json = File.ReadAllText(Application.persistentDataPath + saveFile);
        //if (json == null) return;
        //WorldData worldData = JsonConvert.DeserializeObject<WorldData>(json);
        //List<TileData> loadedTiles = new List<TileData>();

        //int[,] mapArray = worldData.mapGridArray;
        Debug.Log("Sending seed to client!");
        var seed9 = worldDataStorage.WorldSeed;
        LoadDataClientRpc(seed9);
    }

    private void DeleteWorldData(string saveFilePath)
    {
        if (File.Exists(saveFilePath)) File.Delete(saveFilePath); //If the file exists, delete it
    }

    //Create chunks on client
    //Go through mapData on Server, send each tile to Client and instantiate
    //Then on client, go through each tile and assign to chunks

    //Generate Chunks on client
    [ClientRpc] //Fired by Server, Executed on Client
    private void LoadDataClientRpc(float seed) //Take in map data, load world on client based on that
    {
        if (IsHost) return; //If your the host ABORT!
        Debug.Log("Seed on Client Load: " + seed);
        Debug.Log("Thanks for the seed! Generating Map...");
        StartCoroutine(mapGenerator.GenerateForestSurface(seed));
    }
}

