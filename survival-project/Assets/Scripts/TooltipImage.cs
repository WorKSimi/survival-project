using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class TooltipImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        //tooltipManager.EnableTooltip(inventorySlot.AssignedInventorySlot.itemData.DisplayName, inventorySlot.AssignedInventorySlot.itemData.Description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //tooltipManager.DisableTooltip();
    }
}
