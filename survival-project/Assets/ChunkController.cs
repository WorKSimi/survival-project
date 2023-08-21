using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ChunkController : NetworkBehaviour
{
    public NetworkManager networkManager;
    public GameObject[] worldChunksHolder;
    private List<PlayerChunk> otherPlayers = new List<PlayerChunk>();
    public bool chunksLoaded = false; //Bool for if chunks are loaded. This is off by default.
    public bool playersFound = false; //Bool for if player is found, off by default
    private Chunk originChunk;

    private void Start()
    {
        StartCoroutine(SearchForPlayers());
    }

    
    private struct PlayerChunk
    {
        public GameObject player;
        public Chunk currentChunk;
        public Chunk previousChunk;
    };

    private IEnumerator SearchForPlayers()
    {
        while (true)
        {
            yield return new WaitForSeconds(3); //Wait 3 seconds

            if (chunksLoaded == true)
            {
                originChunk = worldChunksHolder[0].GetComponent<Chunk>();

                GetNonLocalPlayers();
                ChunkReset();
                DisableEnableChunks();
            }      
        }
    }


    private void GetNonLocalPlayers()
    {
        otherPlayers.Clear(); //Clear list

        GameObject[] tempHolder = GameObject.FindGameObjectsWithTag("Player"); //Find objects with player tag
        
        foreach (var player in tempHolder)
        {
            PlayerChunk playerChunk = new PlayerChunk();

            playerChunk.player = player; //Set player as this player

            if (playerChunk.currentChunk == null) //If no current chunk
            {
                playerChunk.previousChunk = originChunk; //Prev chunk is the OG
            }
            else //If there IS a current chunk
            {
                playerChunk.previousChunk = playerChunk.currentChunk; //Previous chunk is the current chunk
            }
        
            playerChunk.currentChunk = GetPlayerChunk(player.transform.position.x, player.transform.position.y, playerChunk); //Set player current chunk as this
            otherPlayers.Add(playerChunk);              
        }        
    }

   
    private Chunk GetPlayerChunk(float x, float y, PlayerChunk player)
    {
        //For every chunk, check with the players cords and see if it is greater than the minimum and less than the maximum  
        foreach (var chunkObject in worldChunksHolder)
        {   
            var chunk = chunkObject.GetComponent<Chunk>();

            if (chunk.xMin < x && chunk.xMax > x && chunk.yMin < y && chunk.yMax > y)
            {
                return chunk; //Get Chunk
            }
        }
        return player.previousChunk; //If cant get the right chunk, return previous one.
    }

    private void ChunkReset()
    {
        foreach (var chunkObject in worldChunksHolder)
        {
            var chunk = chunkObject.GetComponent<Chunk>();
            chunk.toBeEnabled = false;
        }
    }

    private void DisableEnableChunks()
    {       
        foreach (var chunkObject in worldChunksHolder)
        {
            foreach (var player in otherPlayers)
            {
                var currentChunk = player.currentChunk;
                Chunk chunk = chunkObject.GetComponent<Chunk>();

                if (chunk == currentChunk)
                {
                    chunk.EnableChunk(); //Current Chunk
                    chunk.toBeEnabled = true;
                }
                else if (chunk.chunkCordX == currentChunk.chunkCordX + 1 && chunk.chunkCordY == currentChunk.chunkCordY)
                {
                    chunk.EnableChunk(); //Chunk to the Right
                    chunk.toBeEnabled = true;
                }
                else if (chunk.chunkCordX == currentChunk.chunkCordX - 1 && chunk.chunkCordY == currentChunk.chunkCordY)
                {
                    chunk.EnableChunk(); //Chunk to the Left
                    chunk.toBeEnabled = true;
                }
                else if (chunk.chunkCordX == currentChunk.chunkCordX && chunk.chunkCordY == currentChunk.chunkCordY + 1)
                {
                    chunk.EnableChunk(); //Chunk to the Above
                    chunk.toBeEnabled = true;
                }
                else if (chunk.chunkCordX == currentChunk.chunkCordX && chunk.chunkCordY == currentChunk.chunkCordY - 1)
                {
                    chunk.EnableChunk(); //Chunk to the Down
                    chunk.toBeEnabled = true;
                }
                else if (chunk.chunkCordX == currentChunk.chunkCordX + 1 && chunk.chunkCordY == currentChunk.chunkCordY + 1)
                {
                    chunk.EnableChunk(); //Chunk to the Up Right
                    chunk.toBeEnabled = true;
                }
                else if (chunk.chunkCordX == currentChunk.chunkCordX - 1 && chunk.chunkCordY == currentChunk.chunkCordY - 1)
                {
                    chunk.EnableChunk(); //Chunk to the Down Left
                    chunk.toBeEnabled = true;
                }
                else if (chunk.chunkCordX == currentChunk.chunkCordX + 1 && chunk.chunkCordY == currentChunk.chunkCordY - 1)
                {
                    chunk.EnableChunk(); //Chunk to the Down Right
                    chunk.toBeEnabled = true;
                }
                else if (chunk.chunkCordX == currentChunk.chunkCordX - 1 && chunk.chunkCordY == currentChunk.chunkCordY + 1)
                {
                    chunk.EnableChunk(); //Chunk to the Up Left
                    chunk.toBeEnabled = true;
                }
                else if (chunk.toBeEnabled == true)
                {
                    chunk.EnableChunk(); 
                }
                else
                {
                    chunk.DisableChunk();
                }
            }
        }
    }
}