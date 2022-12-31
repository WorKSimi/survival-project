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
    //private GameObject[] caveEntrances; //Creates an array to store cave entrances
    //private int currentCaveEntrances = 0;

    // ground, carpet, wall, interactive
    // Start is called before the first frame update
    void Start()
    {
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
                    if (Random.value >= 0.95) //5 percent chance
                    {
                        Instantiate(caveEntrance, new Vector3(x, y, 0), Quaternion.identity);
                    }
                }
                else if (height < 1.0f) //Dirt Wall
                {
                    wallTilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
                }
            }
        }
    }
}
    //void SpawnCaveEntrance(int x, int y)
    //{
    //    int maxEntrnaces = 5;

    //    if (currentCaveEntrances < maxEntrnaces)
    //    {
    //       Instantiate(caveEntrance, new Vector3(x, y, 0), Quaternion.identity);
    //       currentCaveEntrances++;
    //    }
    //    else if (currentCaveEntrances >= maxEntrnaces)
    //    {
    //       Debug.Log("Cave Limit Reached");
    //    }
    //}