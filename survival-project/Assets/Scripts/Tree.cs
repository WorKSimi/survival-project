using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Tree : NetworkBehaviour
{
    [SerializeField] private GameObject wood;
    [SerializeField] private double maxHealth = 5;
    double currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(double damage)
    {
        currentHealth -= damage;

        //Play hurt animation

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Tree down!");
        //Tree die animation

        // Destroy the tree and drop wood
        for (int i = 0; i < 5; i++)
        {
            Instantiate(wood);
        }
        Destroy(this.gameObject);
    }


}
