using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Tilemaps;

public class Wall : MonoBehaviour
{
    [SerializeField] private GameObject wood;
    [SerializeField] private double maxHealth = 5;
    private GameObject gridObject;
    private GameObject wallmapObject;
    private Grid grid;
    private Tilemap wallTilemap;

    private Vector3 wallPos;
    private Vector3Int cellPosition;

    private double currentHealth;


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

    void Update()
    {   
            wallPos = transform.position;
            Debug.Log("Wall position" + wallPos);
            Vector3Int cellPosition = grid.WorldToCell(wallPos);
            Debug.Log("Cell Location" + cellPosition);
    }

    public void TakeDamage(double damage)
    {
        currentHealth -= damage;
        Debug.Log("Tree Hit!");
        //Play hurt animation

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Tree down!");
      
        Debug.Log("Tree death animation moment");

        wallTilemap.SetTile(cellPosition, null);

        Instantiate(wood, transform.position, Quaternion.identity); //Spawns dropped item from breaking wall

        Destroy(this.gameObject); //Destroys the game object
        
    }
}
