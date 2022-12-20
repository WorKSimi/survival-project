using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SnailEnemy : MonoBehaviour
{
    private enum State
    {
        Roaming,
        ChaseTarget,
        AttackingTarget,
    }

    private State state;
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 200f;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private Transform enemyGFX;

    private Vector3 targetChargePosition;

    private bool isAttacking;
    private GameObject player;
    private GameObject roamWaypoint;
    private Vector3 startingPosition;
    private Vector3 roamPosition;

    private float currentTime = 0;
    private float startingTime = 10f;

    Path path;
    int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private bool playerFound = false;
    Seeker seeker;
    Rigidbody2D rb;

    private void Awake()
    {
        roamWaypoint = transform.GetChild(1).gameObject; //Sets roam waypoint variable to the second child of the snail object
        isAttacking = false;
        state = State.Roaming;
    }

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);

        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();

        currentTime = startingTime;
    }

    //private void SetPlayerVariable()
    //{
    //    if (playerFound == true) return;

    //    else if (playerFound == false)
    //    {
    //        player = GameObject.FindWithTag("Player");
    //        playerFound = true;
    //    }
    //}
    private void Update()
    {
        player = GameObject.FindWithTag("Player");

        switch (state)
        {
            default:
            case State.Roaming:

            target = roamWaypoint.transform;  //This sets the target to be the roaming waypoint.
            target.position = roamPosition; //Sets the targets position to equal the roaming position.
            MoveToWaypoint(); //Moves snail to the waypoint.
            FindTarget(); //Tries to find a player near it

            break;

            case State.ChaseTarget:

                target = player.transform; //Sets the target to be the player
                MoveToWaypoint(); //Moves the snail to the target (player)

                float attackRange = 5f;
                if (Vector3.Distance(transform.position, player.transform.position) < attackRange) //See if player is close enough for attack
                {
                    Debug.Log("Enemy in attack range (disabled for testing");
                    //state = State.AttackingTarget;                  
                }

                break;

            case State.AttackingTarget:

                StartCoroutine(ChargeAttack());
                state = State.ChaseTarget;
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
        //Stop his movement
        rb.velocity = Vector3.zero;
        Debug.Log("Waiting");
        yield return new WaitForSeconds(5); //Wait 2 seconds       

        targetChargePosition = target.position;
        Vector2 direction = (targetChargePosition - transform.position); 

        rb.AddForce(direction * 40); //Charge at the player
        Debug.Log("Charging");
        yield return new WaitForSeconds(5); //Charge for 3 second

        Debug.Log("Disabling Movement");
        rb.velocity = Vector3.zero; //Disable movement 
        Debug.Log("Brief Pause");
        yield return new WaitForSeconds(5); //Charge for 3 second        
    }

    private void FindTarget()
    {
        float targetRange = 10f;
        if (Vector3.Distance(transform.position, player.transform.position) < targetRange) //Search for the player within target range
        {
            state = State.ChaseTarget;
        }
    }
}

