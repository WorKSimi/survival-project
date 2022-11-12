using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask treeLayers;

    [SerializeField] private float attackRange = 0.5f; 
    [SerializeField] private double attackDamage = 0.5; 
    
   // private double itemDamage = InventoryItemData.Damage;
    //private InventoryItemData itemData;
    [SerializeField] private float attackRate = 2f; //How many times you can attack per second
    float nextAttackTime = 0f;

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    public void Attack()
    {
        // Play an attack animation
        animator.SetTrigger("Attack");

        // Detect trees in range of attack
        Collider2D[] hitTrees = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, treeLayers);

        // Damage trees
        foreach (Collider2D tree in hitTrees)
        {
            tree.GetComponent<Tree>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {   
        if(attackPoint == null)
        return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}


