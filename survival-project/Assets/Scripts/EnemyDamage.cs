using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private GameObject player;
    private PlayerHealth playerHealth;
    private bool foundPlayer;
    public int damage = 2;


    // Start is called before the first frame update
    void Start()
    {
        //foundPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        //LocatePlayer();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerHealthScript = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealthScript.invincibile == true) return; //If player is invincible, return and do not take damage
            playerHealthScript.TakeDamage(damage);
        }
    }

    private void LocatePlayer()
    {
        if (foundPlayer == false)
        {
            player = GameObject.FindWithTag("Player");
            playerHealth = player.GetComponent<PlayerHealth>();
            foundPlayer = true;
        }
        else if (foundPlayer == true) return;
    }
}
