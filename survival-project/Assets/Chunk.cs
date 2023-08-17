using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    //Data for each chunk
    public bool chunkActive;
    public bool placingBlock; //Bool to keep track of if other script is using chunks to place blocks. If true, temporarily keep it away from chunk manager.

    public int chunkCordX;
    public int chunkCordY;

    public int xMax;
    public int xMin;

    public int yMax;
    public int yMin;

    public void EnableChunk() //This function turns the chunk on
    {
        gameObject.SetActive(true);
        chunkActive = true;
    }

    public void DisableChunk()
    {
        if (placingBlock == true) return; //If other scripts are using this chunk, temporarily stop it from being disabled.
        gameObject.SetActive(false);
        chunkActive = false;
    }
}
