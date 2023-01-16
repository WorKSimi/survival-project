using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private GameObject droppedItem;
    [SerializeField] private double maxHealth;
    [SerializeField] private SimpleFlash flashEffect;
    //[SerializeField] private AudioSource hitSound;

    public MobSpawning mobSpawning;
    private double currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(double damage)
    {
        currentHealth -= damage;
        flashEffect.Flash(); //Make enemy flash when hit
        //hitSound.Play(); //Play hit sound

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (droppedItem != null) //If the enemy has an item to drop
        {
            for (int i = 0; i < 5; i++) //Drop 5 of this enemies item drop
            {
                Instantiate(droppedItem, transform.position, Quaternion.identity); //Instantiate that item where snail is              
                Destroy(this.gameObject); //Remove this enemy from the game world
            }
        }
        else
        {
            this.mobSpawning.currentSpawns--;
            Destroy(this.gameObject);
        }
    }
}
