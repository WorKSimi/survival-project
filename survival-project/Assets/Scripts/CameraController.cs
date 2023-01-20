using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CameraController : NetworkBehaviour
{
    public GameObject cameraHolder;

    private void Start()
    {
        if (IsLocalPlayer)
        {
            cameraHolder.SetActive(true);
        }
        else
        {
            cameraHolder.SetActive(false);
        }
    }
}
