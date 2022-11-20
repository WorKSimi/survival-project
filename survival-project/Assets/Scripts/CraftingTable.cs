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
        CurrentInteractor.GetComponent<PlayerInventoryHolder>();

        /*for (int i = 0; i < 50; i++)
        {
            if (PlayerInventoryHolder.InventorySystem.InventorySlot.ContainsItem(InventoryItemData wood))
            {
                Debug.Log("Contains Wood");
            }
        }*/

    }
}
