using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDatabase : MonoBehaviour
{
    public InventoryItemData woodWallData; //ID - 1
    public InventoryItemData torchWallData;//ID - 2
    public InventoryItemData craftingTable; //ID - 3
    public InventoryItemData furnace; //ID - 4
    public InventoryItemData tinAnvil; //ID - 5
    public InventoryItemData woodFloor;
    

    public GameObject TileReturner(int tileID)
    {
        //TURN THIS INTO SWITCH STATEMENT LATER PLEASE
        if (tileID == woodWallData.ID)
        {
            return woodWallData.BlockPrefab;
        }
        else if (tileID == torchWallData.ID)
        {
            return torchWallData.BlockPrefab;
        }
        else if (tileID == craftingTable.ID)
        {
            return craftingTable.BlockPrefab;
        }
        else if (tileID == furnace.ID)
        {
            return furnace.BlockPrefab;
        }
        else if (tileID == tinAnvil.ID)
        {
            return tinAnvil.BlockPrefab;
        }
        else if (tileID == woodFloor.ID)
        {
            return woodFloor.BlockPrefab;
        }
        else return null;       
    }
}
