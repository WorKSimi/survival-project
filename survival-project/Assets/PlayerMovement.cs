using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Variable to control the player movespeed
    public float moveSpeed = 5f;

    //To use player's rigidbody in the code
    public Rigidbody2D rb;

    //Reference to animator to handle animation
    public Animator animator;

    //This stores an x value and a y value (horizontal and vertical)
    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    //Called 50 times a second, no matter the framerate
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
