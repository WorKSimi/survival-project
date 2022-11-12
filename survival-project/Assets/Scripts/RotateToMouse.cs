using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RotateToMouse : NetworkBehaviour
{
    private Transform m_transform;

    void Start()
    {
        if (IsOwner)
        m_transform = this.transform;
    }

    private void RTMouse()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint
        (Input.mousePosition) - m_transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

        m_transform.rotation = rotation;
    }

    void Update()
    {
        RTMouse();
    }
}
