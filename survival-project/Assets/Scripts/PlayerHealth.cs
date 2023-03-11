using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerHealth : NetworkBehaviour
{
    public int currentHealth;
    public int maxHealth = 100;

    public bool healthRegenOn;
    public bool isHealthFull;
    private int healthRegen = 1; //How much health is regenerated

    private PlayerNetwork playerNetwork;

    public HealthBar healthBar;
    //public Transform playerRespawnPoint;
    private Vector3Int respawnPosition = new Vector3Int(0, 0, 0);

    private void Awake()
    {
        playerNetwork = this.gameObject.GetComponent<PlayerNetwork>();
    }

    void Start() // Start is called before the first frame update
    {        
        currentHealth = maxHealth; //Set current health to max health
        healthBar.SetMaxHealth(maxHealth); //Set the health bar to max health

        if (healthRegenOn == true) //If regenerating health bool is true
        {
            InvokeRepeating("RegenHealth", 1, 2); //Regen hp every second
        }
    }

    public void TakeDamage(int amount) //Taking Damage
    {
        if (playerNetwork.state != PlayerNetwork.State.Rolling) //If player is not rolling
        {
            currentHealth -= amount; //Take damage
            healthBar.SetHealth(currentHealth); //Update Healthbar
        }

        if (currentHealth <= 0) //If your health is equal to or less than 0
        {
            Debug.Log("YOU DIED!");
            StartCoroutine(PlayerRespawn());
        }
    }

    public void HealHealth(int amount) //Function for food healing HP
    {
        currentHealth += amount; //Add the amount of health to be healed to your players health
        healthBar.SetHealth(currentHealth); //Update the health bar

        if (currentHealth > maxHealth) //If your current health goes ABOVE max health
        {
            currentHealth = maxHealth; //Set current health to be max health (no overheal)
        }
    }

    private void RegenHealth() //Regenerating Health Function (Can be turned on or off)
    {
        if (currentHealth < maxHealth && currentHealth > 0)
        {           
            currentHealth += healthRegen;
            healthBar.SetHealth(currentHealth);
        }
    }
    private float RespawnTime = 3f;
    private IEnumerator PlayerRespawn()
    {
        bool respawned = false;
        //When a player dies, play a death animation
        playerNetwork.state = PlayerNetwork.State.Dead;
        yield return new WaitForSeconds(RespawnTime); //Wait for respawn time
        currentHealth = maxHealth; //Current health will also be set back to max health
        healthBar.SetHealth(currentHealth); //Update the health bar
        if (respawned == false)
        {
            this.gameObject.transform.position = respawnPosition; //Set player position to respawn position (default is 0, 0, 0)
            respawned = true;
        }
        playerNetwork.state = PlayerNetwork.State.Normal;
        yield break; //End the couroutine
    }
}
