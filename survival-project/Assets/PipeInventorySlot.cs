using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PipeInventorySlot : MonoBehaviour
{
    public Image ItemSprite;
    public InventorySlot pipeInventorySlot;
    public Database database;
    public MouseItemData mouseItemData;
    


    public void UpdatePipeSlot(InventorySlot invSlot)
    {
        pipeInventorySlot.AssignItem(invSlot);
        UpdatePipeSlot();
    }
    public void UpdatePipeSlot()
    {
        ItemSprite.sprite = pipeInventorySlot.ItemData.Icon;
    }

    public void ClearSlot()
    {
        pipeInventorySlot.ClearSlot();
        ItemSprite.sprite = null;
    }

    public void PipeSlotButtonClicked() //This function runs when clicking on the pipe slot
    {
        
        if (mouseItemData.AssignedInventorySlot.itemData != null) //if the mouse item data isnt null, 
        {
            if (mouseItemData.AssignedInventorySlot.itemData.ItemType == "Pipe") //and that item is a pipe...
            {
                Debug.Log("mouse slot not empty and mouse slot is a pipe");
                if (pipeInventorySlot.itemData == null) //If theres no pipe equipped.
                {
                    UpdatePipeSlot(mouseItemData.AssignedInventorySlot); //update pipe slot with the mouse item data.
                    mouseItemData.ClearSlot(); //Clear mouse slot
                }
            }         
        }
        else if (pipeInventorySlot.itemData != null && mouseItemData.AssignedInventorySlot.itemData == null) //if pipe slot is not null, and mouse slot is
        {
            mouseItemData.AssignedInventorySlot.itemData = pipeInventorySlot.itemData; //Set mouse data to the right pipe.
            pipeInventorySlot.ClearSlot();
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
