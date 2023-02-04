using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Tilemaps;

public class Wall : NetworkBehaviour
{
    [SerializeField] private bool isCrop;
    [SerializeField] private GameObject wood;    
    [SerializeField] private double maxHealth = 5;
    [SerializeField] private bool isResourceNode;
    [SerializeField] private int amountToDrop;  

    private GameObject gridObject;
    private GameObject wallmapObject;
    private Grid grid;
    private Tilemap wallTilemap;

    //private Vector3Int mousePos;
    private Vector3Int thisPosition;

    public double currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Awake()
    {
        gridObject = GameObject.FindWithTag("Grid");
        grid = gridObject.GetComponent<Grid>();

        wallmapObject = GameObject.FindWithTag("WallTilemap");
        wallTilemap = wallmapObject.GetComponent<Tilemap>();
        thisPosition = grid.WorldToCell(gameObject.transform.position);

        //GameObject go = this.gameObject;
        //go.GetComponent<NetworkObject>().Spawn(); //On awake, spawn this object on network
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyObjectServerRpc(ServerRpcParams serverRpcParams = default)
    {
        NetworkObject networkObject = this.gameObject.GetComponent<NetworkObject>();
        networkObject.Despawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnItemsServerRpc(ServerRpcParams serverRpcParams = default)
    {
        for (int i = 0; i < amountToDrop; i++)
        {
            NetworkObject networkObject = Instantiate(wood, transform.position, Quaternion.identity).GetComponent<NetworkObject>();
            networkObject.Spawn();
        }
    }

    public void TakeDamage(double damage)
    {
        currentHealth -= damage;
        Debug.Log("Wall Hit");

        if (currentHealth <= 0)
        {
            Die();           
        }
    }

    private void Die()
    {
        for (int i = 0; i < amountToDrop; i++) //Loop based on how many resources to drop
        {
            if (IsHost) //if host, spawn objects
            {
                GameObject go = Instantiate(wood, transform.position, Quaternion.identity); //Drop item, will drop 5
                go.GetComponent<NetworkObject>().Spawn();
            }
            if (IsClient) //if client, send rpc to spawn items
            {
                SpawnItemsServerRpc(); //If client, send the spawn function to server instead
            }
        }
        if (IsClient) //if your the client, send despawn method to server
        {
            DestroyObjectServerRpc(); //Send code to server to destroy it instead
        }
        else //If your the host, despawn normally
        {
            wallTilemap.SetTile(thisPosition, null); //Set wall tile at mouse to null
            NetworkObject networkObject = this.gameObject.GetComponent<NetworkObject>();
            networkObject.Despawn();
        }
       
        if (isCrop == true)
        {
            Debug.Log("Do nothing, Crop death handled by crop script"); 
        }
    }
}
