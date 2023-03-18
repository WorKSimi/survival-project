using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerBuffsDebuffs : MonoBehaviour
{
    //This script will contain the buffs and debuffs of the player
    //A list of debuff and buff bools will be in the script
    //When a buff or debuff is true, it will have different effects on the player
    //Buff will be set true by other scripts or enemies

    //After a time limit, the buff will be set back to false
    //While the buff is active, a visual on the ui and a timer will be displayed (LIKE TERRARIA)
    //When over it will vanish

    public bool FullStomachDebuff;
    private int activeBuffs; //Int to keep track of how many buffs are 

    public GameObject foodCooldownUIObject;
    public TMP_Text timerCountdown;
    private float timeLeft;
    private float maxTime = 60f;

    public void Start()
    {
        timeLeft = maxTime;
        foodCooldownUIObject.SetActive(false);
    }

    public void Update()
    {
        if (FullStomachDebuff == true)
        {
            foodCooldownUIObject.SetActive(true); 
            timeLeft -= Time.deltaTime;
            int timer = ((int)timeLeft);
            timerCountdown.text = timer.ToString();
        }
        else if (FullStomachDebuff == false)
        {
            foodCooldownUIObject.SetActive(false); //Turn debuff thing off
            timeLeft = maxTime; //Reset timer
        }
    }
}
