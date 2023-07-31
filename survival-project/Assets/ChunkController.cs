using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkController : MonoBehaviour
{
    //We got the world splitting into chunks properly
    //We need a holder to hold all of these chunks
    public GameObject[] worldChunksHolder;
    private GameObject player; //Holds player object

    private float playerXCord;
    private float playerYCord;
    public Chunk currentChunk;

    public bool chunksLoaded = false; //Bool for if chunks are loaded. This is off by default.
    private bool playerFound = false; //Bool for if player is found, off by default
    private bool coroutineStarted = false;


    private void Update()
    {
        if (chunksLoaded == true)
        {
            if (playerFound == false) //player not found yet
            {
                player = GameObject.FindGameObjectWithTag("Player"); //Set the player object to the player
                playerFound = true; //Set player as found
            }
            else if (playerFound == true) //If player is found, and chunks are loaded
            {
                ChunkManagement(); //Begin chunk management
            }
        }
        else Debug.Log("Chunks not loaded, please wait.");
    }

    private void ChunkManagement()
    {
        GetPlayerChunk(); //Get player chunk
        DisableEnableChunks(); //Enable and disable the proper chunks            
    }

    private void GetPlayerChunk()
    {
        //Now, to find which chunk the player is in, take the players X and Y cord.
        playerXCord = player.transform.position.x;
        playerYCord = player.transform.position.y;

        //For every chunk, check with the players cords and see if it is greater than the minimum and less than the maximum
        foreach (var chunkObject in worldChunksHolder)
        {
            var chunk = chunkObject.GetComponent<Chunk>();
            if (chunk.xMin < playerXCord && chunk.xMax > playerXCord && chunk.yMin < playerYCord && chunk.yMax > playerYCord)
            {
                 currentChunk = chunk; //When you find the right chunk, set that as the players current chunk.
            }
        }
    }
    //Disable all chunks
    //Now, enable the chunks with cords in the cardinal and diagnol directions.

    private void DisableEnableChunks()
    {
        foreach (GameObject chunkObject in worldChunksHolder)
        {
            Chunk chunk = chunkObject.GetComponent<Chunk>();
            if (chunk == currentChunk)
            {
                chunk.EnableChunk(); //Current Chunk
            }
            else if (chunk.chunkCordX == currentChunk.chunkCordX + 1 && chunk.chunkCordY == currentChunk.chunkCordY)
            {
                chunk.EnableChunk(); //Chunk to the Right
            }
            else if (chunk.chunkCordX == currentChunk.chunkCordX - 1 && chunk.chunkCordY == currentChunk.chunkCordY)
            {
                chunk.EnableChunk(); //Chunk to the Left
            }
            else if (chunk.chunkCordX == currentChunk.chunkCordX && chunk.chunkCordY == currentChunk.chunkCordY + 1)
            {
                chunk.EnableChunk(); //Chunk to the Above
            }
            else if (chunk.chunkCordX == currentChunk.chunkCordX && chunk.chunkCordY == currentChunk.chunkCordY - 1)
            {
                chunk.EnableChunk(); //Chunk to the Down
            }
            else if (chunk.chunkCordX == currentChunk.chunkCordX + 1 && chunk.chunkCordY == currentChunk.chunkCordY + 1)
            {
                chunk.EnableChunk(); //Chunk to the Up Right
            }
            else if (chunk.chunkCordX == currentChunk.chunkCordX - 1 && chunk.chunkCordY == currentChunk.chunkCordY - 1)
            {
                chunk.EnableChunk(); //Chunk to the Down Left
            }
            else if (chunk.chunkCordX == currentChunk.chunkCordX + 1 && chunk.chunkCordY == currentChunk.chunkCordY - 1)
            {
                chunk.EnableChunk(); //Chunk to the Down Right
            }
            else if (chunk.chunkCordX == currentChunk.chunkCordX - 1 && chunk.chunkCordY == currentChunk.chunkCordY + 1)
            {
                chunk.EnableChunk(); //Chunk to the Up Left
            }
            else
            {
                chunk.DisableChunk();
            }
        }
    }
}