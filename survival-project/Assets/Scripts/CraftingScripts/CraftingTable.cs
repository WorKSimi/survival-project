using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CraftingTable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject craftingTableMenu;
    private Interactor CurrentInteractor;

    private void Awake()
    {
        craftingTableMenu.SetActive(false);
    }

    public UnityAction<IInteractable> OnInteractionComplete {get; set;}

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        craftingTableMenu.SetActive(true);
        interactSuccessful = true;
        CurrentInteractor = interactor;
    }

    public void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            craftingTableMenu.SetActive(false);
        }
    }

    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }

    public void CraftFlintAxe()
    {
        //CurrentInteractor = interactor;
        Debug.Log("Trying to Craft Flint Axe");
        var playerInventoryHolder = CurrentInteractor.GetComponent<PlayerInventoryHolder>();

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

        var components = new List<CraftRecipeItem>() { woodComponent, flintComponent};
        {
            playerInventoryHolder.inventorySystem.CraftItem(components, GameManager.Instance.FlintAxe, 1);
            Debug.Log("Crafted Flint Axe");
        }
    }
}