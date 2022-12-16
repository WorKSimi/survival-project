using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WeaponRotation : MonoBehaviour
{
    [SerializeField] private GameObject thisPlayer;
    [SerializeField] private Transform weaponSprite;

    private Transform m_transform;
    private SpriteRenderer swordSprite;

    private Vector3 mouseWorldPos;
    private Vector3 playerPos2;

    private bool facingRight;
    private bool facingLeft;
    private bool swungWeapon = false;



    void Start()
    {
        m_transform = this.transform;
        swordSprite = weaponSprite.GetComponent<SpriteRenderer>();
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
        if (Input.GetMouseButtonDown(0))
        {
            swungWeapon = !swungWeapon;
        }

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
            swordSprite.flipX = false;
            swordSprite.flipY = false;

            if (swungWeapon == true)
            {
                rotation = Quaternion.AngleAxis(angle + 0, Vector3.forward);
                m_transform.rotation = rotation;
                swordSprite.flipX = true;
                swordSprite.flipY = true;
            }
        }
        else if (facingLeft == true)
        {
            Quaternion rotation = Quaternion.AngleAxis(angle + 0, Vector3.forward);
            m_transform.rotation = rotation;
            swordSprite.flipX = true;
            swordSprite.flipY = true;

            if (swungWeapon == true)
            {
                rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
                m_transform.rotation = rotation;
                swordSprite.flipX = false;
                swordSprite.flipY = false;
            }
        }


    }
   
    private void FacingDirection()
    {
        if (mouseWorldPos.x > playerPos2.x)
        {          
            facingRight = true;
            facingLeft = false;
        }

        if (mouseWorldPos.x < playerPos2.x)
        {          
            facingRight = false;
            facingLeft = true;
        }
    }
}
