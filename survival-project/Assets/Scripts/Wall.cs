using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Tilemaps;

public class Wall : MonoBehaviour
{
    [SerializeField] private bool isCrop;
    //[SerializeField] private bool isBlueberryBush; //Flag for if this is on a blueberry bush.
    //[SerializeField] private GameObject wood;
    [SerializeField] private double maxHealth = 5;

    [SerializeField] private AudioSource source; //Audio source for wall object
    [SerializeField] private AudioClip soundOnHit; //Sound to play when wall object is hit.
    [SerializeField] private GameObject soundOnDestroyObject;

    //[SerializeField] private bool isResourceNode;
    //[SerializeField] private int amountToDrop;
    //[SerializeField] private Grass grassScript;
    //private BlueberryBushLogic blueBerryBush;
    public InventoryItemData inventoryItemData;
    private Vector3Int thisPosition;
    public double currentHealth;

    [Header("First Item")]
    [SerializeField] private GameObject itemToDrop;
    [SerializeField] public int maxToDrop;
    [SerializeField] public int minToDrop;
    [SerializeField] public int dropChance;

    [Header("Second Item")]
    [SerializeField] private GameObject itemToDrop2;
    [SerializeField] public int maxToDrop2;
    [SerializeField] public int minToDrop2;
    [SerializeField] public int dropChance2;


    

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (soundOnHit != null) //if sound on hit exists
        {
            source.PlayOneShot(soundOnHit); //Play hit sound
        }
        currentHealth -= damage;
        Debug.Log("Wall Hit");
        //Die();
    }

    

    public void Die()
    {
        if (soundOnDestroyObject != null)
        {
            var go = Instantiate(soundOnDestroyObject, this.transform.position, Quaternion.identity);
        }


        thisPosition = Vector3Int.FloorToInt(this.transform.position);

        if (itemToDrop != null) //If the item to drop is something
        {
            var amountToDrop = Random.Range(minToDrop, maxToDrop); //Get random value between min and max.
            for (int i = 0; i < amountToDrop; i++)
            {               
                float randValue = Random.value; //Random value
                if (randValue >= dropChance) //If random value is greater than equal to drop chance
                {
                    var go = Instantiate(itemToDrop, this.transform.position, Quaternion.identity);
                    go.GetComponent<NetworkObject>().Spawn();
                }
            }
        }

        if (itemToDrop2 != null) //If the item to drop is something
        {
            var amountToDrop2 = Random.Range(minToDrop2, maxToDrop2); //Get random value between min and max.
            for (int i = 0; i < amountToDrop2; i++)
            {
                float randValue = Random.value; //Random value
                if (randValue >= dropChance2) //If random value is greater than equal to drop chance
                {
                    var go = Instantiate(itemToDrop, this.transform.position, Quaternion.identity);
                    go.GetComponent<NetworkObject>().Spawn();
                }
            }
        }

        if (isCrop == true)
        {
            Debug.Log("Do nothing, Crop death handled by crop script");
        }        

        //if (grassScript != null) //If this wall object is on grass
        //{
        //    grassScript.DisableAllStates(); //Turn into normal grass
        //}

        //if (isBlueberryBush) //If this is a blueberry bush
        //{
        //    var blueberryScript = this.gameObject.GetComponent<BlueberryBushLogic>();
        //    blueberryScript.DeactivateBlueberryBush();      
        //}

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

        else //Wall is not on grass
        {
            Destroy(this.gameObject); //Destroy this object locally.
        }
    }
}
