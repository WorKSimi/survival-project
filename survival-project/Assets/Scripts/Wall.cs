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

        GameObject go = this.gameObject;
        go.GetComponent<NetworkObject>().Spawn(); //On awake, spawn this object on network
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
            GameObject gameObject = Instantiate(wood, transform.position, Quaternion.identity); //Instantiate Item
            gameObject.GetComponent<NetworkObject>().Spawn(); //Spawn on network
        }
    }

    public void TakeDamage(double damage)
    {
        currentHealth -= damage;
        Debug.Log("Wall Hit");

        //if (currentHealth <= 0)
        //{
        //    Die();           
        //}
    }

    public void Die()
    {
        if (wood != null) //If the item to drop is something
        {
            if (IsHost)
            {
                for (int i = 0; i < amountToDrop; i++)
                {
                    GameObject gameObject = Instantiate(wood, transform.position, Quaternion.identity); //Instantiate Item
                    gameObject.GetComponent<NetworkObject>().Spawn(); //Spawn on network
                }
            }
            else if (IsClient)
            {
                SpawnItemsServerRpc();
            }
        }
        if (isCrop == true)
        {
            Debug.Log("Do nothing, Crop death handled by crop script");
        }
    }

    public void DestroyAndDespawnThisObject()
    {
        this.gameObject.GetComponent<NetworkObject>().Despawn();
        Destroy(this.gameObject);
    }
}
