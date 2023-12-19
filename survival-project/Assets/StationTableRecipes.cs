using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class StationTableRecipes : MonoBehaviour
{
    private SelectedRecipe selected;

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

    private enum SelectedRecipe
    {
        Smelter,
        Anvil,
        CarpentryTable,
        StonemasonTable,
        CookingPot,
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

    #region selectRecipes

    public void SelectSmelter()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.Furnace.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.Furnace.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.Furnace.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x10"); //Set component amount 

        ComponentText2.SetText("Stone"); //Set component name
        ComponentAmount2.SetText("x10"); //Set component amount 

        ComponentText3.SetText("Flint"); //Set component name
        ComponentAmount3.SetText("x10"); //Set component amount 

        selected = SelectedRecipe.Smelter;
    }

    public void SelectAnvil()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.Anvil.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.Anvil.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.Anvil.Description); //Set description box text

        ComponentText.SetText("Tin"); //Set component name
        ComponentAmount.SetText("x10"); //Set component amount 

        selected = SelectedRecipe.Anvil;
    }

    public void SelectCarpenter()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.CarpenterTable.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.CarpenterTable.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.CarpenterTable.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x10"); //Set component amount 

        ComponentText2.SetText("Stone"); //Set component name
        ComponentAmount2.SetText("x5"); //Set component amount 

        ComponentText3.SetText("Tin"); //Set component name
        ComponentAmount3.SetText("x15"); //Set component amount 

        selected = SelectedRecipe.CarpentryTable;
    }

    public void SelectMason()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.StoneMasonTable.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.StoneMasonTable.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.StoneMasonTable.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x10"); //Set component amount 

        ComponentText2.SetText("Stone"); //Set component name
        ComponentAmount2.SetText("x10"); //Set component amount 

        ComponentText3.SetText("Iron"); //Set component name
        ComponentAmount3.SetText("x5"); //Set component amount 

        selected = SelectedRecipe.StonemasonTable;
    }

    public void SelectCookingpot()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.CookingPot.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.CookingPot.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.CookingPot.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x10"); //Set component amount 

        ComponentText2.SetText("Stone"); //Set component name
        ComponentAmount2.SetText("x10"); //Set component amount 

        ComponentText3.SetText("Flint"); //Set component name
        ComponentAmount3.SetText("x10"); //Set component amount 

        ComponentText4.SetText("Tin");
        ComponentAmount4.SetText("x5");

        selected = SelectedRecipe.CookingPot;
    }

    #endregion


    #region craftRecipeFunctions

    private void CraftForge()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Forge");
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
                Debug.Log("Crafted Forge");
            }
        }
    }
    private void CraftAnvil()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Forge");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var tinComponent = new CraftRecipeItem
            {
                displayName = "tin bar",
                quantity = 10
            };           
            var components = new List<CraftRecipeItem>() { tinComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.Anvil, 1);
                Debug.Log("Crafted Anvil");
            }
        }
    }

    private void CraftCarpenter()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Carpentry Table");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 10
            };
            var tinComponent = new CraftRecipeItem
            {
                displayName = "tin bar",
                quantity = 5
            };
            var stoneComponent = new CraftRecipeItem
            {
                displayName = "stone",
                quantity = 5
            };
            var components = new List<CraftRecipeItem>() { woodComponent, tinComponent , stoneComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.CarpenterTable, 1);
                Debug.Log("Crafted CarpenterTable");
            }
        }
    }

    private void CraftMason()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft StoneMason");
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 10
            };
            var ironComponent = new CraftRecipeItem
            {
                displayName = "iron bar",
                quantity = 5
            };
            var stoneComponent = new CraftRecipeItem
            {
                displayName = "stone",
                quantity = 10
            };
            var components = new List<CraftRecipeItem>() { woodComponent, ironComponent, stoneComponent };
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.StoneMasonTable, 1);
                Debug.Log("Crafted Forge");
            }
        }
    }
    private void CraftCookingPot()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            Debug.Log("Trying to Craft Cook Pot");
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
            var tinComponent = new CraftRecipeItem
            {
                displayName = "tin bar",
                quantity = 5
            };
            var components = new List<CraftRecipeItem>() { woodComponent, flintComponent, stoneComponent, tinComponent};
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.CookingPot, 1);
                Debug.Log("Crafted Cook Pot");
            }
        }
    }

    #endregion

    public void CraftItem()
    {
        if (selected == SelectedRecipe.Smelter)
        {
            CraftForge();
        }
        if (selected == SelectedRecipe.Anvil)
        {
            CraftAnvil();
        }
        if (selected == SelectedRecipe.CarpentryTable)
        {
            CraftCarpenter();
        }
        if (selected == SelectedRecipe.StonemasonTable)
        {
            CraftMason();
        }
        if (selected == SelectedRecipe.CookingPot)
        {
            CraftCookingPot();
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
