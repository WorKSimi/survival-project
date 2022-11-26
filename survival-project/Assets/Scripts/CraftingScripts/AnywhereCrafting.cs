using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class AnywhereCrafting : MonoBehaviour
{
    [SerializeField] private GameObject anywhereCraftingMenu;
    [SerializeField] private GameObject thisPlayer;
    //private bool menuActive;

    private void Awake()
    {
        anywhereCraftingMenu.SetActive(false);
        //menuActive = false;
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            anywhereCraftingMenu.SetActive(false);
            //menuActive = false;
            Debug.Log("Personal Crafting Menu Off");
        }

        else if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            anywhereCraftingMenu.SetActive(true);
            //menuActive = true;
            Debug.Log("Personal Crafting Menu On");
        }
        return;
    }

    public void CraftCraftingTable()
    {
        //CurrentInteractor = interactor;
        Debug.Log("Trying to Craft Crafting Table");
        var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

        var woodComponent = new CraftRecipeItem
        {
            displayName = "wood",
            quantity = 5
        };

        var components = new List<CraftRecipeItem>() { woodComponent};
        {
            playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.CraftingTable, 1);
            Debug.Log("Crafted Flint Axe");
        }
    }
}
