using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CaveExit : MonoBehaviour, IInteractable
{
    private Vector3 entrancePosition;
    private int caveOffset = 600; //Make sure its same as in Map Generator.
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    private void Awake()
    {
        var x = this.transform.position.x - caveOffset;
        entrancePosition = new Vector3(x, this.transform.position.y, 0);
    }
    public void Interact(Interactor interactor, out bool interactSuccessful) //When you interact with cave exit
    {
        //TODO - ADD FADE IN AND OUT BLACK SCREEN EFFECT.

        interactSuccessful = true;
        interactor.transform.position = entrancePosition;
    }

    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }
}
