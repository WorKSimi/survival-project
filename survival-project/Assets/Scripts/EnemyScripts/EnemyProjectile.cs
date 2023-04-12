using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyProjectile : NetworkBehaviour
{
    public int enemyProjectileDamage;
    public float enemyProjectileLifetime;
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo == null) return; //If collider hit info doesn't exist return

        if (hitInfo.CompareTag("Player"))
        {
            PlayerHealth playerHealth = hitInfo.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(enemyProjectileDamage);
            }
            DestroyProjectile();
        }

        else if (hitInfo.tag != "Enemy")
        {
            Debug.Log("Something other than enemy was hit!");
        }
    }

    public void StartDestructionCoroutine()
    {
        //Debug.Log("Projectile on function call: " + Projectilelifetime);
        StartCoroutine(DestroyProjectileAfterTime(enemyProjectileLifetime));
    }
    public IEnumerator DestroyProjectileAfterTime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime); //Wait for lifetime
        DestroyProjectile(); //Destroy the object
    }
    void DestroyProjectile()
    {
        gameObject.GetComponent<NetworkObject>().Despawn();
    }
}
