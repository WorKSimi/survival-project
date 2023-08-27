using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Tilemaps;

public class Wall : MonoBehaviour
{
    [SerializeField] private bool isCrop;
    [SerializeField] private GameObject wood;
    [SerializeField] private double maxHealth = 5;
    [SerializeField] private bool isResourceNode;
    [SerializeField] private int amountToDrop;
    [SerializeField] private Grass grassScript;
    [SerializeField] private GameObject groundObject; //This ground spawns under wall when destroyed.

    //private GameObject gridObject;
    //private GameObject wallmapObject;
    //private Grid grid;
    //private Tilemap wallTilemap;

    public InventoryItemData inventoryItemData;

    //[SerializeField] private GameObject groundTile;

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
        //Die();
    }

    //[ServerRpc(RequireOwnership = false)] //Fired by client, executed on server
    //public void DamageWallServerRpc(float damage)
    //{
    //    TakeDamage(damage);
    //}

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

        if (grassScript != null) //If this wall object is on grass
        {
            grassScript.DisableAllStates(); //Turn into normal grass

            //Send message to all clients (except host) to also disable states.
            //Will have to update changed tiles on this position later to account for the state
        }
        else //Wall is not on grass
        {
            if (groundObject != null)
            {
                Instantiate(groundObject, thisPosition, Quaternion.identity);
            }
            Destroy(this.gameObject); //Destroy this object           
        }
    }   

    public void DeleteWall() //This function removes the object locally without any other stuff.
    {
        thisPosition = Vector3Int.FloorToInt(this.transform.position);

        if (isCrop == true) //If its a crop.
        {
            Debug.Log("Do nothing, Crop death handled by crop script");
        }

        if (grassScript != null) //If this wall object is on grass
        {
            grassScript.DisableAllStates(); //Turn into normal grass

            //Send message to all clients (except host) to also disable states.
            //Will have to update changed tiles on this position later to account for the state
        }

        else //Wall is not on grass
        {
            Destroy(this.gameObject); //Destroy this object locally.
        }
    }
}
