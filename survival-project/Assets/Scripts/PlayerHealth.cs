using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 100;

    public bool healthRegenOn;
    public bool isHealthFull;
    private int healthRegen = 1; //How much health is regenerated

    private PlayerNetwork playerNetwork;

    public HealthBar healthBar;

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
            Debug.Log("PLAYER HAS DIED!"); //DIE!
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
}
