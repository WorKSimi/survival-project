using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;
public class WorldData
{
    public string WorldName; //World data saves a world name
    public float WorldSeed;
    public int[,] mapGridArray;
}

public class ChunkData
{
    public Vector3Int chunkLocation; //Chunk Location
    public int[,] chunkTiles;
}
