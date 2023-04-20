using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RockSpawner : NetworkBehaviour
{
    public InventoryItemData rockItemData;

    private void Awake() //When player spawns
    {
        if (IsHost)
        {
            GameObject go = Instantiate(rockItemData.ItemPrefab, this.transform.position, Quaternion.identity); //Generate rock
            go.GetComponent<NetworkObject>().Spawn(); //Spawn it on server
        }

        if (IsClient)
        {
            SpawnRockServerRpc(this.transform.position);
        }
    }

    [ServerRpc]
    private void SpawnRockServerRpc(Vector3 position)
    {
        GameObject go = Instantiate(rockItemData.ItemPrefab, position, Quaternion.identity); //Generate rock
        go.GetComponent<NetworkObject>().Spawn(); //Spawn it on server
    }
}
