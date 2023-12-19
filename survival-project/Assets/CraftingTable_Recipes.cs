using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class CraftingTable_Recipes : MonoBehaviour
{
    [SerializeField] private GameObject GearTab;
    [SerializeField] private GameObject StructureTab;

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



    public void SelectGearTab()
    {
        GearTab.SetActive(true);
        StructureTab.SetActive(false);
    }

    public void SelectStructureTab()
    {
        GearTab.SetActive(false);
        StructureTab.SetActive(true);
    }

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

    private SelectedRecipe selected;
    private enum SelectedRecipe
    {
        //Structure Tab
        DirtWall,
        DirtGround,
        StationTable,
        CampFire,
        //Gear Tab
        WoodClub,
        StickBow,
        WoodArrow,
        FlintArrow,
        PopGun,
        WoodHelmet,
        WoodChestplate,
        FlintPickaxe,
        FlintShovel,
    }

    //Region for selecting crafting table recipes
    #region RecipeSelectFunctions_CraftingTable
    
    public void SelectDirtWall()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.DirtWall.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.DirtWall.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.DirtWall.Description); //Set description box text

        ComponentText.SetText("Dirt"); //Set component name
        ComponentAmount.SetText("x1"); //Set component amount 

        selected = SelectedRecipe.DirtWall;
    }

    public void SelectDirtGround()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.DirtGround.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.DirtGround.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.DirtGround.Description); //Set description box text

        ComponentText.SetText("Dirt"); //Set component name
        ComponentAmount.SetText("x1"); //Set component amount 

        selected = SelectedRecipe.DirtGround;
    }
    public void SelectStationTable()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.StationTable.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.StationTable.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.StationTable.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x25"); //Set component amount 

        ComponentText2.SetText("Flint"); //Set second component
        ComponentAmount2.SetText("x10"); //Set second component amount

        selected = SelectedRecipe.StationTable;
    }
    public void SelectCampfire()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.Campfire.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.Campfire.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.Campfire.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x20"); //Set component amount 

        ComponentText2.SetText("Flint"); //Set second component
        ComponentAmount2.SetText("x10"); //Set second component amount

        selected = SelectedRecipe.CampFire;
    }
    public void SelectWoodClub()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.WoodClub.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.WoodClub.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.WoodClub.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x20"); //Set component amount 

        selected = SelectedRecipe.WoodClub;
    }
    public void SelectStickBow()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.WoodBow.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.WoodBow.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.WoodBow.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x15"); //Set component amount 

        selected = SelectedRecipe.StickBow;
    }
    public void SelectWoodArrow()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.WoodArrow.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.WoodArrow.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.WoodArrow.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x1"); //Set component amount 

        selected = SelectedRecipe.WoodArrow;
    }
    public void SelectFlintArrow()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.FlintArrow.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.FlintArrow.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.FlintArrow.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x1"); //Set component amount 

        ComponentText2.SetText("Flint");
        ComponentAmount2.SetText("x1");

        selected = SelectedRecipe.FlintArrow;
    }
    public void SelectWoodHelmet()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.WoodHelmet.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.WoodHelmet.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.WoodHelmet.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x20"); //Set component amount 

        selected = SelectedRecipe.WoodHelmet;
    }
    public void SelectWoodChestplate()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.WoodChestplate.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.WoodChestplate.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.WoodChestplate.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x30"); //Set component amount 

        selected = SelectedRecipe.WoodChestplate;
    }
    public void SelectFlintPickaxe()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.FlintPick.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.FlintPick.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.FlintPick.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x20"); //Set component amount 

        ComponentText2.SetText("Flint");
        ComponentAmount2.SetText("x10");

        selected = SelectedRecipe.FlintPickaxe;
    }

    #endregion //Region for selecting crafting table recipes


    #region RecipeCraftFunctions_CraftingTable

    private void CraftDirtWall()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Dirt Wall");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var woodComponent = new CraftRecipeItem
            {
                displayName = "dirt",
                quantity = 1
            };

            var components = new List<CraftRecipeItem>() { woodComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.DirtWall, 2);
                Debug.Log("Crafted Dirt Wall");
            }
        }
    }
    private void CraftDirtGround()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Dirt Ground");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var woodComponent = new CraftRecipeItem
            {
                displayName = "dirt",
                quantity = 1
            };

            var components = new List<CraftRecipeItem>() { woodComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.DirtGround, 2);
                Debug.Log("Crafted Dirt Ground");
            }
        }
    }
    private void CraftStationTable()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Station Table");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 25
            };
            var flintComponent = new CraftRecipeItem
            {
                displayName = "flint",
                quantity = 10
            };
            var components = new List<CraftRecipeItem>() { woodComponent, flintComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.StationTable, 1);
                Debug.Log("Crafted Campfire");
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
                quantity = 20
            };
            var flintComponent = new CraftRecipeItem
            {
                displayName = "flint",
                quantity = 10
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
                quantity = 20
            };

            var components = new List<CraftRecipeItem>() { woodComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.WoodClub, 1);
                Debug.Log("Crafted Wood Club");
            }
        }
    }
    private void CraftStickBow()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Stick Bow");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 15
            };

            var components = new List<CraftRecipeItem>() { woodComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.WoodBow, 1);
                Debug.Log("Crafted Wood Club");
            }
        }
    }

    private void CraftWoodArrow()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Stick Bow");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 1
            };

            var components = new List<CraftRecipeItem>() { woodComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.WoodArrow, 10);
                Debug.Log("Crafted Wood Club");
            }
        }
    }
    private void CraftFlintArrow()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft flint arrow");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 1
            };
            var flintComponent = new CraftRecipeItem
            {
                displayName = "flint",
                quantity = 1
            };
            var components = new List<CraftRecipeItem>() { woodComponent, flintComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.FlintArrow, 10);
                Debug.Log("Crafted Campfire");
            }
        }
    }
    private void CraftWoodHelmet()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Wood Helmet");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 20
            };

            var components = new List<CraftRecipeItem>() { woodComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.WoodHelmet, 1);
                Debug.Log("Crafted Wood Helm");
            }
        }
    }

    private void CraftWoodChestplate()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Wood Chestplate");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 30
            };

            var components = new List<CraftRecipeItem>() { woodComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.WoodChestplate, 1);
                Debug.Log("Crafted Wood Chestplate");
            }
        }
    }
    private void CraftFlintPickaxe()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft flint pick");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 20
            };
            var flintComponent = new CraftRecipeItem
            {
                displayName = "flint",
                quantity = 10
            };
            var components = new List<CraftRecipeItem>() { woodComponent, flintComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.FlintPick, 1);
                Debug.Log("Crafted Campfire");
            }
        }
    }
    private void CraftFlintShovel()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft flint arrow");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 15
            };
            var flintComponent = new CraftRecipeItem
            {
                displayName = "flint",
                quantity = 5
            };
            var components = new List<CraftRecipeItem>() { woodComponent, flintComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.FlintPick, 10);
                Debug.Log("Crafted Campfire");
            }
        }
    }

    #endregion

    public void CraftItem()
    {
        if (selected == SelectedRecipe.DirtWall)
        {
            CraftDirtWall();
        }
        if (selected == SelectedRecipe.DirtGround)
        {
            CraftDirtGround();
        }
        if (selected == SelectedRecipe.StationTable)
        {
            CraftStationTable();
        }
        if (selected == SelectedRecipe.CampFire)
        {
            CraftCampfire();
        }
        if (selected == SelectedRecipe.WoodClub)
        {
            CraftWoodenClub();
        }
        if (selected == SelectedRecipe.StickBow)
        {
            CraftStickBow();
        }
        if (selected == SelectedRecipe.WoodArrow)
        {
            CraftWoodArrow();
        }
        if (selected == SelectedRecipe.FlintArrow)
        {
            CraftFlintArrow();
        }       
        if (selected == SelectedRecipe.WoodHelmet)
        {
            CraftWoodHelmet();
        }
        if (selected == SelectedRecipe.WoodChestplate)
        {
            CraftWoodChestplate();
        }
        if (selected == SelectedRecipe.FlintPickaxe)
        {
            CraftFlintPickaxe();
        }
        if (selected == SelectedRecipe.FlintShovel)
        {
            CraftFlintShovel();
        }
        else Debug.Log("Nothing is selected to craft!");
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
