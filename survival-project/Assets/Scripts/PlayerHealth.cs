using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject lastStandCooldownUIObject;
    [SerializeField] private TMP_Text lastStandCountdownText;
    [SerializeField] private TMP_Text healthBarText;

    [SerializeField] private SpriteRenderer weaponSprite;
    [SerializeField] private SpriteRenderer heldGun;

    [SerializeField] private SpriteRenderer playerSprite;

    [SerializeField] private GameObject deathUIHolder;
    [SerializeField] private TMP_Text deathCooldownText;

    public int currentHealth;
    public int maxHealth = 100;
    public bool isPlayerDead = false;

    public bool healthRegenOn;
    public bool isHealthFull;
    public bool invincibile;
    private int healthRegen = 1; //How much health is regenerated

    public int playerTotalDefense = 0; //Total defense value the player has
    private int playerHelmetDefense = 0; //Defense value the player helmet has
    private int playerChestplateDefense = 0; //Defense value the player chestplate has

    private float respawnTime = 5f; //Value for respawn time

    private float lastStandCooldown = 60f; //Cooldown for last stand (30 Seconds)

    private bool lastStand = true; //Bool for last stand. True on default

    private PlayerNetwork playerNetwork;
    public PlayerChestplate playerChestplate;
    public PlayerHelmet playerHelmet;
    public HealthBar healthBar;
    public Vector3Int respawnPoint = new Vector3Int(113, 113, 0); //Default respawn point.
    private SpriteRenderer spriteRend;
    private bool didRespawn;

    private float deathTimer;

    private void Awake()
    {
        playerNetwork = this.gameObject.GetComponent<PlayerNetwork>();
        spriteRend = this.gameObject.GetComponent<SpriteRenderer>();
    }

    void Start() // Start is called before the first frame update
    {        
        currentHealth = maxHealth; //Set current health to max health
        healthBar.SetMaxHealth(maxHealth); //Set the health bar to max health
        healthBarText.text = currentHealth.ToString() + " / " + maxHealth.ToString();

        if (healthRegenOn == true) //If regenerating health bool is true
        {
            InvokeRepeating("RegenHealth", 1, 2); //Regen hp every second
        }
    }

    private float timer;

    private void Update()
    {
        if (lastStand == false)
        {
            timer -= Time.deltaTime;
            int timer2 = ((int)timer);
            lastStandCountdownText.text = timer2.ToString();
        }    

        if (isPlayerDead == true)
        {
            deathTimer -= Time.deltaTime;
            int timer2 = ((int)deathTimer);
            deathCooldownText.text = timer2.ToString();
        }
    }

    public void TakeDamage(int amount) //Taking Damage
    {
        if (playerNetwork.state != PlayerNetwork.State.Rolling) //If player is not rolling
        {
            var damageToTake = amount - playerTotalDefense; //Subtract player defense from amount to take
            if (damageToTake < 1) damageToTake = 1; //If the damage to take is less then 1, set it to 1
            currentHealth -= damageToTake; //Take damage           
            healthBarText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
            healthBar.SetHealth(currentHealth); //Update Healthbar
            StartCoroutine(Invulnerability());
        }

        if (currentHealth <= 0 && lastStand == true) //If your health is equal to or less than 0, and last stand is true
        {
            lastStand = false;
            currentHealth = 1;
            healthBar.SetHealth(currentHealth);
            healthBarText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
            StartCoroutine(LastStandCooldown());
        }
        else if (currentHealth <= 0 && lastStand == false) //If your health is less than 0, and last stand is not avaliable.
        {
            StartCoroutine(Die()); //Die
            healthBarText.text = "Dead";
        }
    }

    private IEnumerator LastStandCooldown()
    {
        lastStandCooldownUIObject.SetActive(true);
        timer = lastStandCooldown;
        yield return new WaitForSeconds(lastStandCooldown);
        lastStandCooldownUIObject.SetActive(false);
        lastStand = true;
    }

    public IEnumerator Die() //Function for player death
    {
        playerSprite.enabled = false;
        weaponSprite.enabled = false;
        heldGun.enabled = false;

        playerNetwork.state = PlayerNetwork.State.Dead; //Set state to dead
        deathUIHolder.SetActive(true);
        isPlayerDead = true;
        didRespawn = false;

        deathTimer = respawnTime + 1;
        yield return new WaitForSeconds(respawnTime); //Wait for respawn time
        playerSprite.enabled = true;
        weaponSprite.enabled = true;
        heldGun.enabled = true;

        deathUIHolder.SetActive(false);
        isPlayerDead = false;

        if (didRespawn == false)
        {
            this.gameObject.transform.position = respawnPoint; //Set player position back to respawn point
            didRespawn = true;
        }
        
        playerNetwork.state = PlayerNetwork.State.Normal; //Set state back to normal
        currentHealth = maxHealth; //Set player health back to full
        healthBar.SetHealth(currentHealth); //Update the health bar
        healthBarText.text = currentHealth.ToString() + " / " + maxHealth.ToString();

        if (lastStand == false) //If last stand is on cooldown, reset it
        {
            StopCoroutine(LastStandCooldown()); //Turn off the cooldown
            lastStand = true;
            lastStandCooldownUIObject.SetActive(false);
        }
    }
  
    public void HealHealth(int amount) //Function for food healing HP
    {
        currentHealth += amount; //Add the amount of health to be healed to your players health
        healthBar.SetHealth(currentHealth); //Update the health bar
        healthBarText.text = currentHealth.ToString() + " / " + maxHealth.ToString();

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
