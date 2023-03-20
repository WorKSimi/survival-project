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
    public void Awake()
    {
        toolTipObject.SetActive(false);
    }

    public void EnableTooltip(string itemName, string itemDescription)
    {
        toolTipObject.SetActive(true);
        tooltipName.text = itemName;
        tooltipDescription.text = itemDescription;
    }

    public void DisableTooltip()
    {
        toolTipObject.SetActive(false);
    }
}
