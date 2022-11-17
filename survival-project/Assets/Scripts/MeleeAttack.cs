using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask treeLayers;

    private InventoryItemData inventoryItemData;
    private float attackRange = 0.5f; 
    private double attackDamage = 0.5; 
    
    private float attackRate = 2f; //How many times you can attack per second
    float nextAttackTime = 0f;

    public void Update()
    {
        if (Time.time >= nextAttackTime)
            {
            Attack();
            }
    }

    void Attack()
    {
            animator.SetTrigger("Attack"); // Play an attack animation

            Collider2D[] hitTrees = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, treeLayers); // Detect trees in range of attack

            foreach (Collider2D tree in hitTrees) // Damage trees
            {
                tree.GetComponent<Tree>().TakeDamage(attackDamage);
            }

            nextAttackTime = Time.time + 1f / attackRate;
    }

    void OnDrawGizmosSelected()
    {   
        if(attackPoint == null)
        return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}


