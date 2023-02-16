using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCraftingMenuManager : MonoBehaviour
{
    public GameObject personalMenu;
    public GameObject craftingTableMenu;

    public void CraftingTableMenuSelect()
    {
        personalMenu.SetActive(false);
        craftingTableMenu.SetActive(true);
    }

    public void DisableAllMenus()
    {
        personalMenu.SetActive(false);
        craftingTableMenu.SetActive(false);
    }
}
