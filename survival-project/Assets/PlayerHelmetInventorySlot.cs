using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerHelmetInventorySlot : MonoBehaviour
{
    public int HelmetDefenseValue;
    public Image ItemSprite;
    public InventorySlot helmInventorySlot;
    public Database database;
    public MouseItemData mouseItemData;

    public void UpdateHelmetSlot(InventorySlot invSlot)
    {
        helmInventorySlot.AssignItem(invSlot);
        UpdateHelmetSlot();
    }
    public void UpdateHelmetSlot()
    {
        ItemSprite.sprite = helmInventorySlot.ItemData.Icon;
        HelmetDefenseValue = helmInventorySlot.ItemData.DefenseValue;
    }

    public void ClearSlot()
    {
        helmInventorySlot.ClearSlot();
        ItemSprite.sprite = null;
    }

    public void HelmetSlotButtonClicked() //This function runs when clicking on the pipe slot
    {
        if (mouseItemData.AssignedInventorySlot.itemData != null) //if the mouse item data isnt null, 
        {
            if (mouseItemData.AssignedInventorySlot.itemData.ItemType == "Helmet") //and that item is a pipe...
            {
                Debug.Log("mouse slot not empty and mouse slot is a helmet");
                if (helmInventorySlot.itemData == null) //If theres no helmet equipped.
                {
                    Debug.Log("Equipping Helmet");
                    UpdateHelmetSlot(mouseItemData.AssignedInventorySlot); //update helmet slot with the mouse item data.
                    mouseItemData.ClearSlot(); //Clear mouse slot
                }
            }
        }
        else if (helmInventorySlot.itemData != null && mouseItemData.AssignedInventorySlot.itemData == null) //if pipe slot is not null, and mouse slot is
        {
            mouseItemData.AssignedInventorySlot.itemData = helmInventorySlot.itemData; //Set mouse data to the right pipe.
            helmInventorySlot.ClearSlot();
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
