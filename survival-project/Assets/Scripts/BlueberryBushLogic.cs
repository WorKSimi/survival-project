using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class BlueberryBushLogic : NetworkBehaviour, IInteractable
{
    [SerializeField] private GameObject GrownBushObject;
    [SerializeField] private GameObject DeadBushObject;
    [SerializeField] private int growTime = 300; //in seconds
    [SerializeField] private GameObject blueberryItem; //Holds the blueberry
    private bool canInteract;
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }


    private void Awake()
    {
        canInteract = true;
        GrownBushObject.SetActive(true);
        GameObject go = this.gameObject;
        go.GetComponent<NetworkObject>().Spawn(); //On awake, spawn this object on network
    }

    public void Interact(Interactor interactor, out bool interactSuccessful) //When you interact with bush
    {
        interactSuccessful = true;
        if (IsHost)
        {
            if (canInteract == true)
            {
                canInteract = false;
                GrownBushObject.SetActive(false);
                DeadBushObject.SetActive(true);
                BushDeactiveSyncClientRpc(); //Syncs bushes on clients
                var go = Instantiate(blueberryItem, this.transform.position, Quaternion.identity); //Instantiate blueberry item
                go.GetComponent<NetworkObject>().Spawn();
                StartCoroutine(GrowingPlant());
            }
            else return;
        }
        else if (IsClient)
        {
            BlueberryInteractServerRpc();
        }
    }

    private IEnumerator GrowingPlant()
    {
        yield return new WaitForSeconds(growTime); //Wait for how long grow time is set for (in seconds)     
        GrownBushObject.SetActive(true); //Turn bush back to grown
        DeadBushObject.SetActive(false);
        BushActiveSyncClientRpc();
        canInteract = true; //Can interact again
        yield break; //End the couroutine
    }
    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }

    [ServerRpc(RequireOwnership = false)] //Fired by client, executed on server
    public void BlueberryInteractServerRpc()
    {
        if (canInteract == true)
        {
            canInteract = false;
            GrownBushObject.SetActive(false);
            DeadBushObject.SetActive(true);
            BushDeactiveSyncClientRpc(); //Syncs bushes on clients
            var go = Instantiate(blueberryItem, this.transform.position, Quaternion.identity); //Instantiate blueberry item
            go.GetComponent<NetworkObject>().Spawn();
            StartCoroutine(GrowingPlant());
        }
    }

    [ClientRpc] //Fired by server, executed on client
    public void BushActiveSyncClientRpc()
    {
        GrownBushObject.SetActive(true);
        DeadBushObject.SetActive(false);
    }

    [ClientRpc] //Fired by server, executed on client
    public void BushDeactiveSyncClientRpc()
    {
        GrownBushObject.SetActive(false);
        DeadBushObject.SetActive(true);
    }
}
