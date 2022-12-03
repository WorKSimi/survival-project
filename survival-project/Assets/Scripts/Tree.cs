using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Tree : MonoBehaviour
{
    [SerializeField] private GameObject wood;
    [SerializeField] private double maxHealth = 5;

    private double currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(double damage)
    {
        currentHealth -= damage;
        Debug.Log("Tree Hit!");
        //Play hurt animation

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Tree down!");
        //Tree die animation

        Debug.Log("Tree death animation moment");
        // Destroy the tree and drop wood
        
        for (int i = 0; i < 5; i++)
        {
            transform.rotation = Random.rotation;
            Instantiate(wood, transform.position, Quaternion.identity);
            //wood.GetComponent<Rigidbody2D>().AddForce(transform.up * 2, ForceMode2D.Impulse);
        }
        Destroy(this.gameObject);
    }
}
