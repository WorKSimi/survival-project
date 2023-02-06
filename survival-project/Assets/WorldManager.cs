using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class WorldManager : NetworkBehaviour
{
    public WorldDataCollector worldDataCollector;
    public HostClientManager hostClientManager;
    public NetworkManager networkManager;
    public void Start()
    {
        GameObject hostClientObject = GameObject.FindWithTag("HostClientManager");
        hostClientManager = hostClientObject.GetComponent<HostClientManager>();

        NetworkManager.OnClientConnectedCallback += OnConnectedToServer;
    }

    public void LoadWorld()
    {
        if (hostClientManager.IsHost == true) //Host
        {
            //If your a host
            //Host the game right away
            NetworkManager.Singleton.StartHost();                 
            //Set all tiles on the map from the file
            worldDataCollector.LoadWorldData(worldDataCollector.file1);

            //Spawn every object in the network 
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>(); //Find all game objects in scene
            foreach (GameObject gameObject in allObjects)
            {
                if (gameObject.GetComponent<NetworkObject>()) //If the object has a network object component
                {
                    gameObject.GetComponent<NetworkObject>().Spawn(); //Spawn all objects
                }
            }
        }

        else if (hostClientManager.IsHost == false) //Client   
        {
            NetworkManager.Singleton.StartClient();     //Connect on Client        
        }       
    }

    public void OnConnectedToServer(ulong clientId)
    {
        AskForDataServerRpc(); //Send server a hello
    }

    [ServerRpc]
    public void AskForDataServerRpc() //Fired by client Ran on Server
    {
        LoadTilesOnClientClientRpc();
    }

    [ClientRpc] //Fired by server, executed on client
    public void LoadTilesOnClientClientRpc() //Load client from server RPC
    {
        worldDataCollector.LoadClientWorldData(worldDataCollector.file1);
    }
}
