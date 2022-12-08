using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField]private Transform spawnedObjectPrefab;
    [SerializeField]private float moveSpeed = 5f;
    [SerializeField]private Rigidbody2D rb;
    [SerializeField]private Animator animator;
    [SerializeField]private GameObject cam;
    Vector2 movement;

    private void Update()
    {
        if (!IsOwner) return;

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
