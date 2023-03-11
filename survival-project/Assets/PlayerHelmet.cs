using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class PlayerHelmet : NetworkBehaviour
{
    public Image HelmetSprite;
    public MouseItemData mouseItemData;
    public bool IsHelmetEquiped;

    [Header("Helmet Sprites")]

    public Sprite WoodHelmetSprite;
    public Sprite TinHelmetSprite;
    public Sprite IronHelmetSprite;
    public Sprite BronzeHelmetSprite;

    private void Awake()
    {
        IsHelmetEquiped = false;
    }

    public void EquipHelmet() //If you click on the helmet button
    {
        if (IsHelmetEquiped == false) //If theres no helmet equipped
        {
            if (mouseItemData.AssignedInventorySlot.ItemData != null) //And the mouse inventory slot is not null
            {
                if (mouseItemData.AssignedInventorySlot.ItemData.ItemType == "Helmet") //And the mouse inventory item data is equal to helmet
                {
                    Debug.Log("Equipping Helmet!");
                    HelmetCatalog(); //Function for all helmet types
                    mouseItemData.AssignedInventorySlot.ClearSlot(); //Remove the item from the mouse inventory slot
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

    public void HelmetCatalog()
    {
        if (mouseItemData.AssignedInventorySlot.itemData.DisplayName == "WoodHelmet")
        {
            HelmetSprite.sprite = WoodHelmetSprite;
        }

        else if (mouseItemData.AssignedInventorySlot.itemData.DisplayName == "TinHelmet")
        {
            HelmetSprite.sprite = TinHelmetSprite;
        }

        else if (mouseItemData.AssignedInventorySlot.itemData.DisplayName == "IronHelmet")
        {
            HelmetSprite.sprite = IronHelmetSprite;
        }

        else if (mouseItemData.AssignedInventorySlot.itemData.DisplayName == "BronzeHelmet")
        {
            HelmetSprite.sprite = BronzeHelmetSprite;
        }
    }
}
