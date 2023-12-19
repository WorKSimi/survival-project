using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        //Anywhere Crafting
        [Header("Anywhere")]
        public InventoryItemData CraftingTable;
        public InventoryItemData Torch;

        //Crafting Table
        [Header("Crafting Table")]
        public InventoryItemData WoodBow;
        public InventoryItemData Campfire;
        public InventoryItemData WoodClub;
        public InventoryItemData FlintPick;
        public InventoryItemData WoodHelmet;
        public InventoryItemData WoodChestplate;
        public InventoryItemData DirtWall;
        public InventoryItemData DirtGround;
        public InventoryItemData StationTable;
        public InventoryItemData WoodArrow;
        public InventoryItemData FlintArrow;
        public InventoryItemData PopGun;

        //Station Table
        [Header("Station Table")]
        public InventoryItemData Furnace;
        public InventoryItemData Anvil;
        public InventoryItemData CarpenterTable;
        public InventoryItemData StoneMasonTable;
        public InventoryItemData CookingPot;

        //Other
        [Header("Other")]
        public InventoryItemData WoodWall;
        public InventoryItemData FlintAxe;
        public InventoryItemData TinBar;
        public InventoryItemData TinSword;
        public InventoryItemData TinHelmet;
        public InventoryItemData TinChestplate;
        public InventoryItemData TinPickaxe;
        public InventoryItemData WoodFloor;
        public InventoryItemData Chest;
        public InventoryItemData IronAnvil;
        public InventoryItemData IronBar;      

        protected void Start()
        {
            Instance = this;
        }
    }
}
