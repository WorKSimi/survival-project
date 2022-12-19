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
    }
    private State state;
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 200f;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private Transform enemyGFX;

    private GameObject player;
    private GameObject roamWaypoint;
    private Vector3 startingPosition;
    private Vector3 roamPosition;

    Path path;
    int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private bool playerFound = false;
    Seeker seeker;
    Rigidbody2D rb;

    private void Awake()
    {
        roamWaypoint = transform.GetChild(1).gameObject; //Sets roam waypoint variable to the second child of the snail object       
        state = State.Roaming;
    }

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);

        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();
    }

    private void SetPlayerVariable()
    {
        if (playerFound == true) return;

        else if (playerFound == false)
        {
            player = GameObject.FindWithTag("Player");
            playerFound = true;
        }
    }

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

    private void FindTarget()
    {
        float targetRange = 10f;
        if (Vector3.Distance(transform.position, player.transform.position) < targetRange) //Search for the player within target range
        {
            state = State.ChaseTarget;
        }
    }
}

