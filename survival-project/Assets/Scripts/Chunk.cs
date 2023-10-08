using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    //Data for each chunk
    public bool chunkActive;
    public bool placingBlock;
    public bool toBeEnabled;

    public int chunkCordX;
    public int chunkCordY;

    public bool chunkInitialLoad = false; //Flag for if chunk is being loaded for the first time. False by default.

    public int xMax;
    public int xMin;
    public int yMax;
    public int yMin;

    public int CavexMax;
    public int CavexMin;
    public int CaveyMax;
    public int CaveyMin;

    public void EnableChunk() //This function turns the chunk on
    {
        gameObject.SetActive(true);
        chunkActive = true;
    }

    public void DisableChunk()
    {
        gameObject.SetActive(false);
        chunkActive = false;
    }
}
