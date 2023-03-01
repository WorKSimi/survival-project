using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerNetwork : NetworkBehaviour
{
    private float moveSpeed = 8f; //Float for the move speed of this player
    private float waterMoveSpeed = 4f; //Speed for when you want players to go slow
    private float currentMoveSpeed; //Variable to store what the players CURRENT move speed is
    private Rigidbody2D rb; //Rigidbody2D of this player

    [SerializeField]private Animator animator; //Animator of this player
    [SerializeField]private Camera playerCam; //This players Camera

    private SpriteRenderer spritePlayerRenderer; //Sprite render for the player sprite.
    [SerializeField] private GameObject thisPlayer;

    private Tilemap waterTilemap; //Water Tilemap
    private Vector3Int playerGridPos; //Position of player on grid

    private float rollSpeed;

    private Vector3 moveDir;
    private Vector3 rollDir;
    private Vector3 lastMoveDir;

    private bool isInWater; //Bool to save if the player is in water or not. By default its false

    private Vector3 mousePos; 
    private Vector3 playerPos;
    private Vector3Int playerPosInt; //Player position in int form
    private Vector3 screenPos;
    private Vector3 distance;

    private float dodgeRate = 0.5f; //How many times you can dodge per second
    float nextDodgeTime = 0f;

    public State state; //Variable for state of player (Normal by default)
    public enum State //Store the states of the player
    {
        Normal, //The normal state of player
        Rolling, //When player is Dodge-Rolling
    }

    private void Awake()
    {
        state = State.Normal;
        currentMoveSpeed = moveSpeed;
        spritePlayerRenderer = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();

        var tilemapObject = GameObject.FindWithTag("WaterTilemap");
        waterTilemap = tilemapObject.GetComponent<Tilemap>();    
    }

    private void Update()
    {
        if (!IsOwner) return;
        playerPos = thisPlayer.transform.position;
        playerGridPos = waterTilemap.WorldToCell(playerPos);       

        mousePos = Input.mousePosition;
        screenPos = playerCam.WorldToScreenPoint(playerPos);
        distance = (mousePos - screenPos);

        PlayerDirection();
        switch (state)
        {
            case State.Normal:
                MovementInput();
                animator.SetFloat("Speed", rb.velocity.sqrMagnitude);
                break;

            case State.Rolling:
                float rollSpeedDropMultiplier = 2f;
                rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;

                float rollSpeedMinimum = 8f;
                if (rollSpeed < rollSpeedMinimum)
                {
                    state = State.Normal;
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Normal:
                rb.velocity = moveDir * currentMoveSpeed;
                break;

            case State.Rolling:
                rb.velocity = rollDir * rollSpeed;
                break;
        }
    }

    private void PlayerDirection()
    {
        if (distance.x > 0 && distance.y < 10 && distance.y > -10) //face right
        {
            animator.SetBool("FacingRight", true);
            animator.SetBool("FacingLeft", false);
            animator.SetBool("FacingUp", false);
            animator.SetBool("FacingDown", false);

            spritePlayerRenderer.flipX = false;
        }

        if (distance.x < 0 && distance.y < 10 && distance.y > -10) //face left
        {
            animator.SetBool("FacingRight", false);
            animator.SetBool("FacingLeft", true);
            animator.SetBool("FacingUp", false);
            animator.SetBool("FacingDown", false);

            spritePlayerRenderer.flipX = true;
        }

        if (distance.y > 0 && distance.x > -10 && distance.x < 10) //Face up
        {
            animator.SetBool("FacingRight", false);
            animator.SetBool("FacingLeft", false);
            animator.SetBool("FacingUp", true);
            animator.SetBool("FacingDown", false);
        }

        if (distance.y < 0 && distance.x > -10 && distance.x < 10) //Face down
        {
            animator.SetBool("FacingRight", false);
            animator.SetBool("FacingLeft", false);
            animator.SetBool("FacingUp", false);
            animator.SetBool("FacingDown", true);
        }
    }

    private void IsPlayerInWater()
    {
        if (waterTilemap.GetTile(playerGridPos)) //If a tile on water tilemap is gotten at player position
        {
            currentMoveSpeed = waterMoveSpeed; //Set current move speed to water speed
        }
        else currentMoveSpeed = moveSpeed;
    }
    
    private void MovementInput()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            moveY = +1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveX = +1f;
        }

        moveDir = new Vector3(moveX, moveY).normalized;

        if (moveX != 0 || moveY != 0) //Player is not idle
        {
            lastMoveDir = moveDir; 
            IsPlayerInWater(); //Check if player is in water. Don't have to do this if not moving!
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Time.time >= nextDodgeTime)
            {
                rollDir = lastMoveDir; //Set direction of roll to direction of movement
                rollSpeed = 20f; //Speed of Dodge Roll
                state = State.Rolling; //Set player state to rolling

                nextDodgeTime = Time.time + 1f / dodgeRate; //Dodge cooldown
            }
        }
    }
}
