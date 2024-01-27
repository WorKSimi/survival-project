using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.Netcode;

public class Chicken : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private GameObject egg;

    //public GameObject[] players; //Array for all players
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

    private float standardSpeed;
    private bool isChickenStopped;

    private void Awake()
    {
        startingPosition = this.transform.position;
        standardSpeed = speed;
        isChickenStopped = false;

        InvokeRepeating("StopStartChicken", 5, 5); //Make chicken stop and start.
        InvokeRepeating("LayEgg", 5, 5*60); //Lay egg every 5 minutes (5 * 60)
    }

    private void StopStartChicken() //Regenerating Health Function (Can be turned on or off)
    {
        if (isChickenStopped == true)
        {
            speed = standardSpeed;
            isChickenStopped = false;
        }
        else if (isChickenStopped == false)
        {
            speed = 0f;
            isChickenStopped = true;
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
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
        animator.SetFloat("Horizontal", rb.velocity.x);
        animator.SetFloat("Vertical", rb.velocity.y);
        animator.SetFloat("Speed", rb.velocity.sqrMagnitude);

        target = roamWaypoint.transform;  //This sets the target to be the roaming waypoint.
        target.position = roamPosition; //Sets the targets position to equal the roaming position.                      
        MoveToWaypoint(); //Moves chicken to the waypoint.
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

    private void LayEgg()
    {
        var go = Instantiate(egg, this.transform.position, Quaternion.identity);
        go.GetComponent<NetworkObject>().Spawn();
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
        rb.velocity = direction * speed;

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
}
