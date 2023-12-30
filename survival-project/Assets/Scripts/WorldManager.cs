using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport.Relay;
using Unity.Netcode.Transports.UTP;

public class WorldManager : NetworkBehaviour
{
    //public static WorldManager Instance { get; private set; }

    [SerializeField] private TMP_Text joinCodeText;
    [SerializeField] private int maxConnections = 8;
    public WorldDataCollector worldDataCollector;
    public HostClientManager hostClientManager;
    public NetworkManager networkManager;
    public ChunkController chunkController;
    //public String JoinCode { get; private set; }
    private String JoinCode;
    public String clientJoinCode;
    public void Start()
    {
        GameObject hostClientObject = GameObject.FindWithTag("HostClientManager");
        hostClientManager = hostClientObject.GetComponent<HostClientManager>();

        NetworkManager.OnClientConnectedCallback += OnConnectedToServer;
    }



    public async void LoadWorld()
    {
        if (hostClientManager.IsHost == true) //If your Host
        {
            //First, Start on Relay
            Allocation allocation;

            try
            {
                allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
                
            }
            catch (Exception e)
            {
                Debug.LogError($"Relay create allocation request failed {e.Message}");
                throw;
            }

            Debug.Log($"server: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
            Debug.Log($"server: {allocation.AllocationId}");

            try
            {
                JoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                Debug.Log(JoinCode);
                joinCodeText.text = JoinCode;
            }
            catch
            {
                Debug.LogError("Relay get join code request failed");
                throw;
            }

            var relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            //If your a host
            //Host the game right away
            NetworkManager.Singleton.StartHost();                 
            //Set all tiles on the map from the file
            worldDataCollector.LoadWorldData(worldDataCollector.file1);

            ////Spawn every object in the network 
            //GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>(); //Find all game objects in scene
            //foreach (GameObject gameObject in allObjects)
            //{
            //    if (gameObject.GetComponent<NetworkObject>()) //If the object has a network object component
            //    {
            //        gameObject.GetComponent<NetworkObject>().Spawn(); //Spawn all objects
            //    }
            //}           
        }



        else if (hostClientManager.IsHost == false) //Client   
        {
            clientJoinCode = hostClientManager.clientJoinCode;
            joinCodeText.text = clientJoinCode;
            StartClient(clientJoinCode);        
        }       
    }



    public async void StartClient(string joinCode)
    {
        JoinAllocation allocation;

        try
        {
            allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        }
        catch
        {
            Debug.LogError("Relay get join code request failed");
            throw;
        }

        Debug.Log($"client: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
        Debug.Log($"host: {allocation.HostConnectionData[0]} {allocation.HostConnectionData[1]}");
        Debug.Log($"client: {allocation.AllocationId}");

        var relayServerData = new RelayServerData(allocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        NetworkManager.Singleton.StartClient();
    }

    public void OnConnectedToServer(ulong clientId) //Called on Client when you have successfully connected to server
    {
        if (IsHost) return; //If your the host, ABORT! DONT DO THIS SHIT!
        if (IsServer) return; //If your the server, ALSO ABORT!

        Debug.Log("Client connected, asking server form info");
        AskForDataServerRpc(); //Send server a hello
        //We need to tell the host that a new player has joined, so players found must be set to false. This will cause it to
        //Grab the players again and have chunks work with all
    }

    [ServerRpc(RequireOwnership = false)] //Fired by client, executed on server
    public void AskForDataServerRpc() //Fired by client Ran on Server
    {
        Debug.Log("Ask for Data RPC!");
        worldDataCollector.LoadClientWorldData(worldDataCollector.file1);
    }

    //[ClientRpc] //Fired by server, executed on client
    //public void LoadTilesOnClientClientRpc() //Load client from server RPC
    //{
    //    worldDataCollector.LoadClientWorldData(worldDataCollector.file1);
    //}

    //private void Update()
    //{
    //    Example_KeepingConnectionAlive();
    //}

    ////Call the below regularly, e.g., in Monobehaviour.Update()
    //private void Example_KeepingConnectionAlive()
    //{
    //    // Update the NetworkDrivers regularly to ensure the host/player is kept online.
    //    if (HostDriver.IsCreated && isRelayServerConnected)
    //    {
    //        HostDriver.ScheduleUpdate().Complete();

    //        //Accept incoming client connections
    //        while (HostDriver.Accept() != default(NetworkConnection))
    //        {
    //            Debug.Log("Accepted an incoming connection.");
    //        }
    //    }

    //    if (PlayerDriver.IsCreated && clientConnection.IsCreated)
    //    {
    //        PlayerDriver.ScheduleUpdate().Complete();

    //        //Resolve event queue
    //        NetworkEvent.Type eventType;
    //        while ((eventType = clientConnection.PopEvent(PlayerDriver, out _)) != NetworkEvent.Type.Empty)
    //        {
    //            if (eventType == NetworkEvent.Type.Connect)
    //            {
    //                Debug.Log("Client connected to the server");
    //            }
    //            else if (eventType == NetworkEvent.Type.Disconnect)
    //            {
    //                Debug.Log("Client got disconnected from server");
    //                clientConnection = default(NetworkConnection);
    //            }
    //        }
    //    }
    //}
}
