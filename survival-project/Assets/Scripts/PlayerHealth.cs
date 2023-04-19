using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 100;

    public bool healthRegenOn;
    public bool isHealthFull;
    public bool invincibile;
    private int healthRegen = 1; //How much health is regenerated

    public int playerTotalDefense = 0; //Total defense value the player has
    private int playerHelmetDefense = 0; //Defense value the player helmet has
    private int playerChestplateDefense = 0; //Defense value the player chestplate has
    private float respawnTime = 5f; //Value for respawn time

    private PlayerNetwork playerNetwork;
    public PlayerChestplate playerChestplate;
    public PlayerHelmet playerHelmet;
    public HealthBar healthBar;
    public Vector3Int respawnPoint = new Vector3Int(0, 0, 0);
    private SpriteRenderer spriteRend;

    private void Awake()
    {
        playerNetwork = this.gameObject.GetComponent<PlayerNetwork>();
        spriteRend = this.gameObject.GetComponent<SpriteRenderer>();
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
            var damageToTake = amount - playerTotalDefense; //Subtract player defense from amount to take
            if (damageToTake < 1) damageToTake = 1; //If the damage to take is less then 1, set it to 1
            currentHealth -= damageToTake; //Take damage           
            healthBar.SetHealth(currentHealth); //Update Healthbar
            StartCoroutine(Invulnerability());
        }

        if (currentHealth <= 0) //If your health is equal to or less than 0
        {
            StartCoroutine(Die());
        }
    }

    public IEnumerator Die() //Function for player death
    {
        playerNetwork.state = PlayerNetwork.State.Dead; //Set state to dead
        yield return new WaitForSeconds(respawnTime); //Wait for respawn time
        this.gameObject.transform.position = respawnPoint; //Set player position back to respawn point
        playerNetwork.state = PlayerNetwork.State.Normal; //Set state back to normal

        currentHealth = maxHealth; //Set player health back to full
        healthBar.SetHealth(currentHealth); //Update the health bar

        //TO DO - DEATH ANIMATION!
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
    public void UpdateArmor() //function used by other scripts to update the players armor
    {
        playerChestplateDefense = playerChestplate.ChestplateDefense;
        playerHelmetDefense = playerHelmet.helmetDefense;

        playerTotalDefense = playerChestplateDefense + playerHelmetDefense;
    }
    private float iFramesDuration = 0.5f;
    private float numberOfFlashes = 3f;

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(9 , 10, true);
        invincibile = true;
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(9, 10, false);
        invincibile = false;
    }
}
