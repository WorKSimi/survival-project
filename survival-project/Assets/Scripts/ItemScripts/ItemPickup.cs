using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(UniqueID))]

public class ItemPickup : NetworkBehaviour
{
    public float PickUpRadius = 1f;
    public InventoryItemData ItemData;

    [SerializeField] private float _rotationSpeed = 20f; //MAKES ITEMS ROTATE CHANGE LATER

    private CircleCollider2D myCollider;

    [SerializeField] private ItemPickUpSaveData itemSaveData;
    private string id;

    private void Awake()
    {
        //id = GetComponent<UniqueID>().ID;
        SaveLoad.OnLoadGame += LoadGame;
        itemSaveData = new ItemPickUpSaveData(ItemData, transform.position, transform.rotation);

        myCollider = GetComponent<CircleCollider2D>();
        myCollider.isTrigger = true;
        myCollider.radius = PickUpRadius;
    }

    private void Start()
    {
        //SaveGameManager.data.activeItems.Add(id, itemSaveData);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime); //MAKES ITEMS ROTATE, CHANGE LATER
    }

    private void LoadGame(SaveData data)
    {
        //    if (data.collectedItems.Contains(id)) Destroy(this.gameObject);
    }

    //private void OnDestroy()
    //{
    //    if (SaveGameManager.data.activeItems.ContainsKey(id)) SaveGameManager.data.activeItems.Remove(id);
    //    SaveLoad.OnLoadGame -= LoadGame;
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        var inventory = other.transform.GetComponent<PlayerInventoryHolder>();
        Debug.Log("Collision Happened");
        if (!inventory) return;

        if (inventory.AddToInventory(ItemData, 1))
        {
            Debug.Log("Picking Up Item");
            //If host or server, destroy normally
            if (IsHost)
            {
                Debug.Log("Picking up Item HOST");
                SaveGameManager.data.collectedItems.Add(id);
                NetworkObject networkObject = this.gameObject.GetComponent<NetworkObject>();
                networkObject.Despawn();

            }
            else if (IsClient) //If your a client, send message to server to destroy it instead
            {
                Debug.Log("Picking up Item CLIENT");
                SaveGameManager.data.collectedItems.Add(id);
                DestroyObjectServerRpc();
            }
            else //FailSafe
            {
                NetworkObject networkObject = this.gameObject.GetComponent<NetworkObject>();
                networkObject.Despawn();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyObjectServerRpc()
    {
        NetworkObject networkObject = this.gameObject.GetComponent<NetworkObject>();
        networkObject.Despawn();
    }
    
}

[System.Serializable]
public struct ItemPickUpSaveData
{
    public InventoryItemData ItemData;
    public Vector3 Position;
    public Quaternion Rotation;

    public ItemPickUpSaveData(InventoryItemData _data, Vector3 _position, Quaternion _rotation)
    {
        ItemData = _data;
        Position = _position;
        Rotation = _rotation;
    }
}


