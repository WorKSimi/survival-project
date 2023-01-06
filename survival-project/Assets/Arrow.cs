using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public double Arrowdamage;
    private float lifeTime = 0.9f;

    private void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "Enemy")
        {
            EnemyHealth enemyHealth = hitInfo.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(Arrowdamage);
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

