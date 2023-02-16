using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;

public class UseItemManager : NetworkBehaviour
{
    private Grid grid; //Game World Grid
    private Tilemap wallTilemap; //The wall tilemap of the world
    private Tilemap interactiveTilemap; //The interactive tilemap of the world 
    private Tilemap groundTilemap;
    private Tilemap waterTilemap;

    public bool facingRight; //Bool for if the player is facing right
    public bool facingLeft; //Bool if the player is facing left

    public Camera playerCam;

    [SerializeField] private Tile hoverTile = null; //Tile that is used when you hover on a spot on grid
    [SerializeField] private RuleTile dryFarmTile; //Stores dry farm tile
    [SerializeField] private RuleTile wetFarmTile; //Stores wet farm tile

    [SerializeField] private GameObject thisPlayer; //This players game object

    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform projectileRotationObject; //Rotation for the arrow projectile for bows
    [SerializeField] private Transform defaultProjectileRotation; //Default rotation for a projectile

    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject slashPrefab;

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

    private PlayerHealth playerHealth;

    private Vector3Int mousePos; //Mouse position on grid
    private Vector3Int previousMousePos; //Previous mouse position on grid

    private Vector3 mouseWorldPos; //Mouse position in world
    private Transform hoveredWall; //Wall object your mouse is currently hovering over
    private Transform m_transform;
    private SpriteRenderer swordSprite;

    private Vector2 boxSize = new Vector2(1.5f, 1.5f); //Melee weapon hitbox size
    private Vector3 dir;

    private bool swungWeapon = false; //Flag for swinging the weapon

    void Awake()
    {
        var gridObject = GameObject.FindWithTag("Grid");
        grid = gridObject.GetComponent<Grid>();

        var wallmapObject = GameObject.FindWithTag("WallTilemap");
        wallTilemap = wallmapObject.GetComponent<Tilemap>();

        var groundMapObject = GameObject.FindWithTag("GroundTilemap");
        groundTilemap = groundMapObject.GetComponent<Tilemap>();

        var interactivemapObject = GameObject.FindWithTag("InteractiveTilemap");
        interactiveTilemap = interactivemapObject.GetComponent<Tilemap>();

        var watermapObject = GameObject.FindWithTag("WaterTilemap");
        waterTilemap = watermapObject.GetComponent<Tilemap>();

        m_transform = weaponAnchor.transform;
        swordSprite = weaponSprite.GetComponent<SpriteRenderer>();

        playerHealth = thisPlayer.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (!IsLocalPlayer) return; //If your not local player return
        if (!IsOwner) return; //If not owner return
        playerPos2 = thisPlayer.transform.position;
        mouseWorldPos = playerCam.ScreenToWorldPoint(Input.mousePosition); 
        playerPos = grid.WorldToCell(transform.position);
        mousePos = grid.WorldToCell(mouseWorldPos);

        if (IsInRange() == true)
        {
            interactiveTilemap.SetTile(previousMousePos, null);
            interactiveTilemap.SetTile(mousePos, hoverTile);
            previousMousePos = mousePos;
        }
        else if (IsInRange() == false)
        {
            interactiveTilemap.SetTile(mousePos, null);
        }

        FacingDirection();

        Vector2 direction = playerCam.ScreenToWorldPoint
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
    
    public bool IsInRange()
    {
        float maxRange = 3;
        float dist = Vector3Int.Distance(mousePos, playerPos);
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
        Vector3 mouseWorldPos = playerCam.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }

    //public bool PlayerDetector()
    //{
    //    var ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Create a Ray
    //    var hit = Physics2D.GetRayIntersection(ray, 50f); //Shoot a ray, see what it hits

    //    if (hit.transform.CompareTag("Player")) //If the ray hit a player
    //    {
    //        Debug.Log("Player found, cannot place block!");
    //        return true;
    //    }
    //    else
    //    {
    //        Debug.Log("No Player Found, can place block!");
    //        return false; 
    //    }
    //}

    private float attackRange = 0.5f; 
    private float attackRate = 2f; //How many times you can attack per second
    float nextAttackTime = 0f;

    public void UseAxe(double itemDamage)
    {
        Debug.Log("AXE USED!");
        if (Time.time >= nextAttackTime)
        {
            Collider2D[] hitTrees = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, treeLayers); // Detect trees in range of attack

            foreach (Collider2D tree in hitTrees) // Damage trees
            {
                tree.GetComponent<TreeLogic>().TakeDamage(itemDamage);
            }
            nextAttackTime = Time.time + 1f / attackRate; //Cooldown Stuff
        }
    }

    public void PlaceBlock(RuleTile ItemTile) //Takes in a ItemTile based on what your holding and places it
    {      
        Vector3Int mousePos = GetMousePosition(); //Gets mouse position                
        wallTilemap.SetTile(mousePos, ItemTile); //Sets tile on the tilemap where your mouse is
    }

    public void UsePick(double itemDamage)
    {
        if (Time.time >= nextAttackTime) //If not on cooldown
        {
            SwingTweenAnimation(); //Swing animation
            nextAttackTime = Time.time + 1f / attackRate; //Do cooldown stuff

            if (IsInRange()) //If your in range
            {
                var ray = playerCam.ScreenPointToRay(Input.mousePosition);
                var hit = Physics2D.GetRayIntersection(ray);
                hoveredWall = hit.transform;              
                hoveredWall.GetComponent<Wall>().TakeDamage(itemDamage);
                wallTilemap.SetTile(mousePos, null);
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

    public void UseSword(double itemDamage, GameObject projectilePrefab, float projectileSpeed, float projectileLifetime)
    {
        if (Time.time >= nextAttackTime)
        {
            if (IsHost)
            {
                if (projectilePrefab != null)
                {
                    LaunchProjectile(itemDamage, projectilePrefab, defaultProjectileRotation, projectileSpeed, projectileLifetime);
                }
                else Debug.Log("This weapon has no projectile, not firing");

                SwingTweenAnimation(); //Swing animation
                Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, boxSize, 1f, enemyLayers); // Detect enemies in range of attack
                foreach (Collider2D enemy in hitEnemies) // Damage enemies
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(itemDamage);
                }
                nextAttackTime = Time.time + 1f / attackRate;
            }
            else if (IsClient)
            {
                if (projectilePrefab != null)
                {
                    Vector3 fireLocation = firePoint.position;
                    Vector3 projectileDirection = firePoint.up;
                    Quaternion tempRotation = projectileRotationObject.rotation;
                    SwordSlashProjectileServerRpc(itemDamage, projectileSpeed, projectileLifetime, projectileDirection, tempRotation, fireLocation);                 
                }
                else Debug.Log("This weapon has no projectile, not firing");

                SwingTweenAnimation(); //Swing animation
                Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, boxSize, 1f, enemyLayers); // Detect enemies in range of attack
                foreach (Collider2D enemy in hitEnemies) // Damage enemies
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(itemDamage);
                }
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    public void UseFood(int healthHealed)
    {
        playerHealth.HealHealth(healthHealed);       
    }

    public void UseWaterCan()
    {
        var tile = groundTilemap.GetTile(mousePos);

        if (tile.name == "DryFarmTile") //If mouse is over dry farm land
        {
            groundTilemap.SetTile(mousePos, wetFarmTile); //Set dry tile to wet tile
        }
    }

    public void UseHoe()
    {
        var tile = groundTilemap.GetTile(mousePos);

        if (IsInRange())
        {
            if (tile.name == "GrassTile" || tile.name == "DirtTile") //If your over a grass OR dirt tile
            {
                groundTilemap.SetTile(mousePos, dryFarmTile); //Set tile at mouse to dry farm land
            }
        }
    }

    public void UseSeed(RuleTile cropToPlant)
    {
        wallTilemap.SetTile(mousePos, cropToPlant); //This sets wall tile to the crop
    }

    public bool MouseOverCropland()
    {
        var tile = groundTilemap.GetTile(mousePos);
        if (tile.name == "WetFarmTile" || tile.name == "DryFarmTile") //If tile mouse is on is either wet OR dry farmland
        {
            return true;
        }
        else return false;
    }

    public bool IsHealthFull()
    {
        if (playerHealth.currentHealth == playerHealth.maxHealth)
        {
            return true;
        }
        else return false;
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
    public void UseBow(double itemDamage, GameObject projectilePrefab, float projectileSpeed, float projectileLifetime)
    {
        if (Time.time >= nextAttackTime)
        {
            if (IsHost)
            {
                LaunchProjectile(itemDamage, projectilePrefab, projectileRotationObject, projectileSpeed, projectileLifetime); //Shoot the projectile
                nextAttackTime = Time.time + 1f / attackRate; //Do Cooldown
            }

            else if (IsClient)
            {
                Vector3 fireLocation = firePoint.position;
                Vector3 projectileDirection = firePoint.up;
                Quaternion tempRotation = projectileRotationObject.rotation;
                LaunchArrowProjectileServerRpc(itemDamage, projectileSpeed, projectileLifetime, projectileDirection, tempRotation, fireLocation);
                nextAttackTime = Time.time + 1f / attackRate; //Do Cooldown
            }
        }
    }

    private void LaunchProjectile(double itemDamage, GameObject projectilePrefab, Transform rotationObject, float projectileSpeed, float projectileLifetime)
    {
        Debug.Log("Start of launch function: " + projectileLifetime);
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, rotationObject.rotation);

        Debug.Log("Right before pass in: " + projectileLifetime);
        Projectile projectileScript = bullet.GetComponent<Projectile>();
        projectileScript.Projectiledamage = itemDamage;
        projectileScript.Projectilelifetime = projectileLifetime;
        projectileScript.StartDestructionCoroutine();

        bullet.GetComponent<NetworkObject>().Spawn();
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();       
        rb.AddForce(firePoint.up * projectileSpeed, ForceMode2D.Impulse);               
    }

    [ServerRpc]
    public void LaunchArrowProjectileServerRpc(double itemDamage,  float projectileSpeed, float projectileLifetime, Vector3 fireDirection, Quaternion quaternion, Vector3 spawnLocation)
    {
        GameObject bullet = Instantiate(arrowPrefab, spawnLocation, quaternion);
        bullet.GetComponent<NetworkObject>().Spawn();
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(fireDirection * projectileSpeed, ForceMode2D.Impulse);
        Projectile projectile = bullet.GetComponent<Projectile>();
        projectile.Projectiledamage = itemDamage;
        projectile.Projectilelifetime = projectileLifetime;
        projectile.StartDestructionCoroutine();
    }

    [ServerRpc]
    public void SwordSlashProjectileServerRpc(double itemDamage, float projectileSpeed, float projectileLifetime, Vector3 fireDirection, Quaternion quaternion, Vector3 spawnLocation)
    {
        GameObject bullet = Instantiate(slashPrefab, spawnLocation, quaternion);
        bullet.GetComponent<NetworkObject>().Spawn();
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(fireDirection * projectileSpeed, ForceMode2D.Impulse);
        Projectile projectile = bullet.GetComponent<Projectile>();
        projectile.Projectiledamage = itemDamage;
        projectile.Projectilelifetime = projectileLifetime;
        projectile.StartDestructionCoroutine();
    }

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, new Vector3 (1.5f, 1.5f, 0));
    }
}
