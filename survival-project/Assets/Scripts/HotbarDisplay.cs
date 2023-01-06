using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarDisplay : StaticInventoryDisplay
{
    public GameObject player;
    [SerializeField] private GameObject weaponAnchor; //Weapon anchor on player
    [SerializeField] private GameObject weapon; //Weapon object on player
    [SerializeField] private GameObject heldItem; //Held item object on player
    private SpriteRenderer heldItemSpriteRenderer; //Held item sprite renderer on object
    private SpriteRenderer swordSpriteRenderer; //Held Sword sprite renderer on object

    private int _maxIndexSize = 9;
    private int _currentIndex = 0;

    private PlayerControls _playerControls;

    private void Awake()
    {
        _playerControls = new PlayerControls();

        swordSpriteRenderer = weapon.GetComponent<SpriteRenderer>();
        heldItemSpriteRenderer = heldItem.GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();

        _currentIndex = 0;
        _maxIndexSize = slots.Length - 1;

        slots[_currentIndex].ToggleHighlight();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _playerControls.Enable();

        _playerControls.Player.Hotbar1.performed += Hotbar1;
        _playerControls.Player.Hotbar2.performed += Hotbar2;
        _playerControls.Player.Hotbar3.performed += Hotbar3;
        _playerControls.Player.Hotbar4.performed += Hotbar4;
        _playerControls.Player.Hotbar5.performed += Hotbar5;
        _playerControls.Player.Hotbar6.performed += Hotbar6;
        _playerControls.Player.Hotbar7.performed += Hotbar7;
        _playerControls.Player.Hotbar8.performed += Hotbar8;
        _playerControls.Player.Hotbar9.performed += Hotbar9;
        _playerControls.Player.Hotbar10.performed += Hotbar10;
        _playerControls.Player.UseItem.performed += UseItem;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _playerControls.Disable();

        _playerControls.Player.Hotbar1.performed -= Hotbar1;
        _playerControls.Player.Hotbar2.performed -= Hotbar2;
        _playerControls.Player.Hotbar3.performed -= Hotbar3;
        _playerControls.Player.Hotbar4.performed -= Hotbar4;
        _playerControls.Player.Hotbar5.performed -= Hotbar5;
        _playerControls.Player.Hotbar6.performed -= Hotbar6;
        _playerControls.Player.Hotbar7.performed -= Hotbar7;
        _playerControls.Player.Hotbar8.performed -= Hotbar8;
        _playerControls.Player.Hotbar9.performed -= Hotbar9;
        _playerControls.Player.Hotbar10.performed -= Hotbar10;
        _playerControls.Player.UseItem.performed -= UseItem;
    }

    private void Hotbar1(InputAction.CallbackContext obj)
    {
        SetIndex(0);
    }

    private void Hotbar2(InputAction.CallbackContext obj)
    {
        SetIndex(1);
    }
    
    private void Hotbar3(InputAction.CallbackContext obj)
    {
        SetIndex(2);
    }
    
    private void Hotbar4(InputAction.CallbackContext obj)
    {
        SetIndex(3);
    }
    
    private void Hotbar5(InputAction.CallbackContext obj)
    {
        SetIndex(4);
    }
    
    private void Hotbar6(InputAction.CallbackContext obj)
    {
        SetIndex(5);
    }
    
    private void Hotbar7(InputAction.CallbackContext obj)
    {
        SetIndex(6);
    }
    
    private void Hotbar8(InputAction.CallbackContext obj)
    {
        SetIndex(7);
    }
    
    private void Hotbar9(InputAction.CallbackContext obj)
    {
        SetIndex(8);
    }

    private void Hotbar10(InputAction.CallbackContext obj)
    {
        SetIndex(9);
    }

    private void Update()
    {
        if (_playerControls.Player.MouseWheel.ReadValue<float>() > 0.1f) ChangeIndex(-1); //Changes selected hotbar slot
        if (_playerControls.Player.MouseWheel.ReadValue<float>() < -0.1f) ChangeIndex(1); //Changes selected hotbar slot the other way

        if (slots[_currentIndex].AssignedInventorySlot.ItemData != null)
        {
            if (slots[_currentIndex].AssignedInventorySlot.ItemData.ItemType == "Bow") //Checks if held item is bow
            {
                heldItemSpriteRenderer.sprite = slots[_currentIndex].AssignedInventorySlot.ItemData.Icon;
            }

            if (slots[_currentIndex].AssignedInventorySlot.ItemData.ItemType != "Bow")
            {
                heldItemSpriteRenderer.sprite = null;
            }

            if (slots[_currentIndex].AssignedInventorySlot.ItemData.ItemType == "Sword") //Checks if the currently held item is a sword type
            {
                swordSpriteRenderer.sprite = slots[_currentIndex].AssignedInventorySlot.ItemData.Icon; //Sets the visual for the sword weapon to the held item.
            }

            if (slots[_currentIndex].AssignedInventorySlot.ItemData.ItemType != "Sword")
            {
                swordSpriteRenderer.sprite = null;
            }
        }

        else if (slots[_currentIndex].AssignedInventorySlot.ItemData == null)
        {
            swordSpriteRenderer.sprite = null;
            heldItemSpriteRenderer.sprite = null;
        }
    }

    private void UseItem(InputAction.CallbackContext obj)
    {
        if (slots[_currentIndex].AssignedInventorySlot.ItemData != null) //Checks if current slot data is not null
        {
            InventoryItemData itemData = slots[_currentIndex].AssignedInventorySlot.ItemData;
            switch (itemData.ItemType) //Switch statement that runs different code from use item manager depending on item type.
            {
                case "Axe":
                player.GetComponent<UseItemManager>().UseAxe(itemData.itemDamage);
                break;

                case "Block":
                if (player.GetComponent<UseItemManager>().IsInRange()) 
                {
                     if (player.GetComponent<UseItemManager>().TileFound() == false)
                     {
                         player.GetComponent<UseItemManager>().PlaceBlock(itemData.ItemTile);
                         slots[_currentIndex].AssignedInventorySlot.RemoveFromStack(1);
                         RefreshStaticDisplay();
                     }
                }
                break;

                case "Pick":
                player.GetComponent<UseItemManager>().UsePick(itemData.itemDamage);
                break;

                case "Sword":
                player.GetComponent<UseItemManager>().UseSword(itemData.itemDamage);
                break;

                case "Bow":
                player.GetComponent<UseItemManager>().UseBow(itemData.itemDamage, itemData.projectilePrefab);
                break;

                case "Rock":
                player.GetComponent<UseItemManager>().UseRock(itemData.itemDamage);
                break;

                default:
                Debug.Log($"Default Case");
                break;
            }
        }   
    }

    private void ChangeIndex(int direction)
    {
        slots[_currentIndex].ToggleHighlight();
        _currentIndex += direction;

        if (_currentIndex > _maxIndexSize) _currentIndex = 0;
        if (_currentIndex < 0) _currentIndex = _maxIndexSize;

        slots[_currentIndex].ToggleHighlight();
    }

    private void SetIndex(int newIndex)
    {
        slots[_currentIndex].ToggleHighlight();
        if (newIndex < 0) _currentIndex = 0;
        if (newIndex > _maxIndexSize) _currentIndex = _maxIndexSize;

        _currentIndex = newIndex;
        slots[_currentIndex].ToggleHighlight();
    }
}
