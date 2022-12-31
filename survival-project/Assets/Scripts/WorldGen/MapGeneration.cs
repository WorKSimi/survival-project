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
    public RuleTile wallTile;
    public RuleTile dirtTile;
    public RuleTile stoneTile;

    public GameObject tree;
    public GameObject caveEntrance;

    [Header("Dimensions")]
    public int width = 50;
    public int height = 50;
    public float scale = 1.0f;
    public Vector2 offset;

    [Header("Other Stuff")]
    public Grid worldGrid;

    [Header("Height Map")]
    public Wave[] heightWaves;
    public float[,] heightMap;

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
                //Instantiate tile on tilemap based on height value               
                var height = heightMap[x, y];

                if (height < 0.2f) //Water
                {
                    groundTilemap.SetTile(new Vector3Int(x, y, 0), waterTile);
                }
                else if (height < 0.25f) //Sand
                {
                    groundTilemap.SetTile(new Vector3Int(x, y, 0), sandTile);
                }
                else if (height < 0.6f) //Grass
                {
                    groundTilemap.SetTile(new Vector3Int(x, y, 0), grassTile);

                    if (Random.value >= 0.96) //4 percent chance
                    {
                        Instantiate(tree, new Vector3Int(x, y, 0), Quaternion.identity);
                    }
                }
                else if (height < 0.7f) //Dirt Ground
                {
                    groundTilemap.SetTile(new Vector3Int(x, y, 0), dirtTile);

                    while (currentCaveEntrances < maxCaveEntrances)
                    {
                        SpawnCaveEntrance(x, y);
                    }
                }
                else if (height < 1.0f) //Dirt Wall
                {
                    wallTilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
                }
            }
        }

        //while (currentCaveEntrances < maxCaveEntrances) //While there are less current caves than max caves
        //{
        //    for (int x = 0; x < width; ++x) //Cycle through noise map
        //    {
        //        for (int y = 0; y < height; ++y) //Cycle through the noise map
        //        {
        //            if (height < 0.7f && height > 6.0f) //If the height of the map is less then 7 but greater than 6 (place caves can spawn)
        //            {
        //                SpawnCaveEntrance(x, y);
        //            }
        //        }
        //    }
        //}

    }

    void SpawnCaveEntrance(int x, int y)
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
                caveEntrances = GameObject.FindGameObjectsWithTag("CaveEntrances");

                foreach (GameObject caveEntrance in caveEntrances) //Do this for every cave entrance in the array
                {
                    float minDistance = 30f; //Min distance is 30f;
                    float dist = Vector3.Distance(caveEntrance.transform.position, new Vector3(x, y, 0)); //Get distance between each cave entrance and where to place

                    if (dist < minDistance) //If the distance between the cave entrance and the spot is less then the minimum
                    {
                        Debug.Log("Cannot Place Cave, too close"); //DONT place entrance
                    }
                    else if (dist >= minDistance) //If distance is greater then...
                    {

                        Instantiate(caveEntrance, new Vector3(x, y, 0), Quaternion.identity); //Spawn a cave entrance
                        currentCaveEntrances++;  //Add 1 to current cave count
                    }
                }
            }
    }  
}