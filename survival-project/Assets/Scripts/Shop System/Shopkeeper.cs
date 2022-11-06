using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]

public class Shopkeeper : MonoBehaviour, IInteractable
{
    [SerializeField] private ShopItemList _shopItemsHeld;
    [SerializeField] private ShopSystem _shopSystem;

    private void Awake()
    {
        _shopSystem = new ShopSystem(_shopItemsHeld.Items.Count, _shopItemsHeld.MaxAllowedGold, _shopItemsHeld.BuyMarkUp,
                        _shopItemsHeld.SellMarkUp);

        foreach (var item in _shopItemsHeld.Items)
        {
            _shopSystem.AddToShop(item.ItemData, item.Amount);
        }
    }

    public UnityAction<IInteractable> OnInteractionComplete {get; set;}

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        throw new System.NotImplementedException();
    }

    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }
}
