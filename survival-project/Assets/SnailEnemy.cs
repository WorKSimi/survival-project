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

    private GameObject player;
    private Vector3 startingPosition;
    private Vector3 roamPosition;

    Path path;
    int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);

        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();
    }

    private void Update()
    {
        target.position = roamPosition;
    }

    void FixedUpdate()
    {
        MoveToWaypoint();
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
        return startingPosition + GetRandomDir() * Random.Range(10f, 70f);
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

        if (rb.velocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}

