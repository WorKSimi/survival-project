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

        public InventoryItemData FlintAxe;
        public InventoryItemData CraftingTable;
        public InventoryItemData Campfire;
        public InventoryItemData WoodClub;
        public InventoryItemData FlintPick;
        public InventoryItemData Torch;
        public InventoryItemData WoodBow;
        public InventoryItemData WoodWall;
        public InventoryItemData WoodHelmet;
        public InventoryItemData WoodChestplate;
        public InventoryItemData TinBar;
        public InventoryItemData Furnace;
        public InventoryItemData TinAnvil;
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
