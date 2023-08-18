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
    public bool healCooldownComplete;

    public ChunkController chunkController;

    public bool facingRight; //Bool for if the player is facing right
    public bool facingLeft; //Bool if the player is facing left
    public BlockDatabase blockDatabase; //Database of all tiles in game

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
    private PlayerBuffsDebuffs playerBuffsDebuffs;

    private Vector3Int mousePos; //Mouse position on grid
    private Vector3Int previousMousePos; //Previous mouse position on grid

    private Vector3 mouseWorldPos; //Mouse position in world
    private Transform hoveredWall; //Wall object your mouse is currently hovering over
    private Transform m_transform;
    private SpriteRenderer swordSprite;

    private Vector2 boxSize = new Vector2(1.5f, 1.5f); //Melee weapon hitbox size
    private Vector3 dir;

    public bool HoldingPickOrBlock = false; //Bool if your holding pickaxe or block. Set to false at the start

    private bool swungWeapon = false; //Flag for swinging the weapon

    void Awake()
    {
        healCooldownComplete = true;
        var gridObject = GameObject.FindWithTag("Grid");
        grid = gridObject.GetComponent<Grid>();

        //var wallmapObject = GameObject.FindWithTag("WallTilemap");
        //wallTilemap = wallmapObject.GetComponent<Tilemap>();

        //var groundMapObject = GameObject.FindWithTag("GroundTilemap");
        //groundTilemap = groundMapObject.GetComponent<Tilemap>();

        //var interactivemapObject = GameObject.FindWithTag("InteractiveTilemap");
        //interactiveTilemap = interactivemapObject.GetComponent<Tilemap>();

        //var watermapObject = GameObject.FindWithTag("WaterTilemap");
        //waterTilemap = watermapObject.GetComponent<Tilemap>();

        m_transform = weaponAnchor.transform;
        swordSprite = weaponSprite.GetComponent<SpriteRenderer>();

        playerHealth = thisPlayer.GetComponent<PlayerHealth>();
        playerBuffsDebuffs = thisPlayer.GetComponent<PlayerBuffsDebuffs>();
    }

    void Update()
    {
        if (!IsLocalPlayer) return; //If your not local player return
        if (!IsOwner) return; //If not owner return
        playerPos2 = thisPlayer.transform.position;
        mouseWorldPos = playerCam.ScreenToWorldPoint(Input.mousePosition);
        //playerPos = grid.WorldToCell(transform.position);
        //mousePos = grid.WorldToCell(mouseWorldPos);

        //if (IsInRange() == true) //If is in range is true
        //{
        //    if (HoldingPickOrBlock == true) //if your holding pick or block
        //    {
        //        interactiveTilemap.SetTile(previousMousePos, null);
        //        interactiveTilemap.SetTile(mousePos, hoverTile);
        //        previousMousePos = mousePos;
        //    }
        //}
        //else if (IsInRange() == false) //If not in range 
        //{
        //    interactiveTilemap.SetTile(mousePos, null); //Set interactive tilemap to null at mouse
        //    interactiveTilemap.SetTile(previousMousePos, null); //Set to null at previous mouse pos
        //}
        //if (HoldingPickOrBlock == false) //OR not holding block
        //{
        //    interactiveTilemap.SetTile(mousePos, null); //Set interactive tilemap to null at mouse
        //    interactiveTilemap.SetTile(previousMousePos, null); //Set to null at previous mouse pos
        //}
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
        var ray = playerCam.ScreenPointToRay(Input.mousePosition);
        var hit = Physics2D.GetRayIntersection(ray);

        if (hit.transform == null)
        {
            Debug.Log("Hit has no transform! No Object Found!");
            return false;
        }
        else
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Wall"))
            {
                Debug.Log("Wall found!");
                return true;
            }
            else return false;
        }
    }
    
    private void FindPlayersChunkController()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var player in players)
        {
            var itemManager = player.GetComponent<UseItemManager>();
            if (itemManager.chunkController == null)
            {
                itemManager.chunkController = GameObject.FindGameObjectWithTag("ChunkController").GetComponent<ChunkController>();
            }
        }
    }

    private Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = playerCam.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }

    
    private float attackRange = 0.5f;
    private float attackRate = 2f; //How many times you can attack per second
    float nextAttackTime = 0f;

    public void UseAxe(float itemDamage)
    {
        Debug.Log("AXE USED!");
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

    public void UsePick(float itemDamage)
    {
        FindPlayersChunkController();

        if (IsHost)
        {
            if (Time.time >= nextAttackTime) //If not on cooldown
            {
                SwingTweenAnimation(); //Swing animation
                nextAttackTime = Time.time + 1f / attackRate; //Do cooldown stuff

                if (IsInRange()) //If your in range
                {
                    var ray = playerCam.ScreenPointToRay(Input.mousePosition);
                    var hit = Physics2D.GetRayIntersection(ray);
                    var tilePosition = Vector3Int.FloorToInt(hit.transform.position);

                    hoveredWall = hit.transform;
                    var wallScript = hoveredWall.GetComponent<Wall>();

                    wallScript.TakeDamage(itemDamage);

                    if (wallScript.currentHealth <= 0)
                    {
                        wallScript.Die();
                       
                        //string chunkName = chunkController.currentChunkName;
                        //int chunkInt = int.Parse(chunkName);

                        KillWallClientRpc(tilePosition, 0);
                    }
                }
            }
        }
        if (IsClient)
        {
            if (Time.time >= nextAttackTime) //If not on cooldown
            {
                SwingTweenAnimation(); //Swing animation
                nextAttackTime = Time.time + 1f / attackRate; //Do cooldown stuff

                if (IsInRange()) //If your in range
                {
                    FindPlayersChunkController();

                    var ray = playerCam.ScreenPointToRay(Input.mousePosition);
                    var hit = Physics2D.GetRayIntersection(ray);
                    var tilePosition = Vector3Int.FloorToInt(hit.transform.position);

                    //string chunkName = chunkController.currentChunkName;
                    //int chunkInt = int.Parse(chunkName);

                    DamageWallServerRpc(tilePosition, itemDamage, 1); //Communicate to server to damage wall                   
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)] //Launched by client, ran on server
    private void DamageWallServerRpc(Vector3Int position, float damage, int chunkNumber)
    {
        var vec2 = (Vector2Int)position; //Turn position to vector 2
       
        Debug.Log(vec2);

        FindPlayersChunkController();

        GameObject chunkTileIn = chunkController.worldChunksHolder[chunkNumber];

        var chunkScript = chunkTileIn.GetComponent<Chunk>();
        chunkScript.placingBlock = true; //Placing a block on this chunk! Dont disable yet!
        chunkScript.EnableChunk(); //Turn the chunk on so it can be accessed.

        RaycastHit2D hit; //Variable for racyast
        hit = Physics2D.Raycast(vec2, Vector2.up, 0.1f); //Set hit to raycast
        
        if (hit.transform.CompareTag("Wall"))
        {
            var wallScript = hit.transform.GetComponent<Wall>();
            wallScript.TakeDamage(damage);

            if (wallScript.currentHealth <= 0)
            {
                wallScript.Die(); //Kill wall on host end. Spawn in item.
                KillWallClientRpc(position, chunkNumber); //RPC to remove wall on all clients EXCEPT HOST! Its already done here.
                chunkScript.placingBlock = false; //Block placement done, so it can work as normal again               
            }
            chunkScript.placingBlock = false; //Block placement done, so it can work as normal again
        }           
        chunkScript.placingBlock = false; //Block placement done, so it can work as normal again
    }

    [ClientRpc] //Launched by server, ran on clients
    private void KillWallClientRpc(Vector3Int position, int chunkNumber)
    {
        if (IsHost) return; //If your the host, dont do this.

        var vec2 = (Vector2Int)position; //Turn position to vector 2
       
        GameObject chunkTileIn = chunkController.worldChunksHolder[chunkNumber];
        var chunkScript = chunkTileIn.GetComponent<Chunk>();
        chunkScript.placingBlock = true; //Block placement in progress, dont turn off chunk no matter what!
        chunkScript.EnableChunk();

        RaycastHit2D hit; //Variable for racyast
        hit = Physics2D.Raycast(vec2, Vector2.up, 0.1f); //Set hit to raycast
        
        if (hit.transform.CompareTag("Wall"))
        {
            var wallScript = hit.transform.GetComponent<Wall>();
            wallScript.DeleteWall();
            chunkScript.placingBlock = false; //Block placement done, so it can work as normal again
        }
        chunkScript.placingBlock = false;
    }

    public void UseRock(float itemDamage)
    {
        Debug.Log("The rock has been used");
        if (Time.time >= nextAttackTime)
        {
            SwingTweenAnimation(); //Swing animation

            if (IsHost)
            {
                if (Time.time >= nextAttackTime) //If not on cooldown
                {
                    Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, boxSize, 1f, enemyLayers); // Detect enemies in range of attack
                    foreach (Collider2D enemy in hitEnemies) // Damage enemies
                    {
                        enemy.GetComponent<EnemyHealth>().TakeDamage(itemDamage);
                    }

                    nextAttackTime = Time.time + 1f / attackRate; //Do cooldown stuff

                    if (IsInRange()) //If your in range
                    {
                        var ray = playerCam.ScreenPointToRay(Input.mousePosition);
                        var hit = Physics2D.GetRayIntersection(ray);
                        hoveredWall = hit.transform;
                        var wallScript = hoveredWall.GetComponent<Wall>();
                        wallScript.TakeDamage(itemDamage);
                    }
                }
            }
            if (IsClient)
            {
                if (Time.time >= nextAttackTime) //If not on cooldown
                {
                    Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, boxSize, 1f, enemyLayers); // Detect enemies in range of attack
                    foreach (Collider2D enemy in hitEnemies) // Damage enemies
                    {
                        enemy.GetComponent<EnemyHealth>().TakeDamageServerRpc(itemDamage);
                    }

                    nextAttackTime = Time.time + 1f / attackRate; //Do cooldown stuff

                    if (IsInRange()) //If your in range
                    {
                        var ray = playerCam.ScreenPointToRay(Input.mousePosition);
                        var hit = Physics2D.GetRayIntersection(ray);
                        hoveredWall = hit.transform;
                        var wallScript = hoveredWall.GetComponent<Wall>();
                        //wallScript.DamageWallServerRpc(itemDamage);
                    }
                }
            }
        }
    }

    public void UseSword(float itemDamage)
    {
        if (IsHost)
        {
            if (Time.time >= nextAttackTime) //If not on cooldown
            {
                SwingTweenAnimation(); //Swing animation
                Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, boxSize, 1f, enemyLayers); // Detect enemies in range of attack
                foreach (Collider2D enemy in hitEnemies) // Damage enemies
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(itemDamage);
                }
                nextAttackTime = Time.time + 1f / attackRate; //Do cooldown stuff
            }
        }
        if (IsClient)
        {
            if (Time.time >= nextAttackTime) //If not on cooldown
            {
                SwingTweenAnimation(); //Swing animation
                Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, boxSize, 1f, enemyLayers); // Detect enemies in range of attack
                foreach (Collider2D enemy in hitEnemies) // Damage enemies
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamageServerRpc(itemDamage);
                }
                nextAttackTime = Time.time + 1f / attackRate; //Do cooldown stuff
            }
        }
    }

    public void UseFood(int healthHealed)
    {
        playerHealth.HealHealth(healthHealed);
        StartCoroutine(healingCooldown());
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

    public IEnumerator healingCooldown()
    {
        healCooldownComplete = false; //Set cooldown complete to false
        playerBuffsDebuffs.FullStomachDebuff = true; //Set full stomach debuff to true
        yield return new WaitForSeconds(60f); //Do 60 second cooldown
        healCooldownComplete = true; //Set cooldown complete to true
        playerBuffsDebuffs.FullStomachDebuff = false; //Turn full stomach debuff off
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
    public void UseBow(float itemDamage, GameObject projectilePrefab, float projectileSpeed, float projectileLifetime, float chargeMultiplier)
    {
        if (Time.time >= nextAttackTime)
        {
            if (IsHost)
            {
                LaunchProjectile(itemDamage * chargeMultiplier, projectilePrefab, projectileRotationObject, projectileSpeed * chargeMultiplier, projectileLifetime); //Shoot the projectile
                nextAttackTime = Time.time + 1f / attackRate; //Do Cooldown
            }

            else if (IsClient)
            {
                //ProjectileTest(itemDamage * chargeMultiplier, projectilePrefab, projectileRotationObject, projectileSpeed * chargeMultiplier, projectileLifetime);
                Vector3 fireLocation = firePoint.position;
                Vector3 projectileDirection = firePoint.up;
                Quaternion tempRotation = projectileRotationObject.rotation;
                LaunchArrowProjectileServerRpc(itemDamage, projectileSpeed, projectileLifetime, projectileDirection, tempRotation, fireLocation);
                nextAttackTime = Time.time + 1f / attackRate; //Do Cooldown
            }
        }
    }

    private void LaunchProjectile(float itemDamage, GameObject projectilePrefab, Transform rotationObject, float projectileSpeed, float projectileLifetime)
    {
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, rotationObject.rotation);
        Projectile projectileScript = bullet.GetComponent<Projectile>();
        projectileScript.Projectiledamage = itemDamage;
        projectileScript.Projectilelifetime = projectileLifetime;
        projectileScript.StartDestructionCoroutine();

        bullet.GetComponent<NetworkObject>().Spawn();
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.up * projectileSpeed;
    }

    private void ProjectileTest(float itemDamage, GameObject projectilePrefab, Transform rotationObject, float projectileSpeed, float projectileLifetime)
    {
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, rotationObject.rotation);
        Projectile projectileScript = bullet.GetComponent<Projectile>();
        projectileScript.Projectiledamage = itemDamage;
        projectileScript.Projectilelifetime = projectileLifetime;
        projectileScript.StartDestructionCoroutine();
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.up * projectileSpeed;
    }

    [ServerRpc(RequireOwnership = false)]
    public void LaunchArrowProjectileServerRpc(float itemDamage, float projectileSpeed, float projectileLifetime, Vector3 fireDirection, Quaternion quaternion, Vector3 spawnLocation)
    {
        GameObject bullet = Instantiate(arrowPrefab, spawnLocation, quaternion);
        bullet.GetComponent<NetworkObject>().Spawn();
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = fireDirection * projectileSpeed;
        Projectile projectile = bullet.GetComponent<Projectile>();
        projectile.Projectiledamage = itemDamage;
        projectile.Projectilelifetime = projectileLifetime;
        projectile.StartDestructionCoroutine();
    }

    [ServerRpc]
    public void SwordSlashProjectileServerRpc(float itemDamage, float projectileSpeed, float projectileLifetime, Vector3 fireDirection, Quaternion quaternion, Vector3 spawnLocation)
    {
        GameObject bullet = Instantiate(slashPrefab, spawnLocation, quaternion);
        bullet.GetComponent<NetworkObject>().Spawn();
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = fireDirection * projectileSpeed;
        Projectile projectile = bullet.GetComponent<Projectile>();
        projectile.Projectiledamage = itemDamage;
        projectile.Projectilelifetime = projectileLifetime;
        projectile.StartDestructionCoroutine();
    }

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, new Vector3(1.5f, 1.5f, 0));
    }
    public void PlaceBlock(GameObject BlockPrefab, int ItemID) //Takes in a ItemTile based on what your holding and places it
    {
        if (IsHost) //If host, do this and then spawn it on network
        {
            Vector3Int mousePos = GetMousePosition(); //Gets mouse position
            var go = Instantiate(BlockPrefab, mousePos, Quaternion.identity); //Instantiate Wall
            //string chunkName = chunkController.currentChunkName; //Name of Chunk
            int chunkInt = 5;            //Int of Chunk
            GameObject chunkIn = chunkController.worldChunksHolder[chunkInt]; //Object of Chunk player is In
            go.transform.parent = chunkIn.transform; //Tile is parented to chunk.

            if (chunkIn.activeInHierarchy == false) //If the chunk is NOT active
            {
                go.SetActive(false); //Make sure the tile is also NOT ACTIVE!
            }

            PlaceBlockOnClientRpc(chunkInt, ItemID, mousePos);
        }
        else if (IsClient)
        {
            Vector3Int mousePos = GetMousePosition(); //Get position where tile will go
            //string chunkName = chunkController.currentChunkName; //Name of Chunk
            int chunkInt = 5;            //Get int of chunk your in.
            PlaceBlockServerRpc(chunkInt, ItemID, mousePos);
        }
    }

    [ServerRpc(RequireOwnership = false)] //Fired by Client, Execute by Server
    public void PlaceBlockServerRpc(int chunkNum, int tileId, Vector3Int tilePosition)
    {
        var blockObject = blockDatabase.TileReturner(tileId); //Get tile need to place

        var go = Instantiate(blockObject, tilePosition, Quaternion.identity); //Instantiate it on host
        GameObject chunkIn = chunkController.worldChunksHolder[chunkNum]; //Find the chunk with the correct id
        go.transform.parent = chunkIn.transform; //Make tile parent the correct hunk!

        if (chunkIn.activeInHierarchy == false) //If the chunk is NOT active
        {
            go.SetActive(false); //Make sure the tile is also NOT ACTIVE!
        }

        PlaceBlockOnClientRpc(chunkNum, tileId, tilePosition); //Place it on all clients!
    }

    [ClientRpc] //Fire by server, execute by client
    public void PlaceBlockOnClientRpc(int chunkNum, int tileID, Vector3Int tilePosition)
    {
        if (IsHost) return; //If your the host DONT DO IT!
        var blockObject = blockDatabase.TileReturner(tileID);

        var go = Instantiate(blockObject, tilePosition, Quaternion.identity);
        GameObject chunkIn = chunkController.worldChunksHolder[chunkNum];
        go.transform.parent = chunkIn.transform;

        if (chunkIn.activeInHierarchy == false) //If the chunk is NOT active
        {
            go.SetActive(false); //Make sure the tile is also NOT ACTIVE!
        }
    }


    

    //public GameObject TileReturner(int tileID)
    //{
    //    if (tileID == blockDatabase.woodWallData.ID)
    //    {
    //        return blockDatabase.woodWallData.BlockPrefab;
    //    }
    //    else if (tileID == blockDatabase.torchWallData.ID)
    //    {
    //        return blockDatabase.torchWallData.BlockPrefab;
    //    }
    //    else if (tileID == blockDatabase.craftingTable.ID)
    //    {
    //        return blockDatabase.craftingTable.BlockPrefab;
    //    }
    //    else if (tileID == blockDatabase.furnace.ID)
    //    {
    //        return blockDatabase.furnace.BlockPrefab;
    //    }
    //    else if (tileID == blockDatabase.tinAnvil.ID)
    //    {
    //        return blockDatabase.tinAnvil.BlockPrefab;
    //    }
    //    else return null;
    //}
}
