using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public WorldDataCollector worldDataCollector;
    private void Start() //When the scene is loaded
    {
        worldDataCollector.LoadWorldData(worldDataCollector.file1); //Load File 1
    }
}
