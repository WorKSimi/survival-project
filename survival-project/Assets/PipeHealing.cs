using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PipeHealing : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;   
    [SerializeField] private TMP_Text herbAmountText; //Text for the amount of herb you got.
    [SerializeField] private Database inventoryItemDatabase;
    [SerializeField] private MouseItemData mouseItemData;

    [SerializeField] private PipeInventorySlot pipeSlot;
    [SerializeField] private HerbInventorySlot herbSlot;

    [SerializeField] private GameObject cooldownTimerObject;
    [SerializeField] private TMP_Text cooldownText;

    private float healCooldown; //Float for the healing cooldown
    private int healValue; //Amount healed by herb
    private float healTime; //Time it takes to heal

    private bool isPipeEquipped = true; //Flag for if pipe is equipped. True by default.
    private bool isHerbEquipped = true; //Flag for if herb is equipped. True by default.

    private bool canHeal = true; //Bool for if healing is allowed. This is TRUE by default.
    private float timeHeld;
    private float timer;

    private void Update()
    {
        if (Input.GetKey(KeyCode.H) && canHeal == true && playerHealth.currentHealth < playerHealth.maxHealth) //If hold down H AND canHeal = true AND player does not have full health.
        {
            timeHeld += Time.deltaTime; //Start counting up timer

            if (timeHeld >= pipeSlot.pipeInventorySlot.itemData.pipeHealTime) //If held down as long as healTime requires..
            {
                PipeHeal(); //Do healing time
            }
        }

        if (canHeal == false) //If pipe on cooldown
        {
            TimerUI();
        }
    }

    private void PipeHeal() //This script handles the pipe healing
    {
        SetHealCooldown();
        if (isPipeEquipped == false || isHerbEquipped == false) return; //if theres no pipe or herb equipped, return and dont do anything.
        healValue = herbSlot.herbInventorySlot.itemData.herbHealValue;
        timer = healCooldown; //Set timer to be equal to heal cooldown
        herbSlot.herbInventorySlot.RemoveFromStack(1); //Remove herb when healing
        herbSlot.UpdateHerbSlot();
        playerHealth.HealHealth(healValue); //Heal player HP by health value
        
        timeHeld = 0; //Reset the timeHeld value
        StartCoroutine(healingCooldown()); //Start healing cooldown
    }

    public void SetHealCooldown()
    {
        healCooldown = pipeSlot.pipeInventorySlot.itemData.pipeHealCooldown;
    }
 

    private IEnumerator healingCooldown() //This handles the healing cooldown
    {
        canHeal = false; //Set flag to false
        cooldownTimerObject.SetActive(true);
        yield return new WaitForSeconds(healCooldown); //Wait for amount of time cooldown is set to.
        cooldownTimerObject.SetActive(false);
        canHeal = true; //Set flag to true
    }

    private void TimerUI()
    {
        timer -= Time.deltaTime;
        int timer2 = ((int)timer);
        cooldownText.text = timer2.ToString();
    }
}
