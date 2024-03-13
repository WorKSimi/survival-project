using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TinAnvilRecipes : MonoBehaviour
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
        TinHelmet,
        TinChestplate,
        TinSword,
        TinPickaxe,
        TinBow,
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

    public void SelectTinHelmet()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.TinHelmet.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.TinHelmet.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.TinHelmet.Description); //Set description box text

        ComponentText.SetText("Tin Bar"); //Set component name
        ComponentAmount.SetText("x15"); //Set component amount 

        selected = SelectedRecipe.TinHelmet;
    }

    public void SelectTinChestplate()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.TinChestplate.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.TinChestplate.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.TinChestplate.Description); //Set description box text

        ComponentText.SetText("Tin Bar"); //Set component name
        ComponentAmount.SetText("x25"); //Set component amount 

        selected = SelectedRecipe.TinChestplate;
    }

    public void SelectTinSword()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.TinSword.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.TinSword.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.TinSword.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x10"); //Set component amount 

        ComponentText2.SetText("Tin Bar"); //Set component name
        ComponentAmount2.SetText("x10"); //Set component amount 

        selected = SelectedRecipe.TinSword;
    }

    public void SelectTinBow()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.ReinforcedBow.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.ReinforcedBow.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.ReinforcedBow.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x10"); //Set component amount 

        ComponentText2.SetText("Tin Bar"); //Set component name
        ComponentAmount2.SetText("x10"); //Set component amount 

        selected = SelectedRecipe.TinBow;
    }

    public void SelectTinPickaxe()
    {
        ClearComponents();

        NameText.SetText(GameManager.Instance.TinPickaxe.DisplayName); //Set the text to what your crafting
        RecipeIcon.sprite = GameManager.Instance.TinPickaxe.Icon; //Set icon to the item
        ItemDescriptionText.SetText(GameManager.Instance.TinPickaxe.Description); //Set description box text

        ComponentText.SetText("Wood"); //Set component name
        ComponentAmount.SetText("x10"); //Set component amount 

        ComponentText2.SetText("Tin Bar"); //Set component name
        ComponentAmount2.SetText("x10"); //Set component amount 

        selected = SelectedRecipe.TinPickaxe;
    }


    #endregion


    #region craftRecipeFunctions

    private void CraftTinHelmet()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var tinComponent = new CraftRecipeItem
            {
                displayName = "tin bar",
                quantity = 15
            };
            var components = new List<CraftRecipeItem>() {tinComponent};
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.TinHelmet, 1);
            }
        }
    }

    private void CraftTinChestplate()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var tinComponent = new CraftRecipeItem
            {
                displayName = "tin bar",
                quantity = 25
            };
            var components = new List<CraftRecipeItem>() { tinComponent};
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.TinChestplate, 1);
            }
        }
    }

    private void CraftTinSword()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var tinComponent = new CraftRecipeItem
            {
                displayName = "tin bar",
                quantity = 10
            };

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 10
            };

            var components = new List<CraftRecipeItem>() { tinComponent, woodComponent};
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.TinSword, 1);
            }
        }
    }

    private void CraftTinBow()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var tinComponent = new CraftRecipeItem
            {
                displayName = "tin bar",
                quantity = 10
            };

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 10
            };

            var components = new List<CraftRecipeItem>() { woodComponent, tinComponent};
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.ReinforcedBow, 1);
            }
        }
    }

    private void CraftTinPickaxe()
    {
        for (int i = 0; i < amountToCraft; i++)
        {
            var playerInventoryHolder = thisPlayer.GetComponent<PlayerInventoryHolder>();

            var tinComponent = new CraftRecipeItem
            {
                displayName = "tin bar",
                quantity = 10
            };

            var woodComponent = new CraftRecipeItem
            {
                displayName = "wood",
                quantity = 10
            };

            var components = new List<CraftRecipeItem>() { woodComponent, tinComponent};
            {
                playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.TinPickaxe, 1);
            }
        }
    }

    #endregion

    public void CraftItem()
    {
        if (selected == SelectedRecipe.TinHelmet)
        {
            CraftTinHelmet();
        }
        if (selected == SelectedRecipe.TinChestplate)
        {
            CraftTinChestplate();
        }
        if (selected == SelectedRecipe.TinSword)
        {
            CraftTinSword();
        }
        if (selected == SelectedRecipe.TinBow)
        {
            CraftTinBow();
        }
        if (selected == SelectedRecipe.TinPickaxe)
        {
            CraftTinPickaxe();
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
