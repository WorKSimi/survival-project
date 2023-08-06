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

    //private GameObject gridObject;
    //private GameObject wallmapObject;
    //private Grid grid;
    //private Tilemap wallTilemap;

    public InventoryItemData inventoryItemData;

    [SerializeField] private GameObject groundTile;

    //private Vector3Int mousePos;
    private Vector3Int thisPosition;

    public double currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Wall Hit");

        //if (currentHealth <= 0)
        //{
        //    Die();           
        //}
    }

    [ServerRpc(RequireOwnership = false)] //Fired by client, executed on server
    public void DamageWallServerRpc(float damage)
    {
        TakeDamage(damage);
    }

    public void Die()
    {
        thisPosition = Vector3Int.FloorToInt(this.transform.position);

        if (wood != null) //If the item to drop is something
        {
            for (int i = 0; i < amountToDrop; i++)
            {
                GameObject gameObject = Instantiate(wood, transform.position, Quaternion.identity); //Instantiate Item
                gameObject.GetComponent<NetworkObject>().Spawn(); //Spawn on network
            }
        }

        if (isCrop == true)
        {
            Debug.Log("Do nothing, Crop death handled by crop script");
        }

        if (groundTile != null) //If the object has a ground tile
        {
            var go = Instantiate(groundTile, thisPosition, Quaternion.identity); //Instantiate in normal ground tile
            go.GetComponent<NetworkObject>().Spawn(); //Spawn it in
        }

        this.gameObject.GetComponent<NetworkObject>().Despawn();
    }
}
