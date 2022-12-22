using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject arrowPrefab;
    private float bulletForce = 10f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}
