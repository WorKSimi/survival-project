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
    

    private void Start()
    {
        StartCoroutine(SearchForPlayers());
    }

    private struct PlayerChunk
    {
        public GameObject player;
        public Chunk currentChunk;
    };

    private IEnumerator SearchForPlayers()
    {
        while (true)
        {
            yield return new WaitForSeconds(3); //Wait 3 seconds

            GetNonLocalPlayers();
            DisableEnableChunks();
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
            playerChunk.currentChunk = GetPlayerChunk(player.transform.position.x, player.transform.position.y); //Set player current chunk as this
            otherPlayers.Add(playerChunk);              
        }        
        
        foreach (var player in otherPlayers)
        {
            if (player.currentChunk == null) return;
            Debug.Log(player.currentChunk.transform.name);
        }
    }

   
    private Chunk GetPlayerChunk(float x, float y)
    {
        //For every chunk, check with the players cords and see if it is greater than the minimum and less than the maximum  
        foreach (var chunkObject in worldChunksHolder)
        {
            var chunk = chunkObject.GetComponent<Chunk>();
            if (chunk.xMin < x && chunk.xMax > x && chunk.yMin < y && chunk.yMax > y)
            {
                return chunk;
            }           
        }
        return null;
    }

    private void DisableEnableChunks()
    {
        foreach (var player in otherPlayers)
        {
            var currentChunk = player.currentChunk;

            foreach (var chunkObject in worldChunksHolder)
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
}