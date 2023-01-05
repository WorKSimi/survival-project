using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour
{
    [Header("Tiles")]
    public RuleTile waterTile;
    public RuleTile sandTile;
    public RuleTile grassTile;
    public RuleTile grassWallTile;
    public RuleTile dirtTile;
    public RuleTile stoneTile;
    public RuleTile treeTile;
    public RuleTile tinOre;

    public GameObject caveEntrance;

    [Header("Dimensions")]
    public int width = 50;
    public int height = 50;
    public int falloffSize = (100);
    public float scale = 1.0f;
    public Vector2 offset;

    [Header("Other Stuff")]
    public Grid worldGrid;
    public bool useFalloff;

    [Header("Height Map")]
    public Wave[] heightWaves;
    public float[,] heightMap;
    float[,] falloffMap;

    private Tilemap wallTilemap;
    private Tilemap groundTilemap;
    private Tilemap carpetTilemap;

    private GameObject[] caveEntrances; //Creates an array to store cave entrances
    private int currentCaveEntrances;
    private int maxCaveEntrances = 5;
    bool firstcaveSpawned;

    // ground, carpet, wall, interactive
    // Start is called before the first frame update
    void Start()
    {
        falloffMap = FalloffGenerator.GenerateFalloffMap(falloffSize);
        firstcaveSpawned = false;
        currentCaveEntrances = 0;
        GetTilemaps();
        GenerateMap();
    }

    void GetTilemaps()
    {
        var groundObject = worldGrid.transform.GetChild(0);
        var carpetObject = worldGrid.transform.GetChild(1);
        var wallObject = worldGrid.transform.GetChild(2);

        groundTilemap = groundObject.GetComponent<Tilemap>();
        carpetTilemap = carpetObject.GetComponent<Tilemap>();
        wallTilemap = wallObject.GetComponent<Tilemap>();
    }

    void GenerateMap()
    {
        heightWaves[0].seed = Random.Range(1.0f, 100.0f);
        heightWaves[1].seed = Random.Range(1.0f, 100.0f);

        heightMap = NoiseGenerator.Generate(width, height, scale, heightWaves, offset); //height map
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y) //Cycle through the noise map
            {
                if (useFalloff)
                {
                    heightMap[x, y] = Mathf.Clamp01(heightMap[x, y] - falloffMap[x, y]);
                }

                //Instantiate tile on tilemap based on height value               
                var height = heightMap[x, y];

                if (height < 0.2f) //Water
                {
                    groundTilemap.SetTile(new Vector3Int(x, y, 0), waterTile);
                }
                else if (height < 0.3f) //Sand
                {
                    groundTilemap.SetTile(new Vector3Int(x, y, 0), sandTile);
                }
                else if (height < 0.6f) //Grass
                {
                    groundTilemap.SetTile(new Vector3Int(x, y, 0), grassTile);
                    if (Random.value >= 0.96) //4 percent chance
                    {
                        wallTilemap.SetTile(new Vector3Int(x, y, 0), treeTile);
                    }
                }
                else if (height < 0.7f) //Dirt Ground
                {
                    groundTilemap.SetTile(new Vector3Int(x, y, 0), dirtTile);
                    TrySpawnCaveEntrance(x, y);
                }
                else if (height < 1.0f) //Dirt Wall
                {
                    wallTilemap.SetTile(new Vector3Int(x, y, 0), grassWallTile);
                    if (Random.value >= 0.96) //4 percent chance each wall tile
                    {
                        SpawnOreVein(x, y);
                    }
                }
            }
        }
    }

    void SpawnOreVein(int x, int y)
    {
        wallTilemap.SetTile(new Vector3Int(x, y, 0), tinOre); //Spawn ore tile
        wallTilemap.SetTile(new Vector3Int(x-1, y, 0), tinOre); //Spawn ore tile to the right
        wallTilemap.SetTile(new Vector3Int(x, y-1, 0), tinOre); //Spawn ore tile down
    }
    void TrySpawnCaveEntrance(int x, int y)
    {
        if (firstcaveSpawned == false) //First cave has not been spawned
        {
            Instantiate(caveEntrance, new Vector3(x, y, 0), Quaternion.identity); //Spawn first cave entrance            
            currentCaveEntrances++; //Add 1 to current cave count
            firstcaveSpawned = true; //Set the first cave spawned variable to true
        }

        else if (firstcaveSpawned == true) //The first cave HAS been spawned
        {
            //Find all cave entrances and add to array
            caveEntrances = GameObject.FindGameObjectsWithTag("CaveEntrance");
            var shouldSpawn = true;
            foreach (GameObject ce in caveEntrances) //Do this for every cave entrance in the array
            {
                float minDistance = 50f; //Min distance caves can be from eachother;
                float dist = Vector3.Distance(ce.transform.position, new Vector3(x, y, 0)); //Get distance between each cave entrance and where to place
                Debug.Log(dist);

                if (dist < minDistance) //If the distance between the cave entrance and the spot is less then the minimum
                {
                    shouldSpawn = false;
                    Debug.Log("Cannot Place Cave, too close"); //DONT place entrance
                }
            }
            if (shouldSpawn == true)
            {
                Instantiate(caveEntrance, new Vector3(x, y, 0), Quaternion.identity); //Spawn a cave entrance
                currentCaveEntrances++;  //Add 1 to current cave count
            }
        }
    }  
}