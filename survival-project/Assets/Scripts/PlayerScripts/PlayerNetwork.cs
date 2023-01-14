using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField]private Transform spawnedObjectPrefab; 
    [SerializeField]private float moveSpeed = 5f; //Float for the move speed of this player
    [SerializeField]private Rigidbody2D rb; //Rigidbody2D of this player
    [SerializeField]private Animator animator; //Animator of this player
    [SerializeField]private GameObject cam; //This players Camera

    [SerializeField] private SpriteRenderer spritePlayerRenderer; //Sprite render for the player sprite.
    [SerializeField] private GameObject thisPlayer;

    private Vector3 mousePos; 
    private Vector3 playerPos;
    private Vector3 screenPos;
    private Vector3 distance;

    Vector2 movement;

    private void PlayerDirection()
    { //40 -40
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

        if(distance.y > 0 && distance.x > -10 && distance.x < 10) //Face up
        {
            animator.SetBool("FacingRight", false);
            animator.SetBool("FacingLeft", false);
            animator.SetBool("FacingUp", true);
            animator.SetBool("FacingDown", false);
        }

        if(distance.y < 0 && distance.x > -10 && distance.x < 10) //Face down
        {
            animator.SetBool("FacingRight", false);
            animator.SetBool("FacingLeft", false);
            animator.SetBool("FacingUp", false);
            animator.SetBool("FacingDown", true);
        }
    }

   

    private void Update()
    {
        if (!IsOwner) return;
        
        playerPos = thisPlayer.transform.position;
        mousePos = Input.mousePosition;
        screenPos = Camera.main.WorldToScreenPoint(playerPos);
        distance = (mousePos - screenPos);
        PlayerDirection();

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Speed", movement.sqrMagnitude);
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
