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

    private bool facingRight;
    private bool facingLeft;

    Vector2 movement;

    private void PlayerDirection()
    {
        if (mousePos.x > playerPos.x)
        {
            facingRight = true;
            facingLeft = false;
            spritePlayerRenderer.flipX = false;
        }

        else if (mousePos.x < playerPos.x)
        {
            facingRight = false;
            facingLeft = true;
            spritePlayerRenderer.flipX = true;
        }
    }

   

    private void Update()
    {
        if (!IsOwner) return;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerPos = thisPlayer.transform.position;

        PlayerDirection();

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        if (IsOwner) return;
        cam.SetActive(false);
    }


}
