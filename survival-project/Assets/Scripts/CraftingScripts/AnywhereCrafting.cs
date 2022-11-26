using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class AnywhereCrafting : MonoBehaviour
{
    [SerializeField] private GameObject anywhereCraftingMenu;
    [SerializeField] private GameObject thisPlayer;

    private void Awake()
    {
        anywhereCraftingMenu.SetActive(false);
    }

    public void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            anywhereCraftingMenu.SetActive(false);
        }
        else if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            anywhereCraftingMenu.SetActive(true);
        }
    }

    //public void CraftCraftingTable()
    //{
    //    Debug.Log("Trying to Craft Crafting Table");
    //    var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

    //    var woodComponent = new CraftRecipeItem
    //    {
    //        displayName = "wood",
    //        quantity = 5
    //    };

    //    var components = new List<CraftRecipeItem>() { woodComponent };
    //    {
    //        playerInventoryHolder.inventorySystem.CraftItem(components);
    //        Debug.Log("Contains Wood");
    //    }
    //}
}