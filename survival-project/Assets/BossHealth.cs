using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BossHealth : NetworkBehaviour
{
    [SerializeField] private GameObject droppedItem;
    [SerializeField] public float maxHealth;
    [SerializeField] private SimpleFlash flashEffect;

    [SerializeField] private SnailordBoss snailordBoss;
    public GameObject healthbarObject;
    [SerializeField] private HealthBar healthBar;
    //[SerializeField] private AudioSource hitSound;
    public float currentHealth;
    public bool canBossBeHurt;
    public bool isBerserk = false; //Flag to check if phase 3 began.

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        //healthbarObject.SetActive(false);
    }
    
    public void TakeDamage(float damage)
    {
        if (canBossBeHurt == false) return; //Back if boss cant be hurt
        currentHealth -= damage;
        flashEffect.Flash(); //Make enemy flash when hit
        healthBar.SetHealth(currentHealth); //Set health bar here
        SyncHealthbarClientRpc(currentHealth);

        if (currentHealth <= 0)
        {
            if (isBerserk == false)
            {
                snailordBoss.snailordState = SnailordBoss.SnailordState.Phase3; //Set phase to phase 3

                currentHealth = maxHealth; //Refill health
                healthBar.SetHealth(currentHealth);

                isBerserk = true;
            }
            else
            {
                Die();
            }         
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(float damage)
    {
        TakeDamage(damage);
    }

    private void Die()
    {
        Destroy(this.gameObject);
        //if (IsHost)
        //{
        ////if (droppedItem != null) //If the enemy has an item to drop
        ////{
        ////    for (int i = 0; i < 5; i++) //Drop 5 of this enemies item drop
        //    {
        //        var go = Instantiate(droppedItem, transform.position, Quaternion.identity); //Instantiate that item where snail is
        //        go.GetComponent<NetworkObject>().Spawn(); //Spawn item on server
        //        this.mobSpawning.currentSpawns--; //Remove 1 count from current spawns
        //        Destroy(this.gameObject); //Remove this enemy from the game world
        //    }
        //}
        //else if (droppedItem == null) //If enemy has no item
        //{
        //    this.mobSpawning.currentSpawns--; //Remove 1 count from current spawns
        //    Destroy(this.gameObject); //Remove enemy
        //}
        //}
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

    [ClientRpc] //Fired by server, executed on clients
    public void SyncHealthbarClientRpc(float health)
    {
        healthbarObject.SetActive(true);
        healthBar.SetHealth(health);
    }
}
