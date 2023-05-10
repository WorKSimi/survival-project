using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;

public class MapGeneration : NetworkBehaviour
{
    [Header("Tiles")]
    public RuleTile waterTile;
    public RuleTile sandTile;
    public RuleTile grassTile;
    public RuleTile grassWallTile;
    public RuleTile dirtTile;  
    public RuleTile tinOre;
    public RuleTile stoneTile;
    public RuleTile stoneWall;

    [Header("Object Tiles")]
    public RuleTile redShroomTile;
    public RuleTile brownShroomTile;
    public RuleTile blueberryTile;
    public RuleTile caveEntranceTile;
    public RuleTile treeTile;
    public RuleTile flintTile;
    public RuleTile stoneNodeTile;

    [Header("Dimensions")]
    public int width = 50;
    public int height = 50;
    public int falloffSize = 100;
    public float scale = 1.0f;
    public Vector2 offset;

    [Header("Other Stuff")]
    public Grid surfaceGrid; //Grid for the surface of the world
    public Grid undergroundGrid; //Grid for the underground, it is at a lower z axis
    public bool useFalloff;
    public int worldOffset = 0; //Use this to determine where in the world this island will spawn
    public int caveOffset = 0;

    [Header("Height Map")]
    public Wave[] heightWaves;
    public float[,] heightMap;
    float[,] falloffMap;

    private Tilemap wallTilemap; //Surface wall tilemap
    private Tilemap groundTilemap; //Surface ground tilemap
    private Tilemap waterTilemap; //Tilemap for water
    private Tilemap carpetTilemap; //Surface carpet tilemap

    private Tilemap undergroundWallTilemap; //Underground wall tilemap
    private Tilemap undergroundGroundTilemap; //Underground ground tilemap
    private Tilemap undergroundCarpetTilemap; //Underground carpet tilemap

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
    }

    void GetTilemaps()
    {
        var groundObject = GameObject.FindWithTag("GroundTilemap");
        var carpetObject = GameObject.FindWithTag("CarpetTilemap");
        var wallObject = GameObject.FindWithTag("WallTilemap");
        var waterObject = GameObject.FindWithTag("WaterTilemap");

        groundTilemap = groundObject.GetComponent<Tilemap>();
        carpetTilemap = carpetObject.GetComponent<Tilemap>();
        wallTilemap = wallObject.GetComponent<Tilemap>();
        waterTilemap = waterObject.GetComponent<Tilemap>();

        //var undergroundGroundObject = undergroundGrid.transform.GetChild(0);
        //var undergroundCarpetObject = undergroundGrid.transform.GetChild(1);
        //var undergroundWallObject = undergroundGrid.transform.GetChild(2);

        //undergroundGroundTilemap = undergroundGroundObject.GetComponent<Tilemap>();
        //undergroundCarpetTilemap = undergroundCarpetObject.GetComponent<Tilemap>();
        //undergroundWallTilemap = undergroundWallObject.GetComponent<Tilemap>();
    }

    public void GenerateForestSurface()
    {
        float seed = Random.Range(-9999f, 9999f); //seed for world gen
        int seedInt = (int)seed;
        Random.InitState(seedInt);
        heightWaves[0].seed = seed;
        heightWaves[1].seed = seed + 500;

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
                var newX = (x + worldOffset);
                var newY = (y + worldOffset);

                if (height < 0.2f) //Water
                {
                    waterTilemap.SetTile(new Vector3Int(newX, newY, 0), waterTile);
                }
                else if (height < 0.25f) //Sand
                {
                    groundTilemap.SetTile(new Vector3Int(newX, newY, 0), sandTile);
                }
                else if (height < 0.5f) //Grass
                {
                    groundTilemap.SetTile(new Vector3Int(newX, newY, 0), grassTile);
                    if (Random.value >= 0.98) //If 2 percent chance pass
                    {
                        wallTilemap.SetTile(new Vector3Int(newX, newY, 0), treeTile); //Spawn tree on tile
                    }
                    else if (Random.value >= 0.99) 
                    {
                        wallTilemap.SetTile(new Vector3Int(newX, newY, 0), flintTile); //Spawn Flint node on tile
                    }
                    else if (Random.value >= 0.99)
                    {
                        wallTilemap.SetTile(new Vector3Int(newX, newY, 0), stoneNodeTile); //Spawn Stone node on tile
                    }
                    else if (Random.value >= 0.995) 
                    {
                        wallTilemap.SetTile(new Vector3Int(newX, newY, 0), blueberryTile); //Spawn blueberry bush on tile
                    }
                    else if (Random.value >= 0.995) 
                    {
                        wallTilemap.SetTile(new Vector3Int(newX, newY, 0), redShroomTile); //Spawn Red Mushroom on tile
                    }
                    else if (Random.value >= 0.995) 
                    {
                        wallTilemap.SetTile(new Vector3Int(newX, newY, 0), brownShroomTile); //Spawn brown mushroom on tile
                    }
                }
                else if (height < 0.6f) //Dirt Ground
                {
                    groundTilemap.SetTile(new Vector3Int(newX, newY, 0), dirtTile);
                    TrySpawnCaveEntrance(newX, newY);
                }
                else if (height < 1.0f) //Dirt Wall
                {
                    groundTilemap.SetTile(new Vector3Int(newX, newY, 0), dirtTile); //Set ground to dirt
                    wallTilemap.SetTile(new Vector3Int(newX, newY, 0), grassWallTile); //Set wall to grass wall
                    if (Random.value >= 0.95) //5 percent chance each wall tile
                    {
                        SpawnOreVein(newX, newY); //Gen an ore vein
                    }
                }
            }
        }
    }

    public void GenerateForestUnderground()
    {
        heightWaves[0].seed = Random.Range(1.0f, 999.0f);
        heightWaves[1].seed = Random.Range(1.0f, 999.0f);

        heightMap = NoiseGenerator.Generate(width, height, scale, heightWaves, offset); //height map
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y) //Cycle through the noise map
            {
                //Instantiate tile on tilemap based on height value               
                var height = heightMap[x, y];
                var newX = (x + caveOffset);
                var newY = (y - 50);

                if (height < 0.4f) //Open Space
                {
                    groundTilemap.SetTile(new Vector3Int(newX, newY, 0), stoneTile); //Stone Floor
                }

                else if (height < 1.0f) //Closed Space
                {
                    groundTilemap.SetTile(new Vector3Int(newX, newY, 0), stoneTile); //Set ground to stone
                    wallTilemap.SetTile(new Vector3Int(newX, newY, 0), stoneWall); //Set wall to stone wall
                }
            }
        }
    }

    void SpawnOreVein(int x, int y) //This function generates ore veins. Its currently very simple
    {
        wallTilemap.SetTile(new Vector3Int(x, y, 0), tinOre); //Spawn ore tile
        wallTilemap.SetTile(new Vector3Int(x-1, y, 0), tinOre); //Spawn ore tile to the right
        wallTilemap.SetTile(new Vector3Int(x, y-1, 0), tinOre); //Spawn ore tile down
    }
    void TrySpawnCaveEntrance(int x, int y) //This function handles spawning cave entrances in the world
    {
        if (firstcaveSpawned == false) //First cave has not been spawned
        {
            wallTilemap.SetTile(new Vector3Int(x, y, 0), caveEntranceTile); //Spawn first cave entrance            
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

                if (dist < minDistance) //If the distance between the cave entrance and the spot is less then the minimum
                {
                    shouldSpawn = false;
                    Debug.Log("Cannot Place Cave, too close"); //DONT place entrance
                }
            }
            if (shouldSpawn == true)
            {
                wallTilemap.SetTile(new Vector3Int(x, y, 0), caveEntranceTile); //Spawn a cave entrance
                currentCaveEntrances++;  //Add 1 to current cave count
            }
        }
    }  
}