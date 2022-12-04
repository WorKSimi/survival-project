using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UseItemManager : MonoBehaviour
{
    private Grid grid;
    private GameObject gridObject;

    private GameObject wallmapObject;
    private Tilemap wallTilemap;

    private GameObject interactivemapObject;
    private Tilemap interactiveTilemap;

    [SerializeField] private Tile hoverTile = null;
    [SerializeField] private GameObject thisPlayer;

    private Vector3Int playerPos;
    private Vector3 playerPos2;

    private Vector3Int mousePos;
    private Vector3Int previousMousePos;

    private Vector3 mouseWorldPos;
    private Transform hoveredWall;

    void Awake()
    {
        gridObject = GameObject.FindWithTag("Grid");
        grid = gridObject.GetComponent<Grid>();

        wallmapObject = GameObject.FindWithTag("WallTilemap");
        wallTilemap = wallmapObject.GetComponent<Tilemap>();

        interactivemapObject = GameObject.FindWithTag("InteractiveTilemap");
        interactiveTilemap = interactivemapObject.GetComponent<Tilemap>();
    }

    void Update()
    {
        playerPos2 = thisPlayer.transform.position;
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerPos = grid.WorldToCell(transform.position);
        Vector3Int mousePos = GetMousePosition();

        if (IsInRange()) 
        {
            interactiveTilemap.SetTile(previousMousePos, null);
            interactiveTilemap.SetTile(mousePos, hoverTile);
            previousMousePos = mousePos;
        }
        else interactiveTilemap.SetTile(mousePos, null);
    }

    void DetectObject()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics2D.GetRayIntersection(ray, 10f, wallLayers);
        hoveredWall = hit.transform;
    }
    
    public bool IsInRange()
    {
        float maxRange = 10.5f;
        float dist = Vector3.Distance(mouseWorldPos, playerPos2);
        if (dist <= maxRange)
        {
            return true;
        }
        else return false; 
    }

    private Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }

    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask treeLayers;
    [SerializeField] private LayerMask wallLayers;

    private InventoryItemData inventoryItemData;
    private float attackRange = 0.5f; 
    private double attackDamage = 0.5;

    private float attackRate = 2f; //How many times you can attack per second
    float nextAttackTime = 0f;

    public void UseAxe(double itemDamage)
    {
        Debug.Log("AXE USED!");
        if (Time.time >= nextAttackTime)
        {
            animator.SetTrigger("Attack"); // Play an attack animation

            Collider2D[] hitTrees = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, treeLayers); // Detect trees in range of attack

            foreach (Collider2D tree in hitTrees) // Damage trees
            {
                tree.GetComponent<TreeLogic>().TakeDamage(itemDamage);
            }

            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    public void PlaceBlock(RuleTile ItemTile) //Takes in a ItemTile based on what your holding and places it
    {
        if (IsInRange()) 
        {
            Vector3Int mousePos = GetMousePosition(); //Gets mouse position
            wallTilemap.SetTile(mousePos, ItemTile); //Sets tile on the tilemap where your mouse is    
        }
    }

    public void UsePick(double itemDamage)
    {
        Debug.Log("PICK USED!");

        if (IsInRange())
        {
            if (Time.time >= nextAttackTime)
            {
                animator.SetTrigger("Attack"); // Play an attack animation

                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var hit = Physics2D.GetRayIntersection(ray, 10f, wallLayers);
                hoveredWall = hit.transform;
                hoveredWall.GetComponent<Wall>().TakeDamage(itemDamage);
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }
}
