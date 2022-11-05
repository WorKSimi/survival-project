using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : InventoryHolder
{

    
    public static UnityAction OnPlayerInventoryChanged;

    private void Start()
    {
        SaveGameManager.data.playerInventory = new InventorySaveData(inventorySystem);
    }

    protected override void LoadInventory(SaveData data)
    {
        //Check the save data for this specific chests inventory, and if it exists, load it in.
        if (data.playerInventory.InvSystem != null)
        {
            this.inventorySystem = data.playerInventory.InvSystem; 
            OnPlayerInventoryChanged?.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame) OnDynamicInventoryDisplayRequested?.Invoke(inventorySystem, offset);
    }

    public bool AddToInventory(InventoryItemData data, int amount)
    {
        if (InventorySystem.AddToInventory(data, amount))
        {
            return true;
        }
        
        return false;
    }
}
