using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class StationTable : MonoBehaviour, IInteractable
{
    //[SerializeField] private GameObject craftingTableMenu;
    private Interactor CurrentInteractor;

    private void Awake()
    {
        //craftingTableMenu.SetActive(false);
    }

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        //craftingTableMenu.SetActive(true);
        interactSuccessful = true;
        CurrentInteractor = interactor;
        interactor.GetComponent<PlayerCraftingMenuManager>().StationMenuSelect();
    }

    public void Update()
    {
        //if (Keyboard.current.escapeKey.wasPressedThisFrame)
        //{
        //    CurrentInteractor.GetComponent<PlayerCraftingMenuManager>().DisableAllMenus();
        //}
    }

    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }
}
