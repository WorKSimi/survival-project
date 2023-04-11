using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InventorySlot_UI inventorySlot;
    public Transform thisPlayer;
    private TooltipManager tooltipManager;

    private void Start()
    {
        thisPlayer = this.gameObject.transform.root;
        tooltipManager = thisPlayer.GetComponent<TooltipManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData == null) return;
        if (inventorySlot.AssignedInventorySlot.itemData == null) return;
        tooltipManager.EnableTooltip(inventorySlot.AssignedInventorySlot.itemData.ItemType,
            inventorySlot.AssignedInventorySlot.itemData.DisplayName,
            inventorySlot.AssignedInventorySlot.itemData.Description,
            inventorySlot.AssignedInventorySlot.itemData.itemDamage.ToString(),
            inventorySlot.AssignedInventorySlot.itemData.DefenseValue.ToString(),
            inventorySlot.AssignedInventorySlot.itemData.HealthHealed.ToString(),
            inventorySlot.AssignedInventorySlot.itemData.attackRate.ToString(),
            inventorySlot.AssignedInventorySlot.itemData.BlockHealth.ToString());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipManager.DisableTooltip();
    }
}
