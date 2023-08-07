using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    //This script will go on grass to control all the states.
    [SerializeField] private GameObject treeObject;
    [SerializeField] private GameObject redShroomObject;
    [SerializeField] private GameObject brownMushroomObject;

    [SerializeField] private BoxCollider2D treeCollider;
    [SerializeField] private BoxCollider2D redMushroomCollider;
    [SerializeField] private BoxCollider2D brownMushroomCollider;

    public void EnableTree()
    {
        DisableAllStates();
        treeObject.SetActive(true); //Enable Tree
        treeCollider.enabled = true;
    }

    public void EnableRedMushroom()
    {
        DisableAllStates();
        redShroomObject.SetActive(true); //Enable Red Shroom
        redMushroomCollider.enabled = true;
    }
    public void EnableBrownMushroom()
    {
        DisableAllStates();
        brownMushroomObject.SetActive(true); //Enable Brown Shroom
        brownMushroomCollider.enabled = true;
    }

    public void DisableAllStates()
    {
        treeObject.SetActive(false);
        redShroomObject.SetActive(false);
        brownMushroomObject.SetActive(false);
        treeCollider.enabled = false;
        redMushroomCollider.enabled = false;
        brownMushroomCollider.enabled = false;
    }
}
