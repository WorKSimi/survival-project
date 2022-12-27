using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SnailEnemy : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 200f;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private Transform enemyGFX;

    public SnailState state;

    private Vector3 targetChargePosition;
    public GameObject playerWhoSpawned;

    private bool isCharging = false;
    private bool isRoaming = false;

    public MobSpawning mobSpawning;
    public GameObject player;
    private GameObject roamWaypoint;
    public Vector3 startingPosition;
    private Vector3 roamPosition;

    private float currentTime = 0;
    private float startingTime = 10f;

    Path path;
    int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    
    Seeker seeker;
    Rigidbody2D rb;

    private void Awake()
    { 
        roamWaypoint = transform.GetChild(1).gameObject; //Sets roam waypoint variable to the second child of the snail object
        state = SnailState.Roaming;
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
        player = GameObject.FindWithTag("Player");

        float maxDespawnDistance = 50f;
        if (Vector3.Distance(this.transform.position, playerWhoSpawned.transform.position) > maxDespawnDistance)
        {
            this.mobSpawning.currentSpawns--;
            Destroy(this.gameObject); //Destroy the snail,          
        }

        switch (state)
        {
            default:
            case SnailState.Roaming:
            
            target = roamWaypoint.transform;  //This sets the target to be the roaming waypoint.
            target.position = roamPosition; //Sets the targets position to equal the roaming position.                      
            MoveToWaypoint(); //Moves snail to the waypoint.
            FindTarget(); //Tries to find a player near it

            break;

            case SnailState.ChaseTarget:
                

                isCharging = false;
                target = player.transform; //Sets the target to be the player
                MoveToWaypoint(); //Moves the snail to the target (player)
                
                float attackRange = 5f;
                if (Vector3.Distance(transform.position, player.transform.position) < attackRange) //See if player is close enough for attack
                {                                 
                    state = SnailState.AttackingTarget;                  
                }
                TargetToFar(); //Sees if player is too far or not
                break;

            case SnailState.AttackingTarget:
         

                if (isCharging == false)
                {
                    isCharging = true;
                    StartCoroutine(ChargeAttack());               
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
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb.velocity.x <= 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private IEnumerator ChargeAttack()
    {     
        rb.velocity = Vector3.zero; //Stop his movement       
        yield return new WaitForSeconds(1f); //Wait 1 seconds       

        targetChargePosition = target.position;
        Vector2 direction = (targetChargePosition - transform.position); //Get direction of player
        rb.AddForce(direction * 4, ForceMode2D.Impulse); //Charge at the player

        yield return new WaitForSeconds(1); //Charge for 1 second

        rb.velocity = Vector3.zero; //Disable movement 

        yield return new WaitForSeconds(.5f); //Pause for half a second

        state = SnailState.ChaseTarget;
    }
    
    private void FindTarget()
    {
        float targetRange = 10f;
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (Vector3.Distance(transform.position, player.transform.position) < targetRange) //Search for the player within target range
        {
            state = SnailState.ChaseTarget;
        }
    }

    private void TargetToFar()
    {
        float aggroRange = 20f;
        if (Vector3.Distance(transform.position, player.transform.position) > aggroRange) //If player gets too far
        {
            state = SnailState.Roaming; //Set state back to roaming
        }
    }
}

