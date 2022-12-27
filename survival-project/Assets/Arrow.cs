using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public double Arrowdamage;
    private float lifeTime = 0.5f;

    private void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        EnemyHealth enemyHealth = hitInfo.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(Arrowdamage);
        }
        DestroyProjectile();
    }

    void DestroyProjectile()
    {
        //Insert stuff like effects and sounds here.
        Destroy(gameObject);
    }
}

