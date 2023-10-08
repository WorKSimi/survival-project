using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayerOnSpawn : MonoBehaviour
{
    private void Awake()
    {
        this.transform.position = new Vector3Int(100, 100, 0);
    }
}
