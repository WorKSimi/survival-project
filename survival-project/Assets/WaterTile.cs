using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTile : MonoBehaviour
{
    //Water Tile will have a few properties

    //When a player walks on it, it will reduce the players speed, as they are "Swimming"

    //Perhaps set a players state to InWater
    //If they are in water, change their speed, and maybe do a swimming thing
    //private PlayerNetwork playerNetwork;

    //private void OnTriggerEnter2D(Collider2D collision) //When object touches water
    //{
    //    if (collision.CompareTag("Player")) //If object is tagged Player
    //    {
    //        playerNetwork = collision.GetComponent<PlayerNetwork>(); //Player network is the colliding player's script
    //        playerNetwork.state = PlayerNetwork.State.InWater; //Set the player network to in water
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D collision) //When object leaves water
    //{
    //    if (collision.CompareTag("Player")) //If tag is player
    //    {
    //        playerNetwork = collision.GetComponent<PlayerNetwork>(); //Player network is the colliding player's script
    //        playerNetwork.state = PlayerNetwork.State.Normal; //Set the player network to normal
    //    }
    //}

    //Also, when left clicked on by either a Watering can or Bottle, it will set the water cans amount to full and replace a glass
    //bottle with a water bottle

    //This can easily be achieved by just giving water a tag and doing the similar function of the pickaxe.
}
