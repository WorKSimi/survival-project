using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WeaponRotation : MonoBehaviour
{
    [SerializeField] private GameObject thisPlayer;
    [SerializeField] private Transform weaponSprite;

    private Transform m_transform;

    private Vector3 mouseWorldPos;
    private Vector3 playerPos2;

    private bool facingRight;
    private bool facingLeft;



    void Start()
    {
        m_transform = this.transform;
    }

    private void RTMouse()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint
        (Input.mousePosition) - m_transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (facingRight == true)
        {
            Quaternion rotation = Quaternion.AngleAxis(angle + 160, Vector3.forward);
            m_transform.rotation = rotation;
        }
        else if (facingLeft == true)
        {
            Quaternion rotation = Quaternion.AngleAxis(angle + 190, Vector3.forward);
            m_transform.rotation = rotation;
        }
    }

    void Update()
    {
        playerPos2 = thisPlayer.transform.position;
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        FacingDirection();

        Vector2 direction = Camera.main.ScreenToWorldPoint
        (Input.mousePosition) - m_transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (facingRight == true)
        {
            Quaternion rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
            m_transform.rotation = rotation;

            if (Input.GetMouseButton(0))
            {
                Debug.Log("Swing Test");
                rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            }
        }
        else if (facingLeft == true)
        {
            Quaternion rotation = Quaternion.AngleAxis(angle + 0, Vector3.forward);
            m_transform.rotation = rotation;
        }


    }
   
    private void FacingDirection()
    {
        if (mouseWorldPos.x > playerPos2.x)
        {
            //weaponSprite.rotation = Quaternion.Euler(0, 0, 38);
            facingRight = true;
            facingLeft = false;
        }

        if (mouseWorldPos.x < playerPos2.x)
        {
            //weaponSprite.rotation = Quaternion.Euler(0, 0, -38);
            facingRight = false;
            facingLeft = true;
        }
    }
}
