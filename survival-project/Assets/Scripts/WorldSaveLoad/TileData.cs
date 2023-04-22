using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;
public class WorldData
{
    public string WorldName; //World data saves a world name
    public List<TileData> tilesOnMapList; //World data saves a list of every tile
}

public class TileData
{
    public Vector3Int tileLocation; //Tile location
    public TileType tileType;
    public TilemapType tilemapType;
}
