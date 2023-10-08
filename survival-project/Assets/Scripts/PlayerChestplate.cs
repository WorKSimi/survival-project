using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class PlayerChestplate : NetworkBehaviour
{
    public Image ChestplateSprite;
    public MouseItemData mouseItemData;
    public PlayerHealth playerHealth;
    public bool IsChestplateEquiped;
    public string equippedChestplate = "";
    public int ChestplateDefense;

    public GameObject chestplateSpriteObject;
    public Animator chestplateAnimator;

    [Header("Helmet Sprites")]

    public Sprite WoodChestplateSprite;
    public Sprite TinChestplateSprite;
    public Sprite IronChestplateSprite;
    public Sprite BronzeChestplateSprite;

    [Header("Helmet Item Data")]

    public InventoryItemData WoodChestplate;
    public InventoryItemData TinChestplate;
    public InventoryItemData IronChestplate;
    public InventoryItemData BronzeChestplate;

    private void Awake()
    {
        IsChestplateEquiped = false;
        chestplateSpriteObject.SetActive(false);
    }

    public void EquipChestplate() //If you click on the helmet button
    {
        if (IsChestplateEquiped == false) //If theres no helmet equipped
        {
            if (mouseItemData.AssignedInventorySlot.ItemData != null) //And the mouse inventory slot is not null
            {
                if (mouseItemData.AssignedInventorySlot.ItemData.ItemType == "Chestplate") //And the mouse inventory item data is equal to helmet
                {
                    Debug.Log("Equipping Chestplate!");
                    IsChestplateEquiped = true;
                    ChestplateCatalog(); //Function for all helmet types
                    mouseItemData.ClearSlot(); //Remove the item from the mouse inventory slot
                    playerHealth.UpdateArmor(); //Update the armor value of the player
                    chestplateSpriteObject.SetActive(true);
                }
            }
        }
        else if (IsChestplateEquiped == true) //If a helmet IS equipped
        {
            if (mouseItemData.AssignedInventorySlot.ItemData == null) //Mouse inventory slot is empty
            {
                Debug.Log("Removing Chestplate!");
                IsChestplateEquiped = false;
                ChestplateRemoval();
                ChestplateDefense = 0;
                playerHealth.UpdateArmor();
                mouseItemData.UpdateMouseSlot();
                chestplateSpriteObject.SetActive(false);
            }
        }
    }

    public void ChestplateCatalog()
    {
        if (mouseItemData.AssignedInventorySlot.itemData.DisplayName == "Wood Chestplate")
        {
            ChestplateSprite.sprite = WoodChestplateSprite;
            equippedChestplate = "Wood Chestplate";
            ChestplateDefense = WoodChestplate.DefenseValue;
            Debug.Log("Wood Chestplate Equipped");
        }

        else if (mouseItemData.AssignedInventorySlot.itemData.DisplayName == "Tin Chestplate")
        {
            ChestplateSprite.sprite = TinChestplateSprite;
            equippedChestplate = "Tin Chestplate";
            ChestplateDefense = TinChestplate.DefenseValue;
            Debug.Log("Tin Helmet Equipped");
        }

        else if (mouseItemData.AssignedInventorySlot.itemData.DisplayName == "Iron Chestplate")
        {
            ChestplateSprite.sprite = IronChestplateSprite;
            equippedChestplate = "Iron Chestplate";
            ChestplateDefense = IronChestplate.DefenseValue;
            Debug.Log("Iron Helmet Equipped");
        }

        else if (mouseItemData.AssignedInventorySlot.itemData.DisplayName == "Bronze Chestplate")
        {
            ChestplateSprite.sprite = BronzeChestplateSprite;
            equippedChestplate = "Bronze Chestplate";
            ChestplateDefense = BronzeChestplate.DefenseValue;
            Debug.Log("Bronze Helmet Equipped");
        }
    }

    public void ChestplateRemoval()
    {
        if (equippedChestplate == "Wood Chestplate")
        {
            ChestplateSprite.sprite = null;
            equippedChestplate = "";
            mouseItemData.AssignedInventorySlot.itemData = WoodChestplate;           
            Debug.Log("Wood Helmet Removed");
        }

        else if (equippedChestplate == "Tin Chestplate")
        {
            ChestplateSprite.sprite = null;
            equippedChestplate = "";
            mouseItemData.AssignedInventorySlot.itemData = TinChestplate;
            Debug.Log("Tin Helmet Removed");
        }

        else if (equippedChestplate == "Iron Chestplate")
        {
            ChestplateSprite.sprite = null;
            equippedChestplate = "";
            mouseItemData.AssignedInventorySlot.itemData = IronChestplate;
            Debug.Log("Iron Helmet Removed");
        }

        else if (equippedChestplate == "Bronze Chestplate")
        {
            ChestplateSprite.sprite = null;
            equippedChestplate = "";
            mouseItemData.AssignedInventorySlot.itemData = BronzeChestplate;
            Debug.Log("Bronze Helmet Removed");
        }
    }
}
