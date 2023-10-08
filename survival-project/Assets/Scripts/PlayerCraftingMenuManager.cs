using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCraftingMenuManager : MonoBehaviour
{
    public GameObject personalMenu;
    public GameObject craftingTableMenu;
    public GameObject furnaceMenu;
    public GameObject anvilMenu;

    public void CraftingTableMenuSelect()
    {
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

    public void DisableAllMenus()
    {
        personalMenu.SetActive(false);
        craftingTableMenu.SetActive(false);
        furnaceMenu.SetActive(false);
        anvilMenu.SetActive(false);
    }
}
