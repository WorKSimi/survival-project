using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WorldManager : NetworkBehaviour
{
    public WorldDataCollector worldDataCollector;
    public HostClientManager hostClientManager;

    public void Start()
    {
        GameObject hostClientObject = GameObject.FindWithTag("HostClientManager");
        hostClientManager = hostClientObject.GetComponent<HostClientManager>();
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
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (GameObject gameObject in allObjects)
            {
                if (gameObject.GetComponent<NetworkObject>()) //If the object has a network object component
                {
                    gameObject.GetComponent<NetworkObject>().Spawn(); //Spawn that object
                }
            }
        }

        else if (hostClientManager.IsHost == false) //Client   
        {
            NetworkManager.Singleton.StartClient();     //Connect on Client        
        }       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadTilesOnClientClientRpc();
        }    
    }

    [ClientRpc] //Fired by server, executed on client
    public void LoadTilesOnClientClientRpc() //Load client from server RPC
    {
        worldDataCollector.LoadWorldData(worldDataCollector.file1);
        Debug.Log("RPC");
    }
}
