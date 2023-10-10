using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.Netcode;

public class HerbInventorySlot : MonoBehaviour
{
    public Image ItemSprite;
    public InventorySlot herbInventorySlot;
    public Database database;
    public MouseItemData mouseItemData;
    public TextMeshProUGUI herbItemCount;

    public void UpdateHerbSlot(InventorySlot invSlot)
    {
        herbInventorySlot.AssignItem(invSlot);
        UpdateHerbSlot();
    }
    public void UpdateHerbSlot()
    {
        ItemSprite.sprite = herbInventorySlot.ItemData.Icon;
        herbItemCount.text = herbInventorySlot.StackSize.ToString();
    }

    public void ClearSlot()
    {
        herbInventorySlot.ClearSlot();
        ItemSprite.sprite = null;     
    }

    public void HerbSlotButtonClicked() //This function runs when clicking on the pipe slot
    {
        if (mouseItemData.AssignedInventorySlot.itemData != null) //if the mouse item data isnt null, 
        {
            if (mouseItemData.AssignedInventorySlot.itemData.ItemType == "Herb") //and that item is a Herb...
            {
                if (herbInventorySlot.itemData == null) //If theres no herb equipped
                {
                    UpdateHerbSlot(mouseItemData.AssignedInventorySlot); //update pipe slot with the mouse item data.
                    mouseItemData.ClearSlot(); //Clear mouse slot
                }
                else if (herbInventorySlot.itemData == mouseItemData.AssignedInventorySlot.itemData) //If the herb in mouse slot is same as one equipped
                {
                    herbInventorySlot.AddToStack(mouseItemData.AssignedInventorySlot.stackSize); //Add the amount in mouse to the herb count
                    UpdateHerbSlot(); //update herb slot
                    mouseItemData.ClearSlot(); //Clear mouse slot
                    mouseItemData.UpdateMouseSlot(); //update the mouse slot
                }
            }
        }
        else if (herbInventorySlot.itemData != null && mouseItemData.AssignedInventorySlot.itemData == null) //if pipe slot is not null, and mouse slot is
        {
            mouseItemData.AssignedInventorySlot.itemData = herbInventorySlot.itemData; //Set mouse data to the right pipe.
            mouseItemData.AssignedInventorySlot.stackSize = herbInventorySlot.stackSize;
            herbInventorySlot.ClearSlot();
            //UpdateHerbSlot();
            ClearSlot();
            mouseItemData.UpdateMouseSlot();
        }
    }
}
