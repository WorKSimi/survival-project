using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Assets.Scripts;

public struct CraftRecipeItem
{
    public string displayName;
    public int quantity;
}

public struct FoundItem
{
    public InventorySlot itemSlot;
    public int quantity;
}

[System.Serializable]
//Note, making this a network behaivor causes the U.I. to stop functioning for inventory and chests.
public class InventorySystem
{
    [SerializeField] public List<InventorySlot> inventorySlots;

    public List<InventorySlot> InventorySlots => inventorySlots;

    public int InventorySize => InventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChanged;

    public InventorySystem(int size) // Constructor that sets the amount of slots.
    {
        inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        if (ContainsItem(itemToAdd, out List<InventorySlot> invSlot)) //Check whether item exists in inventory.
        {
            foreach (var slot in invSlot)
            {
                if (slot.EnoughRoomLeftInStack(amountToAdd))
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }
        }


        if (HasFreeSlot(out InventorySlot freeSlot)) //Gets the first avaliable slot
        {
            if (freeSlot.EnoughRoomLeftInStack(amountToAdd))
            {
                freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
                OnInventorySlotChanged?.Invoke(freeSlot);
                return true;
            }
            // Add implementation to only take what can fill the stack, and check for another free slot to put the ramainder in.
        }

        return false;
    }

    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlot) // Do any of our slots have the item to add in them?
    {
        invSlot = InventorySlots.Where(i => i.ItemData == itemToAdd).ToList(); // If they do, then get a list of all of them.
        return invSlot == null ? false : true; // If they do return true, if not return false.
    }

    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null); // Get the first free slot
        return freeSlot == null ? false : true;
    }

    public void CraftItem(List<CraftRecipeItem> itemComponents, InventoryItemData createdItem, int itemAmount)
    {
        var FoundItems = new List<FoundItem>();

        foreach (var InventorySlot in InventorySlots)
        {
            if (InventorySlot.itemData == null) continue;

            foreach (var itemComponent in itemComponents)
            {
                if (InventorySlot.itemData.DisplayName.Equals(itemComponent.displayName, System.StringComparison.OrdinalIgnoreCase))
                {
                    if (InventorySlot.stackSize >= itemComponent.quantity)
                    {
                        FoundItems.Add(new FoundItem { itemSlot = InventorySlot, quantity = itemComponent.quantity});
                    }
                }
            }

        }

        if (FoundItems.Count == itemComponents.Count)
        {
            foreach (var foundItem in FoundItems)
            {
                foundItem.itemSlot.RemoveFromStack(foundItem.quantity); //Removes material amount from your inventory
                OnInventorySlotChanged.Invoke(foundItem.itemSlot);
            }
            var CreatedItem = GameObject.Instantiate(createdItem);
            AddToInventory(CreatedItem, itemAmount);
        }
    }
}
