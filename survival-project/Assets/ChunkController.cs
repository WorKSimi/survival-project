using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ChunkController : NetworkBehaviour
{
    //We got the world splitting into chunks properly
    //We need a holder to hold all of these chunks
    public NetworkManager networkManager;
    public GameObject[] worldChunksHolder;

    public GameObject localPlayer; //Holds local player object
    public List<GameObject> otherPlayers = new List<GameObject>();

    private float playerXCord;
    private float playerYCord;

    public Chunk currentChunk; //Stores local players current chunk
    public string currentChunkName; //Stores name of the current chunk

    //public List<Chunk> currentChunks = new List<Chunk>(); //List to hold all current chunks for each player

    public bool chunksLoaded = false; //Bool for if chunks are loaded. This is off by default.
    public bool playersFound = false; //Bool for if player is found, off by default
    private bool coroutineStarted = false;

    private Chunk previousChunk;

    public void OnConnectedToServer(ulong clientId) //Called on Client when you have successfully connected to server
    {
        NewPlayerConnectedServerRpc();
    }

    [ServerRpc] //Fired by client / execute on server
    private void NewPlayerConnectedServerRpc()
    {
        playersFound = false; //Set this on host to players not found, it will re-find with new players that joined
        NewPlayerConnectedClientRpc();
    }

    [ClientRpc] //Fired by server / execute on client
    private void NewPlayerConnectedClientRpc()
    {
        if (IsHost) return; //If your the host dont do shit, its already done.
        playersFound = false; //Set this on all clients to players not found, it will re-find with new players that joined
    }

    private void Update()
    {
        if (chunksLoaded == true)
        {
            if (playersFound == false) //players not found yet
            {
                FindPlayers();
            }
            else if (playersFound == true) //If player is found, and chunks are loaded
            {
                ChunkManagement(); //Begin chunk management
            }
        }
    }

    private void FindPlayers()
    {
        localPlayer = networkManager.LocalClient.PlayerObject.gameObject;
        localPlayer.GetComponent<UseItemManager>().chunkController = this;
        GetNonLocalPlayers();
        playersFound = true;     
    }

    private void GetNonLocalPlayers()
    {
        GameObject[] tempHolder = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (var player in tempHolder)
        {
            if (player != localPlayer) //if the player is not the local player
            {
                otherPlayers.Add(player); //if the player is not the local one, add it to the list
            }           
        }
    }

    private void ChunkManagement()
    {
        GetPlayerChunks(); //Get player chunk
        DisableEnableChunks(); //Enable and disable the proper chunks          
    }

    private void GetPlayerChunks()
    {
        playerXCord = localPlayer.transform.position.x;
        playerYCord = localPlayer.transform.position.y;

        //For every chunk, check with the players cords and see if it is greater than the minimum and less than the maximum
        foreach (var chunkObject in worldChunksHolder)
        {
            var chunk = chunkObject.GetComponent<Chunk>();
            if (chunk.xMin < playerXCord && chunk.xMax > playerXCord && chunk.yMin < playerYCord && chunk.yMax > playerYCord)
            {
                currentChunk = chunk;
                currentChunkName = chunk.gameObject.name;
            }
        }

           
    }
    //Disable all chunks
    //Now, enable the chunks with cords in the cardinal and diagnol directions.

    private void DisableEnableChunks()
    {
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