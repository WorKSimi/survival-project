using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Tilemaps;

public class Wall : MonoBehaviour
{
    [SerializeField] private bool isCrop;
    [SerializeField] private bool isBlueberryBush; //Flag for if this is on a blueberry bush.
    //[SerializeField] private GameObject wood;
    [SerializeField] private double maxHealth = 5;
    //[SerializeField] private bool isResourceNode;
    //[SerializeField] private int amountToDrop;
    [SerializeField] private Grass grassScript;

    //TODO - ADD IN DROP CHANCES AS WELL!
    [Header("First Item")]
    [SerializeField] private GameObject itemToDrop1;
    [SerializeField] private int amountToDrop1;

    [Header("Second Item")]
    [SerializeField] private GameObject itemToDrop2;
    [SerializeField] private int amountToDrop2;

    private BlueberryBushLogic blueBerryBush;
    public InventoryItemData inventoryItemData;
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

    

    public void Die()
    {
        thisPosition = Vector3Int.FloorToInt(this.transform.position);

        if (itemToDrop1 != null) //If the item to drop is something
        {
            for (int i = 0; i < amountToDrop1; i++)
            {
                GameObject gameObject = Instantiate(itemToDrop1, transform.position, Quaternion.identity); //Instantiate Item
                gameObject.GetComponent<NetworkObject>().Spawn(); //Spawn on network
            }
        }

        if (itemToDrop2 != null) //If the item to drop is something
        {
            for (int i = 0; i < amountToDrop2; i++)
            {
                GameObject gameObject = Instantiate(itemToDrop2, transform.position, Quaternion.identity); //Instantiate Item
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
        }

        if (isBlueberryBush) //If this is a blueberry bush
        {
            var blueberryScript = this.gameObject.GetComponent<BlueberryBushLogic>();
            blueberryScript.DeactivateBlueberryBush();      
        }

        else //Wall is not on grass
        {
            Destroy(this.gameObject); //Destroy this object           
        }
    }   

    public void DeleteWall() //This function removes the object locally without any other stuff. (THIS IS CALLED ON CLIENT)
    {
        thisPosition = Vector3Int.FloorToInt(this.transform.position);

        if (isCrop == true) //If its a crop.
        {
            Debug.Log("Do nothing, Crop death handled by crop script");
        }

        if (grassScript != null) //If this wall object is on grass
        {
            grassScript.DisableAllStates(); //Turn into normal grass
        }

        if (isBlueberryBush == true) //If client is hitting blueberry bush
        {
            var blueberryScript = this.gameObject.GetComponent<BlueberryBushLogic>();
            blueberryScript.ClientHitBlueberryBushServerRpc();
        }

        else //Wall is not on grass
        {
            Destroy(this.gameObject); //Destroy this object locally.
        }
    }
}
