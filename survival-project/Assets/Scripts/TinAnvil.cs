using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TinAnvil : MonoBehaviour, IInteractable
{
    private Interactor CurrentInteractor;
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        interactSuccessful = true;
        CurrentInteractor = interactor;
        interactor.GetComponent<PlayerCraftingMenuManager>().AnvilMenuSelect();
    }

    public void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CurrentInteractor.GetComponent<PlayerCraftingMenuManager>().DisableAllMenus();
        }
    }

    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }
}
