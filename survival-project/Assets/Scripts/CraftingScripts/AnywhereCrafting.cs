using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class AnywhereCrafting : MonoBehaviour
{
    [SerializeField] private GameObject anywhereCraftingMenu;
    [SerializeField] private GameObject thisPlayer;
   
    public void CraftCraftingTable()
    {
        Debug.Log("Trying to Craft Crafting Table");
        var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();
        var woodComponent = new CraftRecipeItem
        {
            displayName = "wood",
            quantity = 10
        };

        var components = new List<CraftRecipeItem>() { woodComponent };
        {
            playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.CraftingTable, 1);
            Debug.Log("Crafted Crafting Table");
        }
    }

    public void CraftTorch()
    {
        Debug.Log("Trying to Craft Torch");
        var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

        var woodComponent = new CraftRecipeItem
        {
            displayName = "wood",
            quantity = 1
        };

        var components = new List<CraftRecipeItem>() { woodComponent };
        {
            playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.Torch, 2);
            Debug.Log("Crafted Torch");
        }
    }

    private void Awake()
    {
        anywhereCraftingMenu.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            anywhereCraftingMenu.SetActive(false);
            
            Debug.Log("Personal Crafting Menu Off");
        }

        //if (Keyboard.current.bKey.wasPressedThisFrame)
        //{
        //    //anywhereCraftingMenu.SetActive(true);     
        //    Debug.Log("Personal Crafting Menu On");
        //}
    }
}
