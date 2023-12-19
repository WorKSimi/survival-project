using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
public class HotbarDisplay : StaticInventoryDisplay
{
    public GameObject player;
    public UseItemManager useItemManager;
    [SerializeField] private TMP_Text heldItemText; 
    [SerializeField] private GameObject weaponAnchor; //Weapon anchor on player
    [SerializeField] private GameObject weapon; //Weapon object on player
    [SerializeField] private GameObject heldItem; //Held item object on player
    [SerializeField] private SpriteRenderer heldGunSpriteRenderer; //Held gun item sprite renderer on object.
    [SerializeField] private SpriteRenderer heldItemSpriteRenderer; //Held item sprite renderer on object

    [SerializeField] private GameObject swordHandR; //Sprite for the hand holding sword facing right
    [SerializeField] private GameObject swordHandL; //Sprite for the hand holding sword facing left

    private SpriteRenderer swordSpriteRenderer; //Held Sword sprite renderer on object

    private float chargeTime;
    private float maxChargeTime = 2f; //Max charge time is 2 seconds
    private float chargeModifier;

    private int _maxIndexSize = 9;
    private int _currentIndex = 0;

    private PlayerControls _playerControls;

    private void Awake()
    {
        _playerControls = new PlayerControls();

        swordHandR.SetActive(false); //Set sword hands to false
        swordHandL.SetActive(false); //Set sword hands to false

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

        ChangeHeldItemText();
        AreYouHoldingPickOrBlock();
        DisplayHeldWeaponSprite();
        DisplayHeldBowSprite();

        ChargeBow();
        FireBow();
    }

    [SerializeField] private GameObject attackChargeBarObject;

    public void ChargeBow()
    {
        if (slots[_currentIndex].AssignedInventorySlot.ItemData == null)
        {
            return;
        }

        if (slots[_currentIndex].AssignedInventorySlot.ItemData.ItemType == "Bow")
        {
            attackChargeBarObject.SetActive(true);
        }
        else
        {
            attackChargeBarObject.SetActive(false);
        }

        if (Input.GetMouseButton(0)) //If holding mouse down
        {     
            chargeTime += Time.deltaTime; //ChargeTime goes up

            float fillPercent = chargeTime / maxChargeTime;
            attackChargeBarObject.GetComponent<Slider>().value = fillPercent * 100;

            if (chargeTime >= maxChargeTime)
            {
                chargeModifier = 1 + maxChargeTime; //If at max (2 seconds) charge multiplier - 1 + 2.0                
            }
            else
            {
                chargeModifier = 1 + chargeTime; //If below max, charge multiplier is equal to 1 + time spent charging              
            }           
        }
        else chargeTime = 0;
    }

    public void FireBow()
    {
        if (slots[_currentIndex].AssignedInventorySlot.ItemData != null) //If held item isnt nothing
        {
            if (slots[_currentIndex].AssignedInventorySlot.ItemData.ItemType == "Bow") //if held item type is bow
            {
                if (Input.GetMouseButtonUp(0)) //If Let go of left click
                {
                    InventoryItemData itemData = slots[_currentIndex].AssignedInventorySlot.ItemData;
                    player.GetComponent<UseItemManager>().UseBow(itemData.itemDamage, itemData.projectilePrefab, itemData.projectileSpeed, itemData.projectileLifetime, chargeModifier, itemData.attackRate);
                    attackChargeBarObject.GetComponent<Slider>().value = 0; //Set charge bar to 0 after firing.
                }
            }
        }
    }

    private void ChangeHeldItemText()
    {
        if (slots[_currentIndex].AssignedInventorySlot.ItemData != null)
        {
            heldItemText.SetText(slots[_currentIndex].AssignedInventorySlot.ItemData.DisplayName);
        }
        else heldItemText.SetText("");
    }    

    private void UseItem(InputAction.CallbackContext obj)
    {
        if (slots[_currentIndex].AssignedInventorySlot.ItemData != null) //Checks if current slot data is not null
        {
            InventoryItemData itemData = slots[_currentIndex].AssignedInventorySlot.ItemData;
            switch (itemData.ItemType) //Switch statement that runs different code from use item manager depending on item type.
            {
                //Note, later just make this call a script on held object that does stuff unique to it. (Unique code for pick / bow ect instead of the 1 megaScript)
                case "Axe":
                player.GetComponent<UseItemManager>().UseAxe(itemData.itemDamage, itemData.attackRate);
                break;

                case "Block":
                    if (player.GetComponent<UseItemManager>().IsInRange()) //Mouse is in valid range
                    {
                        if (player.GetComponent<UseItemManager>().TileFound() == false) //No tile found at this spot
                        {
                            player.GetComponent<UseItemManager>().PlaceBlock(itemData.BlockPrefab, itemData.ID);
                            slots[_currentIndex].AssignedInventorySlot.RemoveFromStack(1);
                            RefreshStaticDisplay();
                        }
                    }
                break;

                case "Floor":
                    if (player.GetComponent<UseItemManager>().IsInRange()) //Mouse is in valid range
                    {
                        if (player.GetComponent<UseItemManager>().TileFound() == false && player.GetComponent<UseItemManager>().FloorFound() == false) //If no wall
                        {
                            player.GetComponent<UseItemManager>().PlaceBlock(itemData.BlockPrefab, itemData.ID);
                            slots[_currentIndex].AssignedInventorySlot.RemoveFromStack(1);
                            RefreshStaticDisplay();
                        }
                    }
                break;

                case "Pick":
                player.GetComponent<UseItemManager>().UsePick(itemData.itemDamage, itemData.attackRate);
                break;

                case "Sword":
                player.GetComponent<UseItemManager>().UseSword(itemData.itemDamage, itemData.attackRate);
                break;

                case "Bow":
                    //player.GetComponent<UseItemManager>().UseBow(itemData.itemDamage, itemData.projectilePrefab, itemData.projectileSpeed, itemData.projectileLifetime);
                break;

                case "Rock":
                player.GetComponent<UseItemManager>().UseRock(itemData.itemDamage, itemData.attackRate);
                break;

                case "Food":
                    if (player.GetComponent<UseItemManager>().IsHealthFull() == false) //If health isnt full
                    {
                        if (player.GetComponent<UseItemManager>().healCooldownComplete == true) //If cooldown is done
                        {
                            player.GetComponent<UseItemManager>().UseFood(itemData.HealthHealed);
                            slots[_currentIndex].AssignedInventorySlot.RemoveFromStack(1);
                            RefreshStaticDisplay();
                        }
                    }
                break;

                case "Watercan":
                    player.GetComponent<UseItemManager>().UseWaterCan();                    
                    break;

                case "Hoe":
                    player.GetComponent<UseItemManager>().UseHoe();
                    break;

                case "Chest":         
                    if (player.GetComponent<UseItemManager>().TileFound() == false) //If no Wall
                    {
                        player.GetComponent<UseItemManager>().PlaceChest(itemData.BlockPrefab, itemData.ID);
                        slots[_currentIndex].AssignedInventorySlot.RemoveFromStack(1);
                        RefreshStaticDisplay();
                    }
                    break;

                case "Warhorn":
                    player.GetComponent<UseItemManager>().UseSlimyWarhorn();
                break;

                case "WeirdPotion":
                    player.GetComponent<UseItemManager>().UseWeirdPotion();
                   break;

                case "Handgun":
                    player.GetComponent<UseItemManager>().UseHandgun(itemData.itemDamage, itemData.projectilePrefab, itemData.projectileSpeed, itemData.projectileLifetime, itemData.attackRate);
                    break;

                //case "Seed":
                //    var seedTile = slots[_currentIndex].AssignedInventorySlot.ItemData.ItemTile;
                //    if (player.GetComponent<UseItemManager>().MouseOverCropland() == true) //if mouse over farmland
                //    {
                //        if (player.GetComponent<UseItemManager>().TileFound() == false)
                //        {
                //            player.GetComponent<UseItemManager>().UseSeed(seedTile); //Use Seed
                //            slots[_currentIndex].AssignedInventorySlot.RemoveFromStack(1); //Remove 1 seed from stack
                //            RefreshStaticDisplay();
                //        }
                //    }
                //    break;

                default:
                Debug.Log($"Default Case");
                break;
            }
        }   
    }

    private void DisplayHeldWeaponSprite()
    {    
        if (slots[_currentIndex].AssignedInventorySlot.ItemData != null) //Held item isnt null
        {
            var heldItemType = slots[_currentIndex].AssignedInventorySlot.ItemData.ItemType; //Store so easier to reference

            if (heldItemType == "Sword" || heldItemType == "Pick" || heldItemType == "Rock") //If held item is a valid type
            {
                swordSpriteRenderer.sprite = slots[_currentIndex].AssignedInventorySlot.ItemData.Icon; //Character hold that item
                swordHandR.SetActive(true);
                swordHandL.SetActive(false);
                if (useItemManager.facingLeft == true)
                {
                    swordHandR.SetActive(false);
                    swordHandL.SetActive(true);
                }
            }
            else
            {
                swordSpriteRenderer.sprite = null; //If you arent holding one of those types, display no sprite
                swordHandR.SetActive(false); //Set sword hands to false
                swordHandL.SetActive(false); //Set sword hands to false
            }
        }
        else if (slots[_currentIndex].AssignedInventorySlot.ItemData == null) //Held item IS null
        {
            swordSpriteRenderer.sprite = null; //Display no sprite
            swordHandR.SetActive(false); //Set sword hands to false
            swordHandL.SetActive(false); //Set sword hands to false
        }
    }

    private void DisplayHeldBowSprite()
    {
        if (slots[_currentIndex].AssignedInventorySlot.ItemData != null)
        {
            if (slots[_currentIndex].AssignedInventorySlot.ItemData.ItemType == "Bow")
            {
                heldItemSpriteRenderer.sprite = slots[_currentIndex].AssignedInventorySlot.ItemData.Icon;
            }
            else if (slots[_currentIndex].AssignedInventorySlot.ItemData.ItemType == "Handgun")
            {
                heldGunSpriteRenderer.sprite = slots[_currentIndex].AssignedInventorySlot.ItemData.Icon;
            }
            else
            {
                heldItemSpriteRenderer.sprite = null;
                heldGunSpriteRenderer.sprite = null;
            }
        }
        else
        {
            heldItemSpriteRenderer.sprite = null;
            heldGunSpriteRenderer.sprite = null;
        }
    }

    private void AreYouHoldingPickOrBlock()
    {
        if (slots[_currentIndex].AssignedInventorySlot.ItemData != null) //If the current held item is not null
        {
            if (slots[_currentIndex].AssignedInventorySlot.ItemData.ItemType == "Block") //If held item type is block
            {
                useItemManager.HoldingPickOrBlock = true; //Set to true
            }
            else if (slots[_currentIndex].AssignedInventorySlot.ItemData.ItemType == "Pick") //If held item type is pick
            {
                useItemManager.HoldingPickOrBlock = true; //Set to true
            }
            else useItemManager.HoldingPickOrBlock = false; //Else set to false
        }
        else useItemManager.HoldingPickOrBlock = false; //If item data is null, set to false
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
