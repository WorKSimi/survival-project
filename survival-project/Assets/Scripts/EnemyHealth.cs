using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private GameObject droppedItem;
    [SerializeField] private double maxHealth;

    private double currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(double damage)
    {
        Debug.Log("ENEMY HIT!");
        currentHealth -= damage;
        //Make enemy flash when hit
        //Play hit sound

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Die!");
        //Enemy die animation
        if (droppedItem != null) //If the enemy has an item to drop
        {
            for (int i = 0; i < 5; i++) //Drop 5 of this enemies item drop
            {
                Instantiate(droppedItem, transform.position, Quaternion.identity); //Instantiate that item where snail is
                Destroy(this.gameObject); //Remove this enemy from the game world
            }
        }
        else Destroy(this.gameObject); 
    }
}
