using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class InteractToBreak : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject itemToDrop; //Item that will be dropped
    [SerializeField] private int amountToDrop; //Amount of item to drop

    private Tilemap wallTilemap;
    private Vector3 objectPos;
    private Vector3Int objectGridPos;
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    private void Awake()
    {
        var tilemapObject = GameObject.FindWithTag("WallTilemap");
        wallTilemap = tilemapObject.GetComponent<Tilemap>();

        objectPos = this.transform.position;
        objectGridPos = wallTilemap.WorldToCell(this.transform.position); //Set the int object pos to where it in world
    }

    public void Interact(Interactor interactor, out bool interactSuccessful) //When you interact with object
    {
        interactSuccessful = true;

        if (itemToDrop != null) //If there is an item to drop
        {
            for (int i = 0; i < amountToDrop; i++) //loop based on how many you want to drop
            {
                Instantiate(itemToDrop, this.transform.position, Quaternion.identity);
            }
            wallTilemap.SetTile(objectGridPos, null); //Set object at this tilemap cell to null, avoiding errors
            Destroy(this.gameObject); //Destroy the game object
        }
        else //If item to drop is null
        {
            wallTilemap.SetTile(objectGridPos, null); //Set object at this tilemap cell to null, avoiding errors
            Destroy(this.gameObject); //Destroy the game object
        }
    }

    
    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }
}
