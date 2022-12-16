using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UseItemManager : MonoBehaviour
{
    private Grid grid; //Game World Grid
    private GameObject gridObject; //Object holding game world grid

    private GameObject wallmapObject; //Object holding wall tilemap
    private Tilemap wallTilemap; //The wall tilemap of the world

    private GameObject interactivemapObject; //Object holding interactive tilemap
    private Tilemap interactiveTilemap; //The interactive tilemap of the world

    private bool facingRight; //Bool for if the player is facing right
    private bool facingLeft; //Bool if the player is facing left

    [SerializeField] private Tile hoverTile = null; //Tile that is used when you hover on a spot on grid
    [SerializeField] private GameObject thisPlayer; //This players game object

    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask treeLayers;
    [SerializeField] private LayerMask wallLayers;

    [SerializeField] private Transform weaponSprite;
    [SerializeField] private GameObject weaponAnchor;

    private Vector3Int playerPos; //Player position on grid
    private Vector3 playerPos2; //Player normal position

    private Vector3Int mousePos; //Mouse position on grid
    private Vector3Int previousMousePos; //Previous mouse position on grid

    private Vector3 mouseWorldPos; //Mouse position in world
    private Transform hoveredWall; //Wall object your mouse is currently hovering over
    private Transform m_transform;
    private SpriteRenderer swordSprite;

    private bool swungWeapon = false;

    void Awake()
    {
        gridObject = GameObject.FindWithTag("Grid");
        grid = gridObject.GetComponent<Grid>();

        wallmapObject = GameObject.FindWithTag("WallTilemap");
        wallTilemap = wallmapObject.GetComponent<Tilemap>();

        interactivemapObject = GameObject.FindWithTag("InteractiveTilemap");
        interactiveTilemap = interactivemapObject.GetComponent<Tilemap>();

        m_transform = weaponAnchor.transform;
        swordSprite = weaponSprite.GetComponent<SpriteRenderer>();
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
        else
        {
            interactiveTilemap.SetTile(mousePos, null);
        }

        FacingDirection();

        Vector2 direction = Camera.main.ScreenToWorldPoint
        (Input.mousePosition) - m_transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (facingRight == true)
        {
            Quaternion rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
            m_transform.rotation = rotation;
            swordSprite.flipX = false;
            swordSprite.flipY = false;

            if (swungWeapon == true)
            {
                rotation = Quaternion.AngleAxis(angle + 0, Vector3.forward);
                m_transform.rotation = rotation;
                swordSprite.flipX = true;
                swordSprite.flipY = true;
            }
        }
        else if (facingLeft == true)
        {
            Quaternion rotation = Quaternion.AngleAxis(angle + 0, Vector3.forward);
            m_transform.rotation = rotation;
            swordSprite.flipX = true;
            swordSprite.flipY = true;

            if (swungWeapon == true)
            {
                rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
                m_transform.rotation = rotation;
                swordSprite.flipX = false;
                swordSprite.flipY = false;
            }
        }

    }

    private void RTMouse()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint
        (Input.mousePosition) - m_transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (facingRight == true)
        {
            Quaternion rotation = Quaternion.AngleAxis(angle + 160, Vector3.forward);
            m_transform.rotation = rotation;
        }
        else if (facingLeft == true)
        {
            Quaternion rotation = Quaternion.AngleAxis(angle + 190, Vector3.forward);
            m_transform.rotation = rotation;
        }
    }

    private void FacingDirection()
    {
        if (mouseWorldPos.x > playerPos2.x)
        {
            facingRight = true;
            facingLeft = false;
        }

        if (mouseWorldPos.x < playerPos2.x)
        {
            facingRight = false;
            facingLeft = true;
        }
    }

    void DetectObject()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics2D.GetRayIntersection(ray, 10f, wallLayers);
        hoveredWall = hit.transform;
    }
    
    public bool IsInRange()
    {
        float maxRange = 13.2f;
        float dist = Vector3.Distance(mouseWorldPos, playerPos2);
        //Debug.Log(dist);
        if (dist <= maxRange)
        {
            return true;
        }
        else return false; 
    }

    public bool TileFound()
    {
        Vector3Int mousePos = GetMousePosition(); //Gets mouse position
        if (wallTilemap.GetTile(mousePos))
        {
            Debug.Log("Tile occupied, can not place block!");
            return true;
        }
        else
        {              
            Debug.Log("No Tile found, placing block");
            return false;
        }
    }

    private Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }

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
                var hit = Physics2D.GetRayIntersection(ray, 50f, wallLayers);
                hoveredWall = hit.transform;
                hoveredWall.GetComponent<Wall>().TakeDamage(itemDamage);
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    public void UseSword()
    {
        swungWeapon = !swungWeapon;
    }
}
