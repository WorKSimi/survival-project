using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;

public class MapGeneration : NetworkBehaviour
{
    [Header("Surface Tiles")]
    public GameObject waterTile;
    public GameObject deepWaterTile;
    public GameObject sandTile;
    public GameObject grassTile;
    public GameObject grassWall;
    public GameObject dirtTile;
    public GameObject tinOreWall;
    public GameObject caveEntrance;
    public GameObject caveExit;

    [Header("Beach Tiles")]
    public GameObject palmTree;
    public GameObject seaShell;

    [Header("Underground Tiles")]
    public GameObject pitTile;
    public GameObject stoneTile;
    public GameObject stoneWall;

    [Header("Object Tiles")]
    //public GameObject redShroomTile;
    //public GameObject brownShroomTile;
    //public GameObject blueberryTile;
    //public RuleTile caveEntranceTile;
    //public GameObject treeTile;
    //public RuleTile flintTile;
    //public RuleTile stoneNodeTile;

    [Header("Dimensions")]
    public int width;
    public int height;
    public int falloffSize = 100;
    public float scale = 1.0f;
    public Vector2 offset;
    public int chunkSize;

    [Header("Other Stuff")]

    public bool useFalloff;
    public int worldOffset = 0; //Use this to determine where in the world this island will spawn
    private int caveOffset = 600; //Spawn cave 600 units away from the normal world gen.
    public int[,] GridMap;
    public GameObject chunkHolder;
    public GameObject caveChunkHolder;
    public GameObject chunkPrefab;
    public GameObject chunkController;
    public GameObject[] worldChunks;
    public GameObject[] caveChunks;

    [Header("Height Map")]
    public Wave[] heightWaves;
    public float[,] heightMap;
    float[,] falloffMap;

    //private Tilemap wallTilemap; //Surface wall tilemap
    //private Tilemap groundTilemap; //Surface ground tilemap
    //private Tilemap waterTilemap; //Tilemap for water
    //private Tilemap carpetTilemap; //Surface carpet tilemap

    //private Tilemap undergroundWallTilemap; //Underground wall tilemap
    //private Tilemap undergroundGroundTilemap; //Underground ground tilemap
    //private Tilemap undergroundCarpetTilemap; //Underground carpet tilemap

    private GameObject[] caveEntrances; //Creates an array to store cave entrances
    private int currentCaveEntrances;
    private int maxCaveEntrances = 8;
    bool firstcaveSpawned;

    private GameObject localPlayer;
    private WorldLoadingScreen worldLoad;

    // ground, carpet, wall, interactive
    // Start is called before the first frame update
    void Start()
    {
        falloffMap = FalloffGenerator.GenerateFalloffMap(falloffSize);
        //fallofMap2 = FalloffGenerator.GenerateFalloffMap()
        firstcaveSpawned = false;
        currentCaveEntrances = 0;
    }

    private void AddTileToGridmap(int x, int y, int value)
    {
        GridMap[x, y] = value;
    }

    private int counter = 0;

    private bool RandomCheck(float seed, double chance, int count) //Take in seed, take in a number percent, take in count of times done it
    {
        int seedInt = (int)seed + count;
        int range = 1;
        System.Random random = new System.Random(seedInt);

        double num = random.NextDouble() * range;

        if (num >= chance)
        {
            counter++; //Increment count each time
            return true;
        }
        else
        {
            counter++; //Increment count each time
            return false;
        }    
    }



    public IEnumerator GenerateForestSurface(float seedNumber)
    {
        Debug.Log("World Generation has Begun!");

        var seed = seedNumber;
        
        heightWaves[0].seed = seed;
        heightWaves[1].seed = seed + 500;

        GridMap = new int[width, height];

        heightMap = NoiseGenerator.Generate(width, height, scale, heightWaves, offset); //height map

        worldLoad = NetworkManager.LocalClient.PlayerObject.gameObject.GetComponent<WorldLoadingScreen>();
        NetworkManager.LocalClient.PlayerObject.gameObject.GetComponent<PlayerNetwork>().state = PlayerNetwork.State.Loading; //Set player to loading (lock movement).

        GenerateChunks();

        int chunkCounter = 0;
        int max = worldChunks.Length;

        foreach (GameObject chunk in worldChunks)
        {
            var chunkScript = chunk.GetComponent<Chunk>();
            {
                for (int x = chunkScript.xMin; x <= chunkScript.xMax; x++)
                {
                    for (int y = chunkScript.yMin; y <= chunkScript.yMax; y++)
                    {
                        if (useFalloff)
                        {
                            heightMap[x, y] = Mathf.Clamp01(heightMap[x, y] - falloffMap[x, y]);
                        }

                        var height = heightMap[x, y];
                        var newX = (x );
                        var newY = (y );

                        if (height < 0.05f) //Deep Water
                        {
                            var go = Instantiate(deepWaterTile, new Vector3(newX, newY, 0), Quaternion.identity);
                            go.transform.parent = chunk.transform;
                            //AddTileToGridmap(newX, newY, 1);
                        }
                        else if (height < 0.2f) //Water
                        {
                            var go = Instantiate(waterTile, new Vector3(newX, newY, 0), Quaternion.identity);
                            go.transform.parent = chunk.transform;                           
                        }
                        else if (height < 0.3f) //Sand
                        {
                            if (RandomCheck(seed, 0.99, counter)) //Spawn Palm Tree
                            {
                                var go = Instantiate(palmTree, new Vector3(newX, newY, 0), Quaternion.identity);
                                go.transform.parent = chunk.transform;

                                var go2 = Instantiate(sandTile, new Vector3(newX, newY, 0), Quaternion.identity);
                                go2.transform.parent = chunk.transform;
                            }
                            else if(RandomCheck(seed, 0.99, counter)) //Spawn sea shell
                            {
                                var go = Instantiate(seaShell, new Vector3(newX, newY, 0), Quaternion.identity);
                                go.transform.parent = chunk.transform;

                                var go2 = Instantiate(sandTile, new Vector3(newX, newY, 0), Quaternion.identity);
                                go2.transform.parent = chunk.transform;
                            }
                            else //Spawn sand with nothing
                            {
                                var go = Instantiate(sandTile, new Vector3(newX, newY, 0), Quaternion.identity);
                                go.transform.parent = chunk.transform;
                            }                                        
                        }
                        else if (height < 0.5f) //Grass
                        {                          
                            if (RandomCheck(seed, 0.97, counter)) //Spawn Tree
                            {
                                var go = Instantiate(grassTile, new Vector3(newX, newY, 0), Quaternion.identity);
                                go.transform.parent = chunk.transform;
                                go.GetComponent<Grass>().EnableTree();
                            }

                            else if (RandomCheck(seed, 0.995, counter)) //Spawn red mushroom
                            {
                                var go = Instantiate(grassTile, new Vector3(newX, newY, 0), Quaternion.identity);
                                go.transform.parent = chunk.transform;
                                go.GetComponent<Grass>().EnableRedMushroom();
                            }

                            else if (RandomCheck(seed, 0.995, counter)) //Spawn brown mushroom
                            {
                                var go = Instantiate(grassTile, new Vector3(newX, newY, 0), Quaternion.identity);
                                go.transform.parent = chunk.transform;
                                go.GetComponent<Grass>().EnableBrownMushroom();
                            }

                            else if (RandomCheck(seed, 0.990, counter)) //Spawn stone node
                            {
                                var go = Instantiate(grassTile, new Vector3(newX, newY, 0), Quaternion.identity);
                                go.transform.parent = chunk.transform;
                                go.GetComponent<Grass>().EnableStoneNode();
                            }

                            else if (RandomCheck(seed, 0.995, counter)) //Spawn flint node
                            {
                                var go = Instantiate(grassTile, new Vector3(newX, newY, 0), Quaternion.identity);
                                go.transform.parent = chunk.transform;
                                go.GetComponent<Grass>().EnableFlintNode();
                            }

                            ////else if (RandomCheck(seed, 0.995, counter)) //Spawn blueberry bush
                            ////{
                            ////    var go = Instantiate(blueberryTile, new Vector3(newX, newY, 0), Quaternion.identity);
                            ////    go.transform.parent = chunk.transform;
                            ////}
                            else //Spawn normal Grass
                            {
                                var go = Instantiate(grassTile, new Vector3(newX, newY, 0), Quaternion.identity);
                                go.transform.parent = chunk.transform;
                                go.GetComponent<Grass>().DisableAllStates(); 
                            }                           
                        }
                        else if (height < 0.6f) //Dirt Ground
                        {
                            var go = Instantiate(dirtTile, new Vector3(newX, newY, 0), Quaternion.identity);
                            go.transform.parent = chunk.transform;
                            //AddTileToGridmap(newX, newY, 5);
                            TrySpawnCaveEntrance(newX, newY, chunkScript);
                        }
                        else if (height < 1.0f) //Wall
                        {
                            if (RandomCheck(seed, 0.95, counter)) //Spawn tin ore
                            {
                                SpawnOreVein(newX, newY, chunk) ;
                            }
                            else //If tin pass fails, spawn a normal wall
                            {
                                var go = Instantiate(grassWall, new Vector3(newX, newY, 0), Quaternion.identity); //Grass Wall
                                go.transform.parent = chunk.transform;

                                var go2 = Instantiate(dirtTile, new Vector3(newX, newY, 0), Quaternion.identity); //Dirt Ground
                                go.transform.parent = chunk.transform;
                                //AddTileToGridmap(newX, newY, 6);
                            }
                        }
                    }
                }                 
            }
            chunk.GetComponent<Chunk>().DisableChunk();

            chunkCounter++;
            float amount = ((float)chunkCounter / (float)max) * 100;
            worldLoad.UpdateLoadBar(amount);
            yield return null; //Wait 1 frame

        } //OPTIMIZE THIS LATER ^^^^
        if (chunkController != null) //If theres a chunk controller.
        {
            var chunkcontrollerScript = chunkController.GetComponent<ChunkController>();
            chunkcontrollerScript.worldChunksHolder = worldChunks; //Set the chunk holder in the controller.
        }
        worldLoad.SurfaceLoaded();

        yield return StartCoroutine(GenerateForestUnderground(seedNumber)); //When surface is made, do cave! 
    }

    private void SpawnOreVein(float x, float y, GameObject chunkObject) //This function generates ore veins. Its currently very simple
    {
        var go = Instantiate(tinOreWall, new Vector3(x, y, 0), Quaternion.identity); //Spawn ore 
        go.transform.parent = chunkObject.transform;

        var go2 = Instantiate(dirtTile, new Vector3(x, y, 0), Quaternion.identity);
        go2.transform.parent = chunkObject.transform;
    }

    private void GenerateChunks()
    {
        int chunkCount = -1;
        var value1 = width * height;
        var value2 = value1 / chunkSize;
        var chunkAmount = value2 / chunkSize;
        worldChunks = new GameObject[chunkAmount]; //Array size is equal to amount of chunks

        for (int a = 0; a < 16; a++)
        {
            for (int b = 0; b < 16; b++)
            {
                chunkCount++;
                GameObject chunk = Instantiate(chunkPrefab, transform.position, Quaternion.identity);
                chunk.name = chunkCount.ToString();
                chunk.transform.parent = chunkHolder.transform;
                worldChunks[chunkCount] = chunk;
                var chunkData = chunk.GetComponent<Chunk>();

                chunkData.chunkCordX = a;
                chunkData.chunkCordY = b;

                chunkData.xMin = (32 * a);
                chunkData.xMax = (32 * a) + 31;

                chunkData.yMin = (32 * b);
                chunkData.yMax = (32 * b) + 31;
            }
        }
    }

    private void GenerateCaveChunks()
    {
        int chunkCount = -1;
        var value1 = width * height;
        var value2 = value1 / chunkSize;
        var chunkAmount = value2 / chunkSize;
        caveChunks = new GameObject[chunkAmount]; //Array size is equal to amount of chunks

        for (int a = 0; a < 16; a++)
        {
            for (int b = 0; b < 16; b++)
            {
                chunkCount++;
                GameObject chunk = Instantiate(chunkPrefab, transform.position, Quaternion.identity);
                chunk.name = "Cave_" + chunkCount.ToString();
                chunk.transform.parent = caveChunkHolder.transform;
                caveChunks[chunkCount] = chunk;
                var chunkData = chunk.GetComponent<Chunk>();
                
                chunkData.chunkCordX = a;
                chunkData.chunkCordY = b;

                chunkData.CavexMin = (32 * a);
                chunkData.CavexMax = (32 * a) + 31;

                chunkData.CaveyMin = (32 * b);
                chunkData.CaveyMax = (32 * b) + 31;

                chunkData.xMin = (chunkData.CavexMin + caveOffset);
                chunkData.xMax = (chunkData.CavexMax + caveOffset);

                chunkData.yMin += chunkData.CaveyMin;
                chunkData.yMax += chunkData.CaveyMax;

                chunkData.DisableChunk();
            }
        }
    }

    public IEnumerator GenerateForestUnderground(float seedNumber)
    {
        Debug.Log("Cave Generation has Begun!");

        var seed = seedNumber;

        heightWaves[0].seed = seed + 100;
        heightWaves[1].seed = seed + 1000;

        GridMap = new int[width, height];

        heightMap = NoiseGenerator.Generate(width, height, scale, heightWaves, offset); //height map

        GenerateCaveChunks();

        int chunkCounter = 0;
        int max = caveChunks.Length;

        foreach (GameObject chunk in caveChunks)
        {
            var chunkScript = chunk.GetComponent<Chunk>();
            {
                for (int x = chunkScript.CavexMin; x <= chunkScript.CavexMax; x++)
                {
                    for (int y = chunkScript.CaveyMin; y <= chunkScript.CaveyMax; y++)
                    {                       
                        var height = heightMap[x, y];
                        var newX = (x + caveOffset);
                        var newY = (y);
                        //var pos = new Vector3(x, y, 0);

                        if (height < 0.1f) //Empty Pit
                        {
                            var go = Instantiate(pitTile, new Vector3(newX, newY, 0), Quaternion.identity);
                            go.transform.parent = chunk.transform;
                        }

                        else if (height < 0.4f) //Open Space
                        {
                            var go = Instantiate(stoneTile, new Vector3(newX, newY, 0), Quaternion.identity);
                            go.transform.parent = chunk.transform;
                        }

                        else if (height < 1.0f) //Closed Space
                        {
                            var go = Instantiate(stoneWall, new Vector3(newX, newY, 0), Quaternion.identity);
                            go.transform.parent = chunk.transform;

                            var go2 = Instantiate(stoneTile, new Vector3(newX, newY, 0), Quaternion.identity);
                            go2.transform.parent = chunk.transform;
                        }
                    }
                }
            }
            chunkCounter++;
            float amount = ((float)chunkCounter / (float)max) * 100;
            worldLoad.UpdateLoadBar(amount);

            yield return null;
        }

        SpawnCaveExits(); //Spawn cave exits

        if (chunkController != null) //If theres a chunk controller.
        {
            var chunkcontrollerScript = chunkController.GetComponent<ChunkController>();
            chunkcontrollerScript.caveChunksHolder = caveChunks;
            chunkcontrollerScript.chunksLoaded = true;
        }
        StartCoroutine(worldLoad.AllLoadingComplete());
        NetworkManager.LocalClient.PlayerObject.gameObject.GetComponent<PlayerNetwork>().state = PlayerNetwork.State.Normal; //Set player to Normal

        Debug.Log("Cave Gen Finished!");
    }

    void TrySpawnCaveEntrance(int x, int y, Chunk chunk) //This function handles spawning cave entrances in the world
    {
        if (firstcaveSpawned == false) //First cave has not been spawned
        {
            var go = Instantiate(caveEntrance, new Vector3(x, y, 0), Quaternion.identity);
            //go.transform.parent = chunk.transform;
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
                }
            }
            if (currentCaveEntrances >= maxCaveEntrances) shouldSpawn = false; //If current cave is more than max, dont spawn anymore.
            if (shouldSpawn == true)
            {
                Instantiate(caveEntrance, new Vector3(x, y, 0), Quaternion.identity);
                currentCaveEntrances++;  //Add 1 to current cave count
            }
        }
    }

    private void SpawnCaveExits()
    {
        caveEntrances = GameObject.FindGameObjectsWithTag("CaveEntrance"); //Get all cave entrances
        List<Vector3Int> caveExitSpots = new List<Vector3Int>();

        foreach (var caveEntrance in caveEntrances) //For each cave entrance
        {
            var caveExitPosition = new Vector3Int((int)caveEntrance.transform.position.x + caveOffset, (int)caveEntrance.transform.position.y, 0);
            caveExitSpots.Add(caveExitPosition);
        }

        foreach (var caveExitSpot in caveExitSpots)
        {
            var x = caveExitSpot.x;
            var y = caveExitSpot.y;

            foreach (var chunkObj in caveChunks) //Foreach cave chunk
            {
                var chunk = chunkObj.GetComponent<Chunk>();
                if (chunk.xMin < x && chunk.xMax > x && chunk.yMin < y && chunk.yMax > y) //if cave exit is to be placed in this chunk
                {
                    chunk.EnableChunk(); //Turn the chunk on.

                    var vec2 = new Vector2Int(x, y); //Get position in vector 2    
                    RaycastHit2D hit = Physics2D.Raycast(vec2, Vector2.up, 0.1f); //Shoot raycast at that spot to place exit

                    if (hit == false) //If NOTHING was hit
                    {
                        Instantiate(caveExit, caveExitSpot, Quaternion.identity); //Spawn cave exit at that location
                        chunk.DisableChunk(); //Turn chunk back off.
                    }
                    else if (hit.transform.CompareTag("Wall")) //If raycast hits a wall.
                    {
                        hit.transform.GetComponent<Wall>().DeleteWall(); //Kill the wall
                        Instantiate(caveExit, caveExitSpot, Quaternion.identity); //Spawn cave exit at that location
                        chunk.DisableChunk(); //Turn chunk back off.
                    }
                    else //If raycast hits not a wall 
                    {
                        Instantiate(caveExit, caveExitSpot, Quaternion.identity); //Spawn cave exit at that location
                        chunk.DisableChunk(); //Turn chunk back off.
                    }    
                }
            }
        }   
    } 
}