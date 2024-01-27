using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyHealth : NetworkBehaviour
{
    [SerializeField] private GameObject droppedItem;
    [SerializeField] private int dropAmount = 1;

    [SerializeField] private float maxHealth;
    [SerializeField] private SimpleFlash flashEffect;
    [SerializeField] private GameObject healthbarObject;
    public HealthBar healthBar;

    [SerializeField] private GameObject damagePopup;
    //[SerializeField] private AudioSource hitSound;

    

    public MobSpawning mobSpawning;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthbarObject.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        healthbarObject.SetActive(true);
        currentHealth -= damage;
        flashEffect.Flash(); //Make enemy flash when hit
        healthBar.SetHealth(currentHealth); //Set health bar here
        SyncHealthbarClientRpc(currentHealth);

        Vector3 pos = new Vector3(this.transform.position.x + 1, this.transform.position.y + 1, 0);
        var go = Instantiate(damagePopup, pos, Quaternion.identity);
        go.GetComponent<DamagePopup>().CreatePopup(damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(float damage)
    {
        TakeDamage(damage);
    }

    private void Die()
    {
        if (IsHost)
        {
            if (droppedItem != null) //If the enemy has an item to drop
            {
                for (int i = 0; i < dropAmount; i++) //Drop 5 of this enemies item drop
                {
                    var go = Instantiate(droppedItem, transform.position, Quaternion.identity); //Instantiate that item where snail is
                    go.GetComponent<NetworkObject>().Spawn(); //Spawn item on server
                    //this.mobSpawning.currentSpawns--; //Remove 1 count from current spawns
                    Destroy(this.gameObject); //Remove this enemy from the game world
                }
            }
            else if (droppedItem == null) //If enemy has no item
            {
                //this.mobSpawning.currentSpawns--; //Remove 1 count from current spawns
                Destroy(this.gameObject); //Remove enemy
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

    [ClientRpc] //Fired by server, executed on clients
    public void SyncHealthbarClientRpc(float health)
    {
        healthbarObject.SetActive(true);
        healthBar.SetHealth(health);
    }
}
