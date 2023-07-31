using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    //Data for each chunk

    public int chunkCordX;
    public int chunkCordY;

    public int xMax;
    public int xMin;

    public int yMax;
    public int yMin;

    public void EnableChunk() //This function turns the chunk on
    {
        gameObject.SetActive(true);
    }

    public void DisableChunk()
    {
        gameObject.SetActive(false);
    }
}
