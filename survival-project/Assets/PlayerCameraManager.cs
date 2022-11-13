using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerCameraManager : NetworkBehaviour
{
    [SerializeField] private GameObject cameraHolder;

    // Start is called before the first frame update
    void Start()
    {
        if (IsOwner)
        {
            cameraHolder.SetActive(true);
        }
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
}

