using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public GameObject toolTipObject;
    public TMP_Text tooltipName;
    public TMP_Text tooltipDescription;
    public TMP_Text tooltipLine1;
    public TMP_Text tooltipLine2;
    public void Awake()
    {
        toolTipObject.SetActive(false);
    }

    public void EnableTooltip(string itemType, string itemName, string itemDescription, string damageValue, string defenseValue, string healingValue, string attackRate, string blockHealth)
    {
        if (itemType == "Pick")
        {
            toolTipObject.SetActive(true);
            tooltipName.text = itemName;            
            tooltipLine1.text = damageValue + " Mining Damage";
            tooltipLine2.text = attackRate + " Attacks Per Second";
            tooltipDescription.text = itemDescription;
        }

        else if (itemType == "Sword" || itemType == "Axe")
        {
            toolTipObject.SetActive(true);
            tooltipName.text = itemName;
            tooltipLine1.text = damageValue + " Melee Damage";
            tooltipLine2.text = attackRate + " Attacks Per Second";
            tooltipDescription.text = itemDescription;
        }

        else if (itemType == "Bow")
        {
            toolTipObject.SetActive(true);
            tooltipName.text = itemName;
            tooltipLine1.text = damageValue + " Ranged Damage";
            tooltipLine2.text = attackRate + " Attacks Per Second";
            tooltipDescription.text = itemDescription;
        }

        else if (itemType == "Food")
        {
            toolTipObject.SetActive(true);
            tooltipName.text = itemName;
            tooltipLine1.text = healingValue + " Health Healed";
            tooltipLine2.text = "";
            tooltipDescription.text = itemDescription;
        }

        else if (itemType == "Block")
        {
            toolTipObject.SetActive(true);
            tooltipName.text = itemName;
            tooltipLine1.text = blockHealth + " Block Health";
            tooltipLine2.text = "";
            tooltipDescription.text = itemDescription;
        }

        else if (itemType == "Helmet" || itemType == "Chestplate")
        {
            toolTipObject.SetActive(true);
            tooltipName.text = itemName;
            tooltipLine1.text = defenseValue + " Defense";
            tooltipLine2.text = "";
            tooltipDescription.text = itemDescription;
        }
    }

    public void DisableTooltip()
    {
        toolTipObject.SetActive(false);
    }
}
