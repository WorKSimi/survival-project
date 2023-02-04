using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerCanvasController : NetworkBehaviour
{
    public GameObject craftingCanvas;
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            gameObject.SetActive(false);
            craftingCanvas.SetActive(false);
        }
    }
}
