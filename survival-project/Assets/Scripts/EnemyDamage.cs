using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private GameObject player;
    private PlayerHealth playerHealth;
    private bool foundPlayer;
    [SerializeField] private int damage = 2;


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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var playerHealthScript = collision.GetComponent<PlayerHealth>();
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
