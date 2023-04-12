using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.Netcode;

public class BeeEnemy : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private Transform enemyGFX;

    [SerializeField] private GameObject stingerProjectile;
    [SerializeField] private float projectileSpeed = 5f; //Speed of projectile
    [SerializeField] private float stingCooldown = 2f; //Fire every 2 seconds
    [SerializeField] private float projectileDuration = 5f; //projectile lasts 5 seconds
    public EnemyDamage enemyDamage;

    public enum BeeState
    {
        Passive,
        Aggro,
    }

    private BeeState state;
    private Vector3 targetChargePosition;
    public GameObject playerWhoSpawned;

    private bool isRoaming = false;

    public MobSpawning mobSpawning;
    public GameObject playerToChase;
    public GameObject[] players; //Array for all players
    private GameObject roamWaypoint;
    public Vector3 startingPosition;
    private Vector3 roamPosition;
    private Vector2 targetDirection;

    private Vector3 fireDirection;

    private float currentTime = 0;
    private float startingTime = 10f;

    Path path;
    int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    private void Awake()
    {
        roamWaypoint = transform.GetChild(1).gameObject; //Sets roam waypoint variable to the second child of the bee object
        state = BeeState.Passive; //Set bee state to passive on awake
    }

    private void Start()
    {
        //animator = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);

        roamPosition = GetRoamingPosition();

        currentTime = startingTime;
    }
    //instead of 1 player, fill an array for every object with the tag Player
    //Whenever checking distance, do it for each player in the array

    private void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        //animator.SetFloat("Horizontal", rb.velocity.x);
        //animator.SetFloat("Vertical", rb.velocity.y);
        //animator.SetFloat("Speed", rb.velocity.sqrMagnitude);

        float maxDespawnDistance = 50f;

        if (playerWhoSpawned != null)
        {
            if (Vector3.Distance(this.transform.position, playerWhoSpawned.transform.position) > maxDespawnDistance)
            {
                this.mobSpawning.currentSpawns--;
                this.gameObject.GetComponent<NetworkObject>().Despawn();
                Destroy(this.gameObject); //Destroy the snail,          
            }
        }
        else if (playerWhoSpawned == null)
        {
            Debug.Log("ERROR! PLAYER WHO SPAWNED IS NULL!");
        }

        switch (state)
        {
            default:
            case BeeState.Passive:

                target = roamWaypoint.transform;  //This sets the target to be the roaming waypoint.
                target.position = roamPosition; //Sets the targets position to equal the roaming position.                      
                MoveToWaypoint(); //Moves snail to the waypoint.
                FindTarget(); //Tries to find a player near it

                break;

            case BeeState.Aggro:

                target = playerToChase.transform; //Sets the target to be the player               
                MoveToWaypoint(); //Moves the bee to the target (player)
                targetDirection = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                StartCoroutine(StingAttack()); //Starts coroutine for the stinger attack

                //Bee will move towards player, and fire projectiles

                //float attackRange = 5f;
                //if (Vector3.Distance(transform.position, playerToChase.transform.position) < attackRange) //See if player is close enough for attack
                //{
                //    state = SnailState.AttackingTarget;
                //}

                TargetToFar(); //Sees if player is too far or not. If yes, set back to passive
                break;

            
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private IEnumerator StingAttack()
    {
        yield return new WaitForSeconds(stingCooldown); //Wait for sting cooldown        
        GameObject bullet = Instantiate(stingerProjectile, transform.position, Quaternion.identity);
        EnemyProjectile projectileScript = bullet.GetComponent<EnemyProjectile>();
        projectileScript.enemyProjectileDamage = enemyDamage.damage;
        projectileScript.enemyProjectileLifetime = projectileDuration;
        projectileScript.StartDestructionCoroutine();
        bullet.GetComponent<NetworkObject>().Spawn();
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        fireDirection = target.transform.position - this.transform.position;
        rb.velocity = fireDirection * projectileSpeed;
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPosition + GetRandomDir() * Random.Range(3f, 3f);
    }

    public static Vector3 GetRandomDir() //This function generates a random normalized direction
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    private void MoveToWaypoint()
    {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            roamPosition = GetRoamingPosition();
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        rb.velocity = direction * 1f;
        //Vector2 force = direction * speed * Time.deltaTime;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (rb.velocity.x >= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (rb.velocity.x <= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }


    private void FindTarget()
    {
        float targetRange = 10f;
        foreach (GameObject player in players) //For each player in players
        {
            float distance = Vector3.Distance(transform.position, player.transform.position); //Distance is bee to current player

            if (Vector3.Distance(transform.position, player.transform.position) < targetRange) //Search for the player within target range
            {
                playerToChase = player; //Player to chase will be if the player is closer
                state = BeeState.Aggro; //Set state to aggro
            }
        }
    }

    private void TargetToFar()
    {
        float aggroRange = 20f;

        foreach (GameObject player in players)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > aggroRange) //If player gets too far
            {
                state = BeeState.Passive; //Set state back to passive
            }
        }
    }
}

