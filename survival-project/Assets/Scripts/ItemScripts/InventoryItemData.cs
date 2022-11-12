using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// <summary>
// This is a scriptable object, that defines what an item is in our game.
// It could be inherited from to have branched versions of items, for examples potions and equipment.
// <summary>

[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]

public class InventoryItemData : ScriptableObject
{
    public int ID = -1;
    public string DisplayName;
    [TextArea(4,4)]
    public string Description;
    public Sprite Icon;
    public int MaxStackSize;
    public int GoldValue;
    public string ItemType;
    public double Damage;
    public GameObject ItemPrefab;

    public void UseItem()
    {
        switch (ItemType)
        {
            case "Axe":
            Debug.Log("CHOPPER");
            break;

            default:
            Debug.Log($"Using {DisplayName}");
            break;
        }
    }
    //Use item function in the game will use different things based on the logic. Add a
    //Weapon type class somewhere and it inherits from this. Then put that logic into the useItem
    // Function. For example, if the item type is a sword, then use sword logic, and vice versa.
}
