using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSortingLayerUnderground : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    //private GameObject thisPlayer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>(); //Player sprite renderer
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.position.x >= 600) //If player is in cave (not in surface)
        {
            spriteRenderer.sortingLayerID = SortingLayer.NameToID("Underground"); //Set player sorting layer to underground
        }
        else //If player on surface
        {
            spriteRenderer.sortingLayerID = SortingLayer.NameToID("Default"); //Set player sorting layer to default.
        }
    }
}
