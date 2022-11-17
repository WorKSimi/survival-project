using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CraftingTable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject craftingTableMenu;

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
}
