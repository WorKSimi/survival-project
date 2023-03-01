using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyHealth : NetworkBehaviour
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
        if (IsHost)
        {
            if (droppedItem != null) //If the enemy has an item to drop
            {
                for (int i = 0; i < 5; i++) //Drop 5 of this enemies item drop
                {
                    Instantiate(droppedItem, transform.position, Quaternion.identity); //Instantiate that item where snail is
                    this.mobSpawning.currentSpawns--; //Remove 1 count from current spawns
                    Destroy(this.gameObject); //Remove this enemy from the game world
                }
            }
            else if (droppedItem == null) //If enemy has no item
            {
                this.mobSpawning.currentSpawns--; //Remove 1 count from current spawns
                Destroy(this.gameObject); //Remove enemy
            }
        }
        else if (IsClient)
        {
            if (droppedItem != null) //If the enemy has an item to drop
            {
                for (int i = 0; i < 5; i++) //Drop 5 of this enemies item drop
                {
                    SpawnItemOnServerRpc();               
                }
                this.mobSpawning.currentSpawns--; //Remove 1 count from current spawns
                DespawnEnemyServerRpc();
            }
            else if (droppedItem == null) //If enemy has no item
            {
                this.mobSpawning.currentSpawns--; //Remove 1 count from current spawns
                DespawnEnemyServerRpc(); 
            }
        }
    }

    [ServerRpc(RequireOwnership = false)] //Fired by client executed on server
    public void DespawnEnemyServerRpc() //Remove enemy from server
    {
        this.gameObject.GetComponent<NetworkObject>().Despawn(); //Despawn object
        Destroy(this.gameObject); //Remove this enemy from the game world
    }

    [ServerRpc(RequireOwnership = false)] //Fired by client executed on server
    public void SpawnItemOnServerRpc()
    {
        var item = Instantiate(droppedItem, transform.position, Quaternion.identity); //Instantiate that item where snail is
        item.GetComponent<NetworkObject>().Spawn(); //Spawn item on server
    }
}
