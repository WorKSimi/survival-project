using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RockSpawner : NetworkBehaviour
{
    public InventoryItemData rockItemData;
    [SerializeField] private PlayerInventoryHolder playerInventory;

    public void AddRockToInventory()
    {
        if (!IsLocalPlayer) return; //if your not local player return
        playerInventory.AddToInventory(rockItemData, 1); //Give player a rock
    }
}
