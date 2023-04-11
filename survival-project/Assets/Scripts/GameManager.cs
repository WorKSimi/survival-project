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

        protected void Start()
        {
            Instance = this;
        }
    }
}
