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

    private Vector3Int mousePos;
    private Vector3 mouseWorldPos;

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
        Debug.Log("Death");
        Instantiate(wood, transform.position, Quaternion.identity); //Spawns dropped item from breaking wall
        mousePos = GetMousePosition();
        wallTilemap.SetTile(mousePos, null);
        Destroy(this.gameObject); //Destroys the game object 
    }

    private Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }
}
