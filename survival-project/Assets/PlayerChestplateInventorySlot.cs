using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerChestplateInventorySlot : MonoBehaviour
{
    public int ChestDefenseValue;
    public Image ItemSprite;
    public InventorySlot chestInventorySlot;
    public Database database;
    public MouseItemData mouseItemData;

    public void UpdateChestSlot(InventorySlot invSlot)
    {
        chestInventorySlot.AssignItem(invSlot);
        UpdateChestSlot();
    }
    public void UpdateChestSlot()
    {
        ItemSprite.sprite = chestInventorySlot.ItemData.Icon;
        ChestDefenseValue = chestInventorySlot.ItemData.DefenseValue;
    }

    public void ClearSlot()
    {
        chestInventorySlot.ClearSlot();
        ItemSprite.sprite = null;
    }

    public void ChestSlotButtonClicked() //This function runs when clicking on the pipe slot
    {
        if (mouseItemData.AssignedInventorySlot.itemData != null) //if the mouse item data isnt null, 
        {
            if (mouseItemData.AssignedInventorySlot.itemData.ItemType == "Chestplate") //and that item is a pipe...
            {
                Debug.Log("mouse slot not empty and mouse slot is a Chestplate");
                if (chestInventorySlot.itemData == null) //If theres no helmet equipped.
                {
                    Debug.Log("Equipping Chest");
                    UpdateChestSlot(mouseItemData.AssignedInventorySlot); //update helmet slot with the mouse item data.
                    mouseItemData.ClearSlot(); //Clear mouse slot
                }
            }
        }
        else if (chestInventorySlot.itemData != null && mouseItemData.AssignedInventorySlot.itemData == null) //if pipe slot is not null, and mouse slot is
        {
            mouseItemData.AssignedInventorySlot.itemData = chestInventorySlot.itemData; //Set mouse data to the right pipe.
            chestInventorySlot.ClearSlot();
            ClearSlot();
            mouseItemData.UpdateMouseSlot();
        }
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
