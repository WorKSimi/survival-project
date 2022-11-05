using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]

public class ChestInventory : InventoryHolder, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete {get; set;}

    protected override void Awake()
    {
        base.Awake();
        SaveLoad.OnLoadGame += LoadInventory;
    }

    private void Start()
    {
        var InventorySaveData = new InventorySaveData(inventorySystem, transform.position, transform.rotation);

        SaveGameManager.data.chestDictionary.Add(GetComponent<UniqueID>().ID, InventorySaveData);
    }

    protected override void LoadInventory(SaveData data)
    {
        //Check the save data for this specific chests inventory, and if it exists, load it in.
        if (data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out InventorySaveData chestData))
        {
            this.inventorySystem = chestData.InvSystem;
            this.transform.position = chestData.Position;
            this.transform.rotation = chestData.Rotation;
        }
    }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        OnDynamicInventoryDisplayRequested?.Invoke(inventorySystem, 0);
        interactSuccessful = true;
    }

    public void EndInteraction()
    {

    }
        
}
