using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UseItemManager : MonoBehaviour
{
    private Grid grid;
    private GameObject gridObject;

    private Tilemap wallTilemap;
    private GameObject wallObject;
    
    void Start()
    {
        gridObject = GameObject.FindWithTag("Grid");
        grid = gridObject.GetComponent<Grid>();

        wallObject = GameObject.FindWithTag("WallTilemap");
        wallTilemap = wallObject.GetComponent<Tilemap>();
    }

    void Update()
    {
        //Vector3Int mousePos = GetMousePosition();
    }

    private Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }

    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask treeLayers;

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
                tree.GetComponent<Tree>().TakeDamage(itemDamage);
            }

            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    public void PlaceBlock(Tile ItemTile)
    {
        Vector3Int mousePos = GetMousePosition(); //Gets mouse position

        wallTilemap.SetTile(mousePos, ItemTile); //Sets tile on the tilemap where your mouse is

        // Finally, remove 1 from the item stack.
    }

}
