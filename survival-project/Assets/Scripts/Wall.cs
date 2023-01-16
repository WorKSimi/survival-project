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

    private GameObject gridObject;
    private GameObject wallmapObject;
    private Grid grid;
    private Tilemap wallTilemap;

    private Vector3Int mousePos;

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
        if (isResourceNode == true) //If this is a tree or other resource node
        {
            Debug.Log("Resource ballin");
            for (int i = 0; i < amountToDrop; i++) //Loop based on how many resources to drop
            {
                Instantiate(wood, transform.position, Quaternion.identity); //Drop item, will drop 5
            }
            GetMousePosition(); //Get mouse position
            wallTilemap.SetTile(mousePos, null); //Set wall tile at mouse to null
            Destroy(this.gameObject); //Destroy the tree
        }
        else if (isResourceNode == false)
        {
            Instantiate(wood, transform.position, Quaternion.identity); //Spawns dropped item from breaking wall
            GetMousePosition();
            wallTilemap.SetTile(mousePos, null);
            Destroy(this.gameObject); //Destroys the game object 
        }
        else if (isCrop == true)
        {
            Debug.Log("Crop death handled by crop script"); 
        }
    }

    private void GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = grid.WorldToCell(mouseWorldPos);
    }
}
