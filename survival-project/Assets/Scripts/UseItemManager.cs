using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UseItemManager : MonoBehaviour
{
    [SerializeField] private GameObject gridObject;
    [SerializeField] private GameObject wallmapObject;
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap wallTilemap;

    void Awake()
    {
        gridObject = GameObject.FindWithTag("Grid");
        grid = gridObject.GetComponent<Grid>();

        wallmapObject = GameObject.FindWithTag("WallTilemap");
        wallTilemap = wallmapObject.GetComponent<Tilemap>();
    }

    void Update()
    {
        
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

    private Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }


    public void PlaceBlock(RuleTile ItemTile) //Takes in a ItemTile based on what your holding and places it
    {
        Debug.Log("Trying to place Block");
        Vector3Int mousePos = GetMousePosition(); //Gets mouse position

        wallTilemap.SetTile(mousePos, ItemTile); //Sets tile on the tilemap where your mouse is

        //RemoveFromStack(1);
        //Finally, remove 1 from the item stack.
        
    }

    public void MineBlock(double itemDamage)
    {

    }

}
