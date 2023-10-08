using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int[,] GridMap;
    private int Width = 100;
    private int Height = 100;

    private void Start()
    {
        //GenerateGridmap();
    }

    private void GenerateGridmap()
    {
        GridMap = new int[Width, Height];
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                GridMap[i, j] = Random.Range(0, 10);
            }
        }
    }

    private void SpawnTileTest(int x, int y, int value)
    {
        GameObject g = new GameObject("X: " + x + "Y: " + y);
        g.transform.position = new Vector3(x - (Width - 0.5f), y - (Height - 0.5f));
    }
}
