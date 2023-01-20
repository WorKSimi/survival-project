using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public double Projectiledamage;
    public float Projectilelifetime;

    private void Start()
    {
        Invoke("DestroyProjectile", Projectilelifetime);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "Enemy")
        {
            EnemyHealth enemyHealth = hitInfo.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(Projectiledamage);
            }
            DestroyProjectile();
        }

        else if (hitInfo.tag != "Enemy")
        {
            Debug.Log("Something other than enemy was hit!");
        }
    }

    void DestroyProjectile()
    {
        //Insert stuff like effects and sounds here.
        Destroy(gameObject);
    }
}

