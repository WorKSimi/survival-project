using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class PlayerHelmetSlot : NetworkBehaviour
{
    public Image HelmetSprite;
    public MouseItemData mouseItemData;
    public bool IsHelmetEquiped;
    private void Awake()
    {
        
    }

    public void EquipHelmet() //If you click on the helmet button
    {
        if (IsHelmetEquiped == false) //If theres no helmet equipped
        {
            if (mouseItemData.AssignedInventorySlot.ItemData != null) //And the mouse inventory slot is not null
            {
                if (mouseItemData.AssignedInventorySlot.ItemData.ItemType == "Helmet") //And the mouse inventory item data is equal to helmet
                {
                    //Remove the item from the mouse inventory slot
                    mouseItemData.AssignedInventorySlot.ClearSlot();

                    //Change the picture of the helmet slot to the helmet
                    HelmetSprite.sprite = mouseItemData.AssignedInventorySlot.ItemData.Icon;
                }
            }
        }
        else if (IsHelmetEquiped == true) //If a helmet IS equipped
        {
            if (mouseItemData.AssignedInventorySlot.ItemData == null) //Mouse inventory slot is empty
            {
                //Set the helmet slot data back to empty
                //Add the item the helmet is equivilant to into the mouse item data
            }
        }
    }
}
