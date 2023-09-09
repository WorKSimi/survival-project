using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class BlueberryBushLogic : MonoBehaviour
{
    [SerializeField] private GameObject GrownBushObject;
    [SerializeField] private GameObject DeadBushObject;
    [SerializeField] private int growTime; //in seconds
    [SerializeField] private GameObject blueberryItem; //Holds the blueberry
    private Vector3 objectLocation;
    private bool isActive; //Check to see if the bush is active or not.
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    
    private void Awake()
    {
        isActive = true; //The blue berry bush is active
        GrownBushObject.SetActive(true); //Bush will be set active
        DeadBushObject.SetActive(false); //Dead bush is deactive.
    }

    public void DeactivateBlueberryBush() //This function is called when a player hits the blueberry bush while its active
    {
        objectLocation = this.transform.position;
        isActive = false;  //set active flag to false
        GrownBushObject.SetActive(false); //Turn off grown object on host
        DeadBushObject.SetActive(true); //Turn on dead object on host
        StartCoroutine(GrowingTimer()); //Start the growing timer
        DeactiveBlueberryBushClientRpc(objectLocation); //Deactivate on all clients

        var go = Instantiate(blueberryItem, objectLocation, Quaternion.identity); //Instantiate blueberry
        go.GetComponent<NetworkObject>().Spawn(); //Spawn in on network
    }

    [ClientRpc] //Fired by Server, Execute on Client
    private void DeactiveBlueberryBushClientRpc(Vector3 objPosition)
    {
        var vec2 = (Vector2)objPosition; //Turn position to vector 2     
        RaycastHit2D hit; //Variable for racyast
        hit = Physics2D.Raycast(vec2, Vector2.up, 0.1f); //Set hit to raycast
        if (hit == false) return; //Cancel if nothing is hit.
        if (hit.transform.CompareTag("BlueberryBush")) //If blueberry bush is hit
        {
            GrownBushObject.SetActive(false); //Turn off grown object on host
            DeadBushObject.SetActive(true); //Turn on dead object on host
        }
    }

    [ServerRpc] //Fired by client, Execute on Server
    public void ClientHitBlueberryBushServerRpc()
    {
        DeactivateBlueberryBush();
    }

    private IEnumerator GrowingTimer()
    {
        yield return new WaitForSeconds(growTime); //Wait for how long grow time is set for (in seconds)     
        GrownBushObject.SetActive(true); //Turn bush back to grown
        DeadBushObject.SetActive(false); 
        isActive = true; //Set active flag back to true
        yield break; //End the couroutine
    }

}
