using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

// <summary>
// This is a scriptable object, that defines what an item is in our game.
// It could be inherited from to have branched versions of items, for examples potions and equipment.
// <summary>

[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]

public class InventoryItemData : ScriptableObject
{
    [Header("Universal")]
    public int ID = -1;
    public string DisplayName;
    [TextArea(4,4)]
    public string Description;
    public Sprite Icon;
    public int MaxStackSize;
    public int GoldValue;
    public string ItemType;
    public GameObject ItemPrefab;

    [Header("Weapons and Tools")]
    public float itemDamage;
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float projectileLifetime;
    public float attackRate;

    [Header("Equipment and Armor")]
    public int DefenseValue;

    [Header("Pipe and Herb")]
    public float pipeHealCooldown; //Int for the healing cooldown
    public float pipeHealTime; //Time it takes to heal
    public int herbHealValue; //Amount healed by herb

    [Header("Other")]
    public int BlockID;
    //public int TilemapID; //1 = Wall
    public GameObject BlockPrefab;
    //public string ItemTilemap;
    public int HealthHealed;
    public int BlockHealth;

    public void UseItem()
    {
        
    }
}
    //Use item function in the game will use different things based on the logic. Add a
    //Weapon type class somewhere and it inherits from this. Then put that logic into the useItem
    // Function. For example, if the item type is a sword, then use sword logic, and vice versa.

