using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;

public class MapGeneration : NetworkBehaviour
{
    [Header("Tiles")]
    public GameObject waterTile;
    public GameObject sandTile;
    public GameObject grassTile;
    public RuleTile grassWallTile;
    public GameObject dirtTile;
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
    public int width;
    public int height;
    public int falloffSize = 100;
    public float scale = 1.0f;
    public Vector2 offset;
    public int chunkSize;

    [Header("Other Stuff")]
    public Grid surfaceGrid; //Grid for the surface of the world
    public Grid undergroundGrid; //Grid for the underground, it is at a lower z axis
    public bool useFalloff;
    public int worldOffset = 0; //Use this to determine where in the world this island will spawn
    public int caveOffset = 0;
    public int[,] GridMap;
    public GameObject chunkHolder;
    public GameObject[] worldChunks;

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
    private int maxCaveEntrances = 5;
    bool firstcaveSpawned;

    // ground, carpet, wall, interactive
    // Start is called before the first frame update
    void Start()
    {
        falloffMap = FalloffGenerator.GenerateFalloffMap(falloffSize);
        firstcaveSpawned = false;
        currentCaveEntrances = 0;
    }

    private void AddTileToGridmap(int x, int y, int value)
    {
        GridMap[x, y] = value;
    }

    public void GenerateForestSurface()
    {
        GenerateChunks();
        float seed = Random.Range(-9999f, 9999f); //seed for world gen
        int seedInt = (int)seed;
        Random.InitState(seedInt);
        heightWaves[0].seed = seed;
        heightWaves[1].seed = seed + 500;

        GridMap = new int[width, height];

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
                    var go = Instantiate(waterTile, new Vector3Int(newX, newY, 0), Quaternion.identity);
                    ChunkCheck(x, y, go);
                    AddTileToGridmap(newX, newY, 1);                  
                }
                else if (height < 0.25f) //Sand
                {                    
                    var go = Instantiate(sandTile, new Vector3Int(newX, newY, 0), Quaternion.identity);
                    ChunkCheck(x, y, go);
                    AddTileToGridmap(newX, newY, 2);
                }
                else if (height < 0.5f) //Grass
                {
                    var go = Instantiate(grassTile, new Vector3Int(newX, newY, 0), Quaternion.identity);
                    ChunkCheck(x, y, go);
                    AddTileToGridmap(newX, newY, 3);

                    //if (Random.value >= 0.98) //If 2 percent chance pass
                    //{
                    //    //wallTilemap.SetTile(new Vector3Int(newX, newY, 0), treeTile); //Spawn tree on tile
                    //}
                    //else if (Random.value >= 0.99) 
                    //{
                    //    //wallTilemap.SetTile(new Vector3Int(newX, newY, 0), flintTile); //Spawn Flint node on tile
                    //}
                    //else if (Random.value >= 0.99)
                    //{
                    //    //wallTilemap.SetTile(new Vector3Int(newX, newY, 0), stoneNodeTile); //Spawn Stone node on tile
                    //}
                    //else if (Random.value >= 0.995) 
                    //{
                    //    //wallTilemap.SetTile(new Vector3Int(newX, newY, 0), blueberryTile); //Spawn blueberry bush on tile
                    //}
                    //else if (Random.value >= 0.995) 
                    //{
                    //    //wallTilemap.SetTile(new Vector3Int(newX, newY, 0), redShroomTile); //Spawn Red Mushroom on tile
                    //}
                    //else if (Random.value >= 0.995) 
                    //{
                    //    //wallTilemap.SetTile(new Vector3Int(newX, newY, 0), brownShroomTile); //Spawn brown mushroom on tile
                    //}
                }
                else if (height < 0.6f) //Dirt Ground
                {
                    var go = Instantiate(dirtTile, new Vector3Int(newX, newY, 0), Quaternion.identity);
                    ChunkCheck(x, y, go);
                    AddTileToGridmap(newX, newY, 4);
                    //TrySpawnCaveEntrance(newX, newY);
                }
                else if (height < 1.0f) //Dirt Wall
                {
                    //Instantiate(dirtTile, new Vector3Int(newX, newY, 0), Quaternion.identity);
                    //wallTilemap.SetTile(new Vector3Int(newX, newY, 0), grassWallTile); //Set wall to grass wall
                    //if (Random.value >= 0.95) //5 percent chance each wall tile
                    //{
                    //    SpawnOreVein(newX, newY); //Gen an ore vein
                    //}
                }
            }
        }
    }

    //Create list of chunks

    private void GenerateChunks()
    {
        var value1 = width * height;
        var value2 = value1 / chunkSize;
        var chunkAmount = value2 / chunkSize;
        worldChunks = new GameObject[chunkAmount]; //Array size is equal to amount of chunks
        for (int i = 0; i < chunkAmount; i++)
        {
            GameObject chunk = new GameObject();
            chunk.name = i.ToString();
            chunk.transform.parent = chunkHolder.transform;
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

    //public void GenerateForestUnderground()
    //{
    //    heightWaves[0].seed = Random.Range(1.0f, 999.0f);
    //    heightWaves[1].seed = Random.Range(1.0f, 999.0f);

    //    heightMap = NoiseGenerator.Generate(width, height, scale, heightWaves, offset); //height map
    //    for (int x = 0; x < width; ++x)
    //    {
    //        for (int y = 0; y < height; ++y) //Cycle through the noise map
    //        {
    //            //Instantiate tile on tilemap based on height value               
    //            var height = heightMap[x, y];
    //            var newX = (x + caveOffset);
    //            var newY = (y - 50);

    //            if (height < 0.4f) //Open Space
    //            {
    //                groundTilemap.SetTile(new Vector3Int(newX, newY, 0), stoneTile); //Stone Floor
    //            }

    //            else if (height < 1.0f) //Closed Space
    //            {
    //                groundTilemap.SetTile(new Vector3Int(newX, newY, 0), stoneTile); //Set ground to stone
    //                wallTilemap.SetTile(new Vector3Int(newX, newY, 0), stoneWall); //Set wall to stone wall
    //            }
    //        }
    //    }
    //}

    //void SpawnOreVein(int x, int y) //This function generates ore veins. Its currently very simple
    //{
    //    wallTilemap.SetTile(new Vector3Int(x, y, 0), tinOre); //Spawn ore tile
    //    wallTilemap.SetTile(new Vector3Int(x-1, y, 0), tinOre); //Spawn ore tile to the right
    //    wallTilemap.SetTile(new Vector3Int(x, y-1, 0), tinOre); //Spawn ore tile down
    //}
    //void TrySpawnCaveEntrance(int x, int y) //This function handles spawning cave entrances in the world
    //{
    //    if (firstcaveSpawned == false) //First cave has not been spawned
    //    {
    //        wallTilemap.SetTile(new Vector3Int(x, y, 0), caveEntranceTile); //Spawn first cave entrance            
    //        currentCaveEntrances++; //Add 1 to current cave count
    //        firstcaveSpawned = true; //Set the first cave spawned variable to true
    //    }

    //    else if (firstcaveSpawned == true) //The first cave HAS been spawned
    //    {
    //        //Find all cave entrances and add to array
    //        caveEntrances = GameObject.FindGameObjectsWithTag("CaveEntrance");
    //        var shouldSpawn = true;
    //        foreach (GameObject ce in caveEntrances) //Do this for every cave entrance in the array
    //        {
    //            float minDistance = 50f; //Min distance caves can be from eachother;
    //            float dist = Vector3.Distance(ce.transform.position, new Vector3(x, y, 0)); //Get distance between each cave entrance and where to place

    //            if (dist < minDistance) //If the distance between the cave entrance and the spot is less then the minimum
    //            {
    //                shouldSpawn = false;
    //                Debug.Log("Cannot Place Cave, too close"); //DONT place entrance
    //            }
    //        }
    //        if (shouldSpawn == true)
    //        {
    //            wallTilemap.SetTile(new Vector3Int(x, y, 0), caveEntranceTile); //Spawn a cave entrance
    //            currentCaveEntrances++;  //Add 1 to current cave count
    //        }
    //    }
    //}  
}