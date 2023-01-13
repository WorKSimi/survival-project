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

    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform projectileRotationObject;
    [SerializeField] private GameObject arrowPrefab;

    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask treeLayers;
    [SerializeField] private LayerMask wallLayers;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private LayerMask interactableLayers;

    [SerializeField] private GameObject weaponSprite;
    [SerializeField] private GameObject weaponAnchor;
    [SerializeField] private Animator slashAnimator;

    private Vector3Int playerPos; //Player position on grid
    private Vector3 playerPos2; //Player normal position

    private Vector3Int mousePos; //Mouse position on grid
    private Vector3Int previousMousePos; //Previous mouse position on grid

    private Vector3 mouseWorldPos; //Mouse position in world
    private Transform hoveredWall; //Wall object your mouse is currently hovering over
    private Transform m_transform;
    private SpriteRenderer swordSprite;

    private Vector2 boxSize = new Vector2(1.5f, 1.5f);
    private Vector3 dir;

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

        //boxPoint = new Vector2(attackPoint.position.x, attackPoint.position.y);
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

        if (facingRight == true) // If your facing right, run this code for rotating weapon
        {
            Quaternion rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
            m_transform.rotation = rotation;
            swordSprite.flipX = false;
            swordSprite.flipY = false;

            if (swungWeapon == true)
            {
                rotation = Quaternion.AngleAxis(angle + 0, Vector3.forward);
                m_transform.rotation = rotation;                           
            }
        }
        else if (facingLeft == true) // If your facing left run this code for rotating weapon
        {
            Quaternion rotation = Quaternion.AngleAxis(angle + 0, Vector3.forward);
            m_transform.rotation = rotation;
            swordSprite.flipX = true;
            swordSprite.flipY = true;

            if (swungWeapon == true)
            {
                rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
                m_transform.rotation = rotation;
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
            dir = Vector3.back;
            facingLeft = false;
        }

        if (mouseWorldPos.x < playerPos2.x)
        {
            facingLeft = true;
            dir = Vector3.forward;
            facingRight = false;
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
        if (Time.time >= nextAttackTime) //If not on cooldown
        {
            SwingTweenAnimation(); //Swing animation
            nextAttackTime = Time.time + 1f / attackRate; //Do cooldown stuff

            if (IsInRange()) //If your in range
            {
                animator.SetTrigger("Attack"); // Play an attack animation
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var hit = Physics2D.GetRayIntersection(ray, 50f);
                hoveredWall = hit.transform;              
                hoveredWall.GetComponent<Wall>().TakeDamage(itemDamage);
            }
        }
    }

    public void UseRock(double itemDamage)
    {
        Debug.Log("The rock has been used");
        if (Time.time >= nextAttackTime)
        {
            SwingTweenAnimation(); //Swing animation

            //Hit Enemies
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, boxSize, 1f, enemyLayers); // Detect enemies in range of attack
            foreach (Collider2D enemy in hitEnemies) // Damage enemies
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(itemDamage);
            }

            //Hit Trees and Walls
            Collider2D[] hitWalls = Physics2D.OverlapBoxAll(attackPoint.position, boxSize, 1f, wallLayers); // Detect trees in range of attack
            foreach (Collider2D wall in hitWalls) // Damage trees
            {
                wall.GetComponent<Wall>().TakeDamage(itemDamage);
            }
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    public void UseSword(double itemDamage)
    {
        if (Time.time >= nextAttackTime)
        {
            SwingTweenAnimation(); //Swing animation

            animator.SetTrigger("Attack"); // Play an attack animation

            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, boxSize, 1f, enemyLayers); // Detect enemies in range of attack

            foreach (Collider2D enemy in hitEnemies) // Damage enemies
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(itemDamage);
            }

            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    private void SwingTweenAnimation()
    {      
        var degreeChange = 180f;
        var weaponDegreeChange = 360f;
        if (swungWeapon)
        {
            degreeChange *= -1f;
            weaponDegreeChange *= -1f;
        }
        slashAnimator.SetBool("isSlashing", true);
        var tween = LeanTween.rotateAround(weaponAnchor, dir, degreeChange, 0.2f).setEaseInOutQuart();
        LeanTween.rotateAround(weaponSprite, dir, weaponDegreeChange, 0.2f).setEaseInOutQuart();
        tween.setOnComplete(() => { swungWeapon = !swungWeapon; slashAnimator.SetBool("isSlashing", false); });
    }

    private float bulletForce = 15f;
    public void UseBow(double itemDamage, GameObject projectilePrefab)
    {
        if (Time.time >= nextAttackTime)
        {
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, projectileRotationObject.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            Arrow arrow = bullet.GetComponent<Arrow>();
            arrow.Arrowdamage = itemDamage;
            rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, new Vector3 (1.2f, 1.2f, 0));
    }
}
