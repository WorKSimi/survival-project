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

        else if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            anywhereCraftingMenu.SetActive(true);
            
            Debug.Log("Personal Crafting Menu On");
        }
        return;
    }

    public void CraftCraftingTable()
    {        
        Debug.Log("Trying to Craft Crafting Table");
        var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

        var woodComponent = new CraftRecipeItem
        {
            displayName = "wood",
            quantity = 10
        };

        var components = new List<CraftRecipeItem>() { woodComponent};
        {
            playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.CraftingTable, 1);
            Debug.Log("Crafted Crafting Table");
        }
    }

    public void CraftCampfire()
    {
        Debug.Log("Trying to Craft Campfire");
        var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

        var woodComponent = new CraftRecipeItem
        {
            displayName = "wood",
            quantity = 5
        };

        var flintComponent = new CraftRecipeItem
        {
            displayName = "flint",
            quantity = 2
        };

        var components = new List<CraftRecipeItem>() { woodComponent, flintComponent};
        {
            playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.Campfire, 1);
            Debug.Log("Crafted Campfire");
        }
    }

    public void CraftWoodenClub()
    {        
        Debug.Log("Trying to Craft Wood Club");
        var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

        var woodComponent = new CraftRecipeItem
        {
            displayName = "wood",
            quantity = 5
        };

        var components = new List<CraftRecipeItem>() { woodComponent};
        {
            playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.WoodClub, 1);
            Debug.Log("Crafted Wood Club");
        }
    }
}
