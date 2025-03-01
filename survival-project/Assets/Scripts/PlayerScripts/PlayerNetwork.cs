using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 7f; //Float for the move speed of this player
    private float waterMoveSpeed = 4f; //Speed for when you want players to go slow
    private float deadSpeed = 0f;
    private float currentMoveSpeed; //Variable to store what the players CURRENT move speed is
    private Rigidbody2D rb; //Rigidbody2D of this player
    private PlayerHealth playerHealth;

    [SerializeField]private Animator animator; //Animator of this player
    [SerializeField] private Animator helmetAnimator;
    [SerializeField] private Animator chestplateAnimator;
    [SerializeField]private Camera playerCam; //This players Camera

    private SpriteRenderer spritePlayerRenderer; //Sprite render for the player sprite.
    [SerializeField] private SpriteRenderer helmetSpriteRenderer;
    [SerializeField] private SpriteRenderer chestplateSpriteRenderer;
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
        Dead, //When the player is dead
        Loading, //When the player is Loading
    }

    private void Start()
    {
        spritePlayerRenderer = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Awake()
    {
        state = State.Normal;
        currentMoveSpeed = moveSpeed;

        //var tilemapObject = GameObject.FindWithTag("WaterTilemap");
        //waterTilemap = tilemapObject.GetComponent<Tilemap>(); 
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

            case State.Dead:
                rb.velocity = moveDir * deadSpeed; //Lock player movement when dead
                break;

            case State.Loading:
                //rb.velocity = moveDir * deadSpeed; //lock player movement when loading.
                rb.velocity = moveDir * currentMoveSpeed;
                break;
        }
    }

    private void Update()
    {
        PlayerDirection();
        MovementInput();
    }

    private void PlayerDirection()
    {
        playerPos = this.transform.position;
        mousePos = playerCam.ScreenToWorldPoint(Input.mousePosition);
        distance = mousePos - playerPos;
        animator.SetFloat("Speed", rb.velocity.sqrMagnitude);

        if (distance.x > 0 && distance.y < 0.1 && distance.y > -0.1) //face right
        {
            animator.SetBool("FacingRight", true);
            animator.SetBool("FacingLeft", false);
            animator.SetBool("FacingUp", false);
            animator.SetBool("FacingDown", false);

            helmetAnimator.SetBool("FacingSide", true);
            helmetAnimator.SetBool("FacingDown", false);
            helmetAnimator.SetBool("FacingUp", false);

            chestplateAnimator.SetBool("FacingSide", true);
            chestplateAnimator.SetBool("FacingDown", false);
            chestplateAnimator.SetBool("FacingUp", false);

            spritePlayerRenderer.flipX = false;
            helmetSpriteRenderer.flipX = false;
            chestplateSpriteRenderer.flipX = false;
        }

        if (distance.x < 0 && distance.y < 0.1 && distance.y > -0.1) //face left
        {
            animator.SetBool("FacingRight", false);
            animator.SetBool("FacingLeft", true);
            animator.SetBool("FacingUp", false);
            animator.SetBool("FacingDown", false);

            helmetAnimator.SetBool("FacingSide", true);
            helmetAnimator.SetBool("FacingDown", false);
            helmetAnimator.SetBool("FacingUp", false);

            chestplateAnimator.SetBool("FacingSide", true);
            chestplateAnimator.SetBool("FacingDown", false);
            chestplateAnimator.SetBool("FacingUp", false);

            spritePlayerRenderer.flipX = true;
            helmetSpriteRenderer.flipX = true;
            chestplateSpriteRenderer.flipX = true;
        }

        if (distance.y > 0 && distance.x > -0.1 && distance.x < 0.1) //Face up
        {
            animator.SetBool("FacingRight", false);
            animator.SetBool("FacingLeft", false);
            animator.SetBool("FacingUp", true);
            animator.SetBool("FacingDown", false);

            helmetAnimator.SetBool("FacingSide", false);
            helmetAnimator.SetBool("FacingDown", false);
            helmetAnimator.SetBool("FacingUp", true);

            chestplateAnimator.SetBool("FacingSide", false);
            chestplateAnimator.SetBool("FacingDown", false);
            chestplateAnimator.SetBool("FacingUp", true);
        }

        if (distance.y < 0 && distance.x > -0.1 && distance.x < 0.1) //Face down
        {
            animator.SetBool("FacingRight", false);
            animator.SetBool("FacingLeft", false);
            animator.SetBool("FacingUp", false);
            animator.SetBool("FacingDown", true);

            helmetAnimator.SetBool("FacingSide", false);
            helmetAnimator.SetBool("FacingDown", true);
            helmetAnimator.SetBool("FacingUp", false);

            chestplateAnimator.SetBool("FacingSide", false);
            chestplateAnimator.SetBool("FacingDown", true);
            chestplateAnimator.SetBool("FacingUp", false);
        }
    }

    //private void IsPlayerInWater()
    //{
    //    if (waterTilemap.GetTile(playerGridPos)) //If a tile on water tilemap is gotten at player position
    //    {
    //        currentMoveSpeed = waterMoveSpeed; //Set current move speed to water speed
    //    }
    //    else currentMoveSpeed = moveSpeed;
    //}
    
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
            //IsPlayerInWater(); //Check if player is in water. Don't have to do this if not moving!
        }

        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //{
        //    if (Time.time >= nextDodgeTime)
        //    {
        //        rollDir = lastMoveDir; //Set direction of roll to direction of movement
        //        rollSpeed = 20f; //Speed of Dodge Roll
        //        state = State.Rolling; //Set player state to rolling

        //        nextDodgeTime = Time.time + 1f / dodgeRate; //Dodge cooldown
        //    }
        //}
    }
}
