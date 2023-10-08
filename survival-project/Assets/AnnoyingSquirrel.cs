using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.Netcode;

public class AnnoyingSquirrel : MonoBehaviour
{

    [SerializeField] private Transform target;  
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private Transform enemyGFX;

    float nextAttackTime = 0f;
    

    private enum SquirrelState
    {
        Roam,
        Chase,
        Attack,
        Flee,
    }

    private SquirrelState state;

    private Vector3 targetChargePosition;
    public GameObject playerWhoSpawned;

    private bool isCharging = false;
    private bool isRoaming = false;

    //public MobSpawning mobSpawning;
    public GameObject playerToChase;
    public GameObject[] players; //Array for all players
    [SerializeField] private GameObject roamWaypoint;
    public Vector3 startingPosition;
    private Vector3 roamPosition;

    private float currentTime = 0;
    private float startingTime = 10f;

    Path path;
    int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    [Header("Squirrel Stats")]
    [SerializeField] private float speed = 200f;
    [SerializeField] private float chaseRange; //Range for Squirrel to Chase Player
    [SerializeField] private float attackRange; //Range for Squirrel to Attack player
    [SerializeField] private float fleeRange; //Range for Squirrel to Run from Player
    [SerializeField] private float escapeRange; //Range for Squirrel to decide it has escaped the player.
    [SerializeField] private float attackRate = 1f; //How many times you can attack per second

    [SerializeField] private GameObject projectile; //Projectile for squirrel
    [SerializeField] private int enemyDamage = 5; //Damage of enemy
    [SerializeField] private float projectileSpeed = 3f; //Speed of projectile
    [SerializeField] private float projectileDuration = 5f; //projectile lasts 5 seconds

    private void Awake()
    {
        startingPosition = this.transform.position;
        //roamWaypoint = transform.GetChild(1).gameObject; //Sets roam waypoint variable to the second child of the snail object
        state = SquirrelState.Roam;
    }

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);

        roamPosition = GetRoamingPosition();

        currentTime = startingTime;
    }

    private void Update()
    {
        //animator.SetFloat("Horizontal", rb.velocity.x);
        //animator.SetFloat("Vertical", rb.velocity.y);
        //animator.SetFloat("Speed", rb.velocity.sqrMagnitude);
        
        switch (state)
        {
            default:
            case SquirrelState.Roam:

                target = roamWaypoint.transform;  //This sets the target to be the roaming waypoint.
                target.position = roamPosition; //Sets the targets position to equal the roaming position.                      
                MoveToWaypoint(); //Moves Squirrel to the waypoint.
                FindTarget(); //Tries to find a player near it

                break;

            case SquirrelState.Chase:
               
                target = playerToChase.transform; //Sets the target to be the player               
                MoveToWaypoint(); //Moves the squirrel to the target (player)

                if (Vector3.Distance(transform.position, playerToChase.transform.position) < attackRange) //See if player is close enough for attack
                {
                    state = SquirrelState.Attack;
                }
                break;

            case SquirrelState.Attack:

                //For this State, have squirrel launch projectiles at player.
                rb.velocity = Vector3.zero; //stop movement
                
                if (Time.time >= nextAttackTime) //If not on cooldown
                {
                    SquirrelProjectileAttack();
                    nextAttackTime = Time.time + 1f / attackRate; //Do cooldown stuff
                }       
                //If player gets too close, set state to flee
                if (Vector3.Distance(transform.position, playerToChase.transform.position) < fleeRange) //See if player is in flee range 
                {
                    state = SquirrelState.Flee; //If they are, set to flee
                }

                if (Vector3.Distance(transform.position, playerToChase.transform.position) > attackRange) //See if player is outside attack range
                {
                    state = SquirrelState.Chase; //If they are, set to flee
                }
                break;

            case SquirrelState.Flee:

                //For this state, have snail move in opposite direction of player.
                //Direction = destination - source
                
                Vector2 direction = ((Vector2)playerToChase.transform.position - rb.position).normalized; //Direction to player
                Vector2 direction2 = -direction;
                Vector2 force = direction2 * (speed*2) * Time.deltaTime;

                rb.AddForce(force);

                //If player gets far enough, set state back to chase.
                if (Vector3.Distance(transform.position, playerToChase.transform.position) > escapeRange) //See if player is far enough
                {
                    state = SquirrelState.Chase; //If they are, set to chase
                }
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
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

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
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players) //For each player in players
        {
            float distance = Vector3.Distance(transform.position, player.transform.position); //Distance is squirrel to current player

            if (Vector3.Distance(transform.position, player.transform.position) < chaseRange) //Search for the player within target range
            {
                playerToChase = player; //Player to chase will be if the player is closer
                state = SquirrelState.Chase;
            }
        }
    }
    private void SquirrelProjectileAttack()
    {
        //Do projectile shit
        var fireDirection = (target.transform.position - this.transform.position).normalized;
        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        GameObject bullet = Instantiate(projectile, transform.position, rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
        EnemyProjectile projectileScript = bullet.GetComponent<EnemyProjectile>();
        projectileScript.enemyProjectileDamage = enemyDamage;
        projectileScript.StartDestructionCoroutine(projectileDuration);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = fireDirection * projectileSpeed;

        //yield return new WaitForSeconds(1f); //Wait 1 second
        nextAttackTime = 0;
    }
}
