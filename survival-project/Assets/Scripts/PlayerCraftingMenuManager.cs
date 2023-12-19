using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCraftingMenuManager : MonoBehaviour
{
    public GameObject personalMenu;
    public GameObject craftingTableMenu;
    public GameObject stationTableMenu;
    public GameObject furnaceMenu;
    public GameObject anvilMenu;

    public void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            this.GetComponent<PlayerCraftingMenuManager>().DisableAllMenus();
        }
    }

    public void CraftingTableMenuSelect()
    {
        Debug.Log("Crafting Table Menu Selected");
        DisableAllMenus();
        craftingTableMenu.SetActive(true);
    }

    public void FurnaceMenuSelect()
    {
        DisableAllMenus();
        furnaceMenu.SetActive(true);
    }

    public void AnvilMenuSelect()
    {
        DisableAllMenus();
        anvilMenu.SetActive(true);
    }

    public void StationMenuSelect()
    {
        Debug.Log("Station Menu Selected!");
        DisableAllMenus();
        stationTableMenu.SetActive(true);
    }

    public void DisableAllMenus()
    {
        personalMenu.SetActive(false);
        craftingTableMenu.SetActive(false);
        furnaceMenu.SetActive(false);
        anvilMenu.SetActive(false);
        stationTableMenu.SetActive(false);
    }
}
