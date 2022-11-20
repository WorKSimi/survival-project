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

        if(PlayerInventoryHolder ContainsItem(wood, out List<InventorySlot> invSlot)
        && PlayerInventoryHolder ContainsItem(flint, out List<InventorySlot> invSlot))
        {
            foreach(var slot in invSlot)
            {
                ClearSlot();
            }
        }


    }
}
