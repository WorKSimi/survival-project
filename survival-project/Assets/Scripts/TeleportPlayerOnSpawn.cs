using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayerOnSpawn : MonoBehaviour
{
    private Vector3Int spawnPos = new Vector3Int(113, 113, 0);

    private void Awake()
    {
        StartCoroutine(TeleportPlayer());
        Debug.Log("Teleporting Player to Spawn");
    }

    private IEnumerator TeleportPlayer()
    {
        yield return new WaitForSeconds(0.2f);
        this.transform.position = spawnPos;
        Debug.Log("Teleported!");
    }
}
