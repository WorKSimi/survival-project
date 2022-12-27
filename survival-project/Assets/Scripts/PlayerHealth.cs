using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 100;
    private int healthRegen = 1;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        InvokeRepeating("RegenHealth", 1, 2);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("PLAYER HAS DIED!");
        }
    }

    private void RegenHealth()
    {
        if (currentHealth < maxHealth && currentHealth > 0)
        {           
            currentHealth += healthRegen;
            healthBar.SetHealth(currentHealth);
        }
    }
}
