using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class BlueberryBushLogic : MonoBehaviour//, IInteractable
{
    [SerializeField] private GameObject GrownBushObject;
    [SerializeField] private GameObject DeadBushObject;
    [SerializeField] private int growTime = 300; //in seconds
    [SerializeField] private GameObject blueberryItem; //Holds the blueberry
    private bool isActive; //Check to see if the bush is active or not.
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    
    private void Awake()
    {
        isActive = true; //The blue berry bush is active
        GrownBushObject.SetActive(true); //Bush will be set active
        DeadBushObject.SetActive(false); //Dead bush is deactive.
    }

    private void BlueberryBushHit() //Function for the blue berry bush getting hit.
    {
        if (isActive) //If the blueberry bush is on.
        {
            isActive = false; //Set active flag to false.
            GrownBushObject.SetActive(false);
            DeadBushObject.SetActive(true);
            StartCoroutine(GrowingPlant());
        }
    }

    private IEnumerator GrowingPlant()
    {
        yield return new WaitForSeconds(growTime); //Wait for how long grow time is set for (in seconds)     
        GrownBushObject.SetActive(true); //Turn bush back to grown
        DeadBushObject.SetActive(false); 
        isActive = true; //Set active flag back to true
        yield break; //End the couroutine
    }

}
