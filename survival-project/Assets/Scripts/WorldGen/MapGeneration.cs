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

    public GameObject testTile0;
    public GameObject testTile1;
    public GameObject testTile2;
    public GameObject testTile3;

    [Header("Dimensions")]
    public int width;
    public int height;
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


    // ground, carpet, wall, interactive

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(worldGrid, new Vector3(0, 0, 0), Quaternion.identity);
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

        for (int x=0; x<width; ++x)
        {
            for (int y=0; y<height; ++y)
            {
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
                else if (height < 0.7f) //Grass 
                {
                    groundTilemap.SetTile(new Vector3Int(x, y, 0), grassTile);                   
                }
                else if (height <= 1.0f) //Dirt Wall
                {
                    groundTilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
                }

            }
        }
    }
}