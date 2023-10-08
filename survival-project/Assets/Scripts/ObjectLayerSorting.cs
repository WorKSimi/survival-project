using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLayerSorting : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>(); //Player sprite renderer

        if (this.gameObject.CompareTag("Floor")) //if the object is a floor
        {
            if (this.gameObject.transform.position.x >= 600) //If player is in cave (not in surface)
            {
                spriteRenderer.sortingLayerID = SortingLayer.NameToID("Underground-Ground"); //Set  sorting layer to underground
            }
            else //If player on surface
            {
                spriteRenderer.sortingLayerID = SortingLayer.NameToID("Ground"); //Set  sorting layer to default.
            }
        }
        else if (this.gameObject.CompareTag("Wall")) //if the object is a wall
        {
            if (this.gameObject.transform.position.x >= 600) //If player is in cave (not in surface)
            {
                spriteRenderer.sortingLayerID = SortingLayer.NameToID("Underground"); //Set  sorting layer to underground
            }
            else //If player on surface
            {
                spriteRenderer.sortingLayerID = SortingLayer.NameToID("Default"); //Set  sorting layer to default.
            }
        }   
    }
}
