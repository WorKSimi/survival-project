using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerCanvasController : NetworkBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            gameObject.SetActive(false);
        }
    }
}
