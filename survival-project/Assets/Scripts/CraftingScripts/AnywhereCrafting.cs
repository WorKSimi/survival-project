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
    [SerializeField] private Button craftButton;

    [SerializeField] private TMP_Text NameText; //Text for what you are crafting
    [SerializeField] private Image RecipeIcon; //Icon of what you are crafting
    [SerializeField] private TMP_Text ItemDescriptionText; //Description text for what your crafting

    [SerializeField] private TMP_Text ComponentText; //Name of Component Text
    [SerializeField] private TMP_Text ComponentAmount; //Number of component needed
    [SerializeField] private GameObject ComponentIcon; //Picture of the needed component

    [SerializeField] private TMP_Text ComponentText2; //Name of Component Text
    [SerializeField] private TMP_Text ComponentAmount2; //Number of component needed
    [SerializeField] private GameObject ComponentIcon2; //Picture of the needed component

    [SerializeField] private TMP_Text ComponentText3; //Name of Component Text
    [SerializeField] private TMP_Text ComponentAmount3; //Number of component needed
    [SerializeField] private GameObject ComponentIcon3; //Picture of the needed component

    [SerializeField] private TMP_Text ComponentText4; //Name of Component Text
    [SerializeField] private TMP_Text ComponentAmount4; //Number of component needed
    [SerializeField] private GameObject ComponentIcon4; //Picture of the needed component

    [SerializeField] private TMP_Text ComponentText5; //Name of Component Text
    [SerializeField] private TMP_Text ComponentAmount5; //Number of component needed
    [SerializeField] private GameObject ComponentIcon5; //Picture of the needed component

    [SerializeField] private Button increaseAmount;
    [SerializeField] private Button decreaseAmount;

    [SerializeField] private TextMeshProUGUI amountToCraftText;
    private int amountToCraft = 1;

    public void IncreaseAmount()
    {
        amountToCraft++;
        amountToCraftText.SetText(amountToCraft.ToString());
    }

    public void DecreaseAmount()
    {
        if (amountToCraft > 1)
        {
            amountToCraft--;
            amountToCraftText.SetText(amountToCraft.ToString());
        }
        else Debug.Log("Cannot craft less than 1!");
    }

    //Amount to craft will be set to 1 on default
    //It can not go below 1
    //When you click the right arrow, it will increase the amount by 1
    //When you click the left arrow, it will decrease by 1, UNLESS it IS 1.
    //Amount of times an item crafts is based on the quantity using a loop

    private SelectedRecipe selected;

    private enum SelectedRecipe
    {
        CraftingTable,
        Campfire,
        WoodClub,
        FlintPickaxe,
        Torch,
        WoodBow,
        WoodWall,
    }
    public void SelectCraftingTable()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.CraftingTable.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.CraftingTable.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.CraftingTable.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x10"); //Set component amount  

        selected = SelectedRecipe.CraftingTable; //Set the selected item to Crafting Table
    }
    public void SelectCampfire()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.Campfire.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.Campfire.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.Campfire.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x5"); //Set component amount 

        ComponentText2.SetText("Flint"); //Set second component
        ComponentAmount2.SetText("x2"); //Set second component amount

        selected = SelectedRecipe.Campfire; //Set the selected item to campfire
    }

    public void SelectWoodClub()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.WoodClub.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.WoodClub.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.WoodClub.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x15"); //Set component amount 

        selected = SelectedRecipe.WoodClub;
    }

    public void SelectFlintPick()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.FlintPick.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.FlintPick.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.FlintPick.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x5"); //Set component amount 

        ComponentText2.SetText("Flint"); //Set second component
        ComponentAmount2.SetText("x5"); //Set second component amount

        selected = SelectedRecipe.FlintPickaxe;
    }

    public void SelectTorch()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.Torch.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.Torch.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.Torch.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x1"); //Set component amount 

        selected = SelectedRecipe.Torch;
    }
    public void SelectWoodBow()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.WoodBow.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.WoodBow.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.WoodBow.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x15"); //Set component amount 

        selected = SelectedRecipe.WoodBow;
    }
    public void SelectWoodWall()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.WoodWall.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.WoodWall.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.WoodWall.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x5"); //Set component amount 

        selected = SelectedRecipe.WoodWall;
    }

    public void CraftItem()
    {
        if (selected == SelectedRecipe.CraftingTable)
        {
            CraftCraftingTable();
        }
        if (selected == SelectedRecipe.Campfire)
        {
            CraftCampfire();
        }
        if (selected == SelectedRecipe.WoodClub)
        {
            CraftWoodenClub();
        }
        if (selected == SelectedRecipe.FlintPickaxe)
        {
            CraftFlintPickaxe();
        }
        if (selected == SelectedRecipe.Torch)
        {
            CraftTorch();
        }
        if (selected == SelectedRecipe.WoodBow)
        {
            CraftWoodBow();
        }
        if (selected == SelectedRecipe.WoodWall)
        {
            CraftWoodWall();
        }
        else Debug.Log("Nothing is selected to craft!");
    }

    private void Awake()
    {
        ClearComponents();
        selected = SelectedRecipe.CraftingTable;
        anywhereCraftingMenu.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            anywhereCraftingMenu.SetActive(false);
            
            Debug.Log("Personal Crafting Menu Off");
        }

        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            //anywhereCraftingMenu.SetActive(true);     
            Debug.Log("Personal Crafting Menu On");
        }
    }

    private void CraftCraftingTable()
    {
        for (int i = 0; i < amountToCraft; i++)
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
    }

    private void CraftCampfire()
    {
        for (int i = 0; i < amountToCraft; i++)
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

            var components = new List<CraftRecipeItem>() { woodComponent, flintComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.Campfire, 1);
                Debug.Log("Crafted Campfire");
            }
        }
    }

    private void CraftWoodenClub()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Wood Club");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 15
            };

            var components = new List<CraftRecipeItem>() { woodComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.WoodClub, 1);
                Debug.Log("Crafted Wood Club");
            }
        }
    }

    private void CraftFlintPickaxe()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Flint Pickaxe");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 5
            };

            var flintComponent = new CraftRecipeItem
            {
                displayName = "flint",
                quantity = 5
            };

            var components = new List<CraftRecipeItem>() { woodComponent, flintComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.FlintPick, 1);
                Debug.Log("Crafted Flint Pickaxe");
            }
        }
    }

    private void CraftTorch()
    {
        for (int i = 0; i < amountToCraft; i++)
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
    }
    private void CraftWoodWall()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Wood Wall");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 5
            };

            var components = new List<CraftRecipeItem>() { woodComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.WoodWall, 2);
                Debug.Log("Crafted WoodWall");
            }
        }
    }

    private void CraftWoodBow()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Wood Bow");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 15
            };

            var components = new List<CraftRecipeItem>() { woodComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.WoodBow, 2);
                Debug.Log("Crafted Wood Bow");
            }
        }
    }

    private void ClearComponents()
    {
        ComponentText.SetText(""); 
        ComponentAmount.SetText("");  

        ComponentText2.SetText(""); 
        ComponentAmount2.SetText("");

        ComponentText3.SetText("");
        ComponentAmount3.SetText("");

        ComponentText4.SetText("");
        ComponentAmount4.SetText("");

        ComponentText5.SetText("");
        ComponentAmount5.SetText("");
    }
}
