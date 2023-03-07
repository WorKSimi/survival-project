using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class BlueberryBushLogic : NetworkBehaviour, IInteractable
{
    private State state;
    private int growTime = 300; //in seconds
    private bool canInteract;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject blueberryItem; //Holds the blueberry
    [SerializeField] private Sprite grownSprite; //Holds the sprite for grown plant
    [SerializeField] private Sprite emptySprite; //Holds the sprite for empty plant

    private void Awake()
    {
        state = State.Grown;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (state == State.Grown)
        {
            spriteRenderer.sprite = grownSprite;
            canInteract = true;
        }
        else if (state == State.Empty)
        {
            //Bush is empty
        }

    }
    private enum State //Store the states of the berry bush
    {
        Empty, //The bush is empty
        Grown, //the bush is grown
    }

    public void Interact(Interactor interactor, out bool interactSuccessful) //When you interact with bush
    {
        interactSuccessful = true;
        if (canInteract == true)
        {
            canInteract = false;
            spriteRenderer.sprite = emptySprite;
            var go = Instantiate(blueberryItem, this.transform.position, Quaternion.identity); //Instantiate blueberry item
            go.GetComponent<NetworkObject>().Spawn();
            state = State.Empty; //Set the state to empty
            StartCoroutine(GrowingPlant());
        }
        else return;
    }

    private IEnumerator GrowingPlant()
    {
        yield return new WaitForSeconds(growTime); //Wait for how long grow time is set for (in seconds)     
        state = State.Grown; //Set state to grown plant
        yield break; //End the couroutine
    }
    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }

    [ServerRpc(RequireOwnership = false)]
    public void BlueberryLogicServerRpc()
    {
        canInteract = false;
        spriteRenderer.sprite = emptySprite;
        var go = Instantiate(blueberryItem, this.transform.position, Quaternion.identity); //Instantiate blueberry item
        go.GetComponent<NetworkObject>().Spawn();
        state = State.Empty; //Set the state to empty
        StartCoroutine(GrowingPlant());
    }
}
