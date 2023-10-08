using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeHerbOnSpawner : MonoBehaviour
{
    [SerializeField] private InventoryItemData wornPipeData;
    [SerializeField] private InventoryItemData greebHerbItemData;

    [SerializeField] private PipeInventorySlot pipeSlot;
    [SerializeField] private HerbInventorySlot herbSlot;

    private void AddStarterPipeHerbToInventory()
    {
        pipeSlot.pipeInventorySlot.itemData = wornPipeData;
        herbSlot.herbInventorySlot.itemData = greebHerbItemData;
        herbSlot.herbInventorySlot.stackSize = 10;

        pipeSlot.UpdatePipeSlot();
        herbSlot.UpdateHerbSlot();
    }

    private void Awake()
    {
        AddStarterPipeHerbToInventory();
    }
}
