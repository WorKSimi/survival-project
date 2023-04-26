using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Projectile : NetworkBehaviour
{
    public float Projectiledamage;
    public float Projectilelifetime;
    
    public void StartDestructionCoroutine()
    {
        StartCoroutine(DestroyProjectileAfterTime(Projectilelifetime));
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo == null) return; //If collider hit info doesn't exist return

        if (hitInfo.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = hitInfo.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(Projectiledamage);
            }
            DestroyProjectile();
        }

        else if (hitInfo.CompareTag("WallTile"))
        {
            DestroyProjectile();
        }
    }

    public IEnumerator DestroyProjectileAfterTime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime); //Wait for lifetime
        DestroyProjectile(); //Destroy the object
    }

    void DestroyProjectile()
    {
        if (IsHost)
        {
            gameObject.GetComponent<NetworkObject>().Despawn();
        }
        else if (IsClient)
        {
            DestroyObjectServerRpc(); //Destroy on server
            Destroy(this.gameObject); //Destroy on client
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyObjectServerRpc(ServerRpcParams serverRpcParams = default)
    {
        NetworkObject networkObject = this.gameObject.GetComponent<NetworkObject>();
        networkObject.Despawn();
    }
}

