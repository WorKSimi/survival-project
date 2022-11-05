using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]

public abstract class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    [SerializeField] protected InventorySystem inventorySystem;
    [SerializeField] protected int offset = 10;

    public int Offset => offset;

    public InventorySystem InventorySystem => inventorySystem;

    public static UnityAction<InventorySystem, int> OnDynamicInventoryDisplayRequested; // Inv system to Display, amount to offset display by

    protected virtual void Awake()
    {
        SaveLoad.OnLoadGame += LoadInventory;
        inventorySystem = new InventorySystem(inventorySize);
    }

    protected abstract void LoadInventory(SaveData saveData);
}

[System.Serializable]

public struct InventorySaveData
{
    public InventorySystem InvSystem;
    public Vector3 Position; //This might need to be vector2, game is 2D
    public Quaternion Rotation; //This might also need to change, game is 2D. It might also not be needed but unsure yet.

    public InventorySaveData(InventorySystem _invSystem, Vector3 _position, Quaternion _rotation)
    {
        InvSystem = _invSystem;
        Position = _position;
        Rotation = _rotation;
    }

    public InventorySaveData(InventorySystem _invSystem)
    {
        InvSystem = _invSystem;
        Position = Vector3.zero; //may need to be Vector2 for 2D, keep an eye out for bugs with this.
        Rotation = Quaternion.identity;
    }
}
