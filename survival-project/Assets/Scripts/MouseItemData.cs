using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.Netcode;

public class MouseItemData : NetworkBehaviour
{
    public Image ItemSprite;
    public TextMeshProUGUI ItemCount;
    public InventorySlot AssignedInventorySlot;

    private Transform playerTransform;
    private Transform interactionPoint;    
   
    private void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemSprite.preserveAspect = true;
        ItemCount.text = "";

        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        if (playerTransform == null) Debug.Log("Player not found!");

        interactionPoint = GameObject.FindWithTag("InteractionPoint").GetComponent<Transform>();
    }

    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        AssignedInventorySlot.AssignItem(invSlot);
        UpdateMouseSlot();
    }

    public void UpdateMouseSlot()
    {
        ItemSprite.sprite = AssignedInventorySlot.ItemData.Icon;
        ItemCount.text = AssignedInventorySlot.StackSize.ToString();
        ItemSprite.color = Color.white;
    }

    private void Update()
    {
        // TODO: Add controller support.
        Vector3 dropOffset = new Vector3(0, 3, 0f);
        Vector3 dropLocation = playerTransform.position - dropOffset;

        if (AssignedInventorySlot.ItemData != null) // If has an item, follow the mouse position.
        {
            transform.position = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject()) 
            {
                if (AssignedInventorySlot.ItemData.ItemPrefab != null)
                {
                    if (IsHost)
                    {
                        GameObject go = Instantiate(AssignedInventorySlot.ItemData.ItemPrefab, dropLocation, Quaternion.identity); //Drop item
                        go.GetComponent<NetworkObject>().Spawn();

                        if (AssignedInventorySlot.StackSize > 1)
                        {
                            AssignedInventorySlot.AddToStack(-1);
                            UpdateMouseSlot();
                        }

                        else
                        {
                            ClearSlot();
                        }
                    }
                    else if (IsClient)
                    {
                        DropItemServerRpc(dropLocation); //Spawn item on network

                        if (AssignedInventorySlot.StackSize > 1)
                        {
                            AssignedInventorySlot.AddToStack(-1);
                            UpdateMouseSlot();
                        }

                        else
                        {
                            ClearSlot();
                        }
                    }
                }
            } 
        }
    }

    public void ClearSlot()
    {
        AssignedInventorySlot.ClearSlot();
        ItemCount.text = "";
        ItemSprite.color = Color.clear;
        ItemSprite.sprite = null;
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DropItemServerRpc( Vector3 dropLocation, ServerRpcParams serverRpcParams = default)
    {
        GameObject go = Instantiate(AssignedInventorySlot.ItemData.ItemPrefab, dropLocation, Quaternion.identity); //Drop item
        go.GetComponent<NetworkObject>().Spawn();
    }
}

