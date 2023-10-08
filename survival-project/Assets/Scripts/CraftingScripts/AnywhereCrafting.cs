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

    public TextMeshProUGUI amountToCraftText;
    public int amountToCraft = 1;

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
        WoodHelmet,
        WoodChestplate,
        TinBar,
        IronBar,
        Furnace,
        TinAnvil,
        TinSword,
        TinHelmet,
        TinChestplate,
        TinPickaxe,
        WoodFloor,
        Chest,
        IronAnvil,
    }

    //Region for selecting anywhere crafting
    #region RecipeSelectFunctions_Anywhere
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

    #endregion 

    //Region for selecting crafting table recipes
    #region RecipeSelectFunctions_CraftingTable
    public void SelectFurnace()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.Furnace.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.Furnace.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.Furnace.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x10"); //Set component amount  

        ComponentText2.SetText("Flint"); //Set second component
        ComponentAmount2.SetText("x10"); //Set second component amount

        ComponentText3.SetText("Stone"); //Set second component
        ComponentAmount3.SetText("x10"); //Set second component amount

        selected = SelectedRecipe.Furnace; //Set the selected item to Crafting Table
    }

    public void SelectFlintPick()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.FlintPick.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.FlintPick.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.FlintPick.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x10"); //Set component amount 

        ComponentText2.SetText("Flint"); //Set second component
        ComponentAmount2.SetText("x10"); //Set second component amount

        selected = SelectedRecipe.FlintPickaxe;
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
        ComponentAmount.SetText("x1"); //Set component amount 

        selected = SelectedRecipe.WoodWall;
    }
    public void SelectTinAnvil()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.TinAnvil.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.TinAnvil.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.TinAnvil.Description); //Set description box text

        ComponentText.SetText("Tin Bar"); //Set component name
        ComponentAmount.SetText("x5"); //Set component amount 

        selected = SelectedRecipe.TinAnvil;
    }
    public void SelectWoodHelmet()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.WoodHelmet.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.WoodHelmet.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.WoodHelmet.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x10"); //Set component amount 

        selected = SelectedRecipe.WoodHelmet;
    }
    public void SelectWoodChestplate()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.WoodChestplate.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.WoodChestplate.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.WoodChestplate.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x20"); //Set component amount 

        selected = SelectedRecipe.WoodChestplate;
    }
    public void SelectWoodFloor()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.WoodFloor.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.WoodFloor.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.WoodFloor.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x1"); //Set component amount 

        selected = SelectedRecipe.WoodFloor;
    }
    public void SelectChest()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.Chest.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.Chest.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.Chest.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x10"); //Set component amount 

        selected = SelectedRecipe.Chest;
    }
    #endregion //Region for selecting crafting table recipes

    //Region for selecting furnace recipes
    #region RecipeSelectFunctions_Furnace
    public void SelectTinBar()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.TinBar.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.TinBar.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.TinBar.Description); //Set description box text

        ComponentText.SetText("Tin Ore"); //Set component name
        ComponentAmount.SetText("x1"); //Set component amount 

        selected = SelectedRecipe.TinBar;
    }

    public void SelectIronBar()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.IronBar.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.IronBar.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.IronBar.Description); //Set description box text

        ComponentText.SetText("Iron Ore"); //Set component name
        ComponentAmount.SetText("x1"); //Set component amount 

        selected = SelectedRecipe.IronBar;
    }
    #endregion

    //Region for selecting tin anvil recipes
    #region RecipeSelectFunctions_TinAnvil
    public void SelectTinSword()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.TinSword.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.TinSword.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.TinSword.Description); //Set description box text

        ComponentText.SetText("Tin Bar"); //Set component name
        ComponentAmount.SetText("x10"); //Set component amount 

        ComponentText2.SetText("Wood");
        ComponentAmount2.SetText("x5");

        selected = SelectedRecipe.TinSword;
    }
    public void SelectTinPickaxe()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.TinPickaxe.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.TinPickaxe.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.TinPickaxe.Description); //Set description box text

        ComponentText.SetText("Tin Bar"); //Set component name
        ComponentAmount.SetText("x10"); //Set component amount 

        ComponentText2.SetText("Wood");
        ComponentAmount2.SetText("x5");

        selected = SelectedRecipe.TinPickaxe;
    }
    public void SelectTinHelmet()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.TinHelmet.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.TinHelmet.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.TinHelmet.Description); //Set description box text

        ComponentText.SetText("Tin Bar"); //Set component name
        ComponentAmount.SetText("x10"); //Set component amount 

        selected = SelectedRecipe.TinHelmet;
    }
    public void SelectTinChestplate()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.TinChestplate.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.TinChestplate.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.TinChestplate.Description); //Set description box text

        ComponentText.SetText("Tin Bar"); //Set component name
        ComponentAmount.SetText("x10"); //Set component amount 

        selected = SelectedRecipe.TinHelmet;
    }
    public void SelectIronAnvil()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.IronAnvil.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.IronAnvil.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.IronAnvil.Description); //Set description box text

        ComponentText.SetText("Iron Bar"); //Set component name
        ComponentAmount.SetText("x5"); //Set component amount 

        selected = SelectedRecipe.IronAnvil;
    }
    #endregion

    /// <summary>
    /// Seperator for Selection and Crafting Functions
    /// </summary>

    //Region for anywhere crafting functions
    #region RecipeCraftingFunctions_Anywhere
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
    #endregion

    //Region for crafting table crafting functions
    #region RecipeCraftingFunctions_CraftingTable
    private void CraftFurnace()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Furnace");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();
            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 10
            };

            var flintComponent = new CraftRecipeItem
            {
                displayName = "flint",
                quantity = 10
            };

            var stoneComponent = new CraftRecipeItem
            {
                displayName = "stone",
                quantity = 10
            };

            var components = new List<CraftRecipeItem>() { woodComponent, flintComponent, stoneComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.Furnace, 1);
                Debug.Log("Crafted Furnace");
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
                quantity = 10
            };

            var flintComponent = new CraftRecipeItem
            {
                displayName = "flint",
                quantity = 10
            };

            var components = new List<CraftRecipeItem>() { woodComponent, flintComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.FlintPick, 1);
                Debug.Log("Crafted Flint Pickaxe");
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
                quantity = 1
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
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.WoodBow, 1);
                Debug.Log("Crafted Wood Bow");
            }
        }
    }
    private void CraftTinAnvil()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Tin Anvil");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var tinComponent = new CraftRecipeItem
            {
                displayName = "tin bar",
                quantity = 5
            };

            var components = new List<CraftRecipeItem>() { tinComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.TinAnvil, 1);
                Debug.Log("Crafted Tin Anvil");
            }
        }
    }

    private void CraftWoodHelmet()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 10
            };

            var components = new List<CraftRecipeItem>() { woodComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.WoodHelmet, 1);
            }
        }
    }
    private void CraftWoodChestplate()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 20
            };

            var components = new List<CraftRecipeItem>() { woodComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.WoodChestplate, 1);
            }
        }
    }

    private void CraftWoodFloor()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Wood Floor");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 1
            };

            var components = new List<CraftRecipeItem>() { woodComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.WoodFloor, 2);
                Debug.Log("Crafted WoodFloor");
            }
        }
    }

    private void CraftChest()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Chest");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 10
            };

            var components = new List<CraftRecipeItem>() { woodComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.Chest, 1);
                Debug.Log("Crafted Chest");
            }
        }
    }
    #endregion

    //Region for furnace crafting functions
    #region RecipeCraftingFunctions_Furnace
    private void CraftTinBar()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Tin Bar");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var tinComponent = new CraftRecipeItem
            {
                displayName = "tin ore",
                quantity = 1
            };

            var components = new List<CraftRecipeItem>() { tinComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.TinBar, 1);
                Debug.Log("Crafted Tin Bar");
            }
        }
    }
    private void CraftIronBar()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Iron Bar");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var tinComponent = new CraftRecipeItem
            {
                displayName = "iron ore",
                quantity = 1
            };

            var components = new List<CraftRecipeItem>() { tinComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.IronBar, 1);
                Debug.Log("Crafted Iron Bar");
            }
        }
    }
    #endregion

    //Region for tinAnvil crafting functions
    #region RecipeCraftingFunctions_TinAnvil
    private void CraftTinSword()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Tin Sword");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var tinComponent = new CraftRecipeItem
            {
                displayName = "tin bar",
                quantity = 10
            };

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 5
            };

            var components = new List<CraftRecipeItem>() { tinComponent, woodComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.TinSword, 1);
                Debug.Log("Crafted Tin Sword");
            }
        }
    }
    private void CraftTinPickaxe()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Tin Pickaxe");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var tinComponent = new CraftRecipeItem
            {
                displayName = "tin bar",
                quantity = 10
            };

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 5
            };

            var components = new List<CraftRecipeItem>() { tinComponent, woodComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.TinPickaxe, 1);
                Debug.Log("Crafted Tin Pickaxe");
            }
        }
    }
    private void CraftTinHelmet()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Tin Helmet");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var tinComponent = new CraftRecipeItem
            {
                displayName = "tin bar",
                quantity = 10
            };

            var components = new List<CraftRecipeItem>() { tinComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.TinHelmet, 1);
                Debug.Log("Crafted Tin Helmet");
            }
        }
    }
    private void CraftTinChestplate()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Tin Chestplate");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var tinComponent = new CraftRecipeItem
            {
                displayName = "tin bar",
                quantity = 10
            };

            var components = new List<CraftRecipeItem>() { tinComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.TinChestplate, 1);
                Debug.Log("Crafted Tin Chestplate");
            }
        }
    }
    private void CraftIronAnvil()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Iron Anvil");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var ironComponent = new CraftRecipeItem
            {
                displayName = "iron bar",
                quantity = 5
            };

            var components = new List<CraftRecipeItem>() { ironComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.IronAnvil, 1);
                Debug.Log("Crafted Iron Chestplate");
            }
        }
    }
    #endregion

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
        if (selected == SelectedRecipe.TinBar)
        {
            CraftTinBar();
        }
        if (selected == SelectedRecipe.Furnace)
        {
            CraftFurnace();
        }
        if (selected == SelectedRecipe.TinAnvil)
        {
            CraftTinAnvil();
        }
        if (selected == SelectedRecipe.TinSword)
        {
            CraftTinSword();
        }
        if (selected == SelectedRecipe.TinHelmet)
        {
            CraftTinHelmet();
        }
        if (selected == SelectedRecipe.TinChestplate)
        {
            CraftTinChestplate();
        }
        if (selected == SelectedRecipe.TinPickaxe)
        {
            CraftTinPickaxe();
        }
        if (selected == SelectedRecipe.WoodHelmet)
        {
            CraftWoodHelmet();
        }
        if (selected == SelectedRecipe.WoodChestplate)
        {
            CraftWoodChestplate();
        }
        if (selected == SelectedRecipe.WoodFloor)
        {
            CraftWoodFloor();
        }
        if (selected == SelectedRecipe.Chest)
        {
            CraftChest();
        }
        if (selected == SelectedRecipe.IronAnvil)
        {
            CraftIronAnvil();
        }
        if (selected == SelectedRecipe.IronBar)
        {
            CraftIronBar();
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
