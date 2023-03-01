using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RotateToMouse : NetworkBehaviour
{
    private Transform m_transform;
    public Camera playerCam;

    void Start()
    {
        m_transform = this.transform;
    }

    private void RTMouse()
    {
        Vector2 direction = playerCam.ScreenToWorldPoint
        (Input.mousePosition) - m_transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

        m_transform.rotation = rotation;
    }

    void Update()
    {
        if (!IsLocalPlayer) return;
        if (!IsOwner) return; //Return if not local player OR owner!
        RTMouse();
    }
}
