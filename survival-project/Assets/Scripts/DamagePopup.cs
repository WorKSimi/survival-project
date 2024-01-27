using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh;

    public void CreatePopup(float damageAmount) //Take in location, take in damage amount
    {
        textMesh.SetText(damageAmount.ToString());
        StartCoroutine(DestroyPopup());     
    }

    public IEnumerator DestroyPopup()
    {
        yield return new WaitForSeconds(2f); //Wait 2 seconds
        Destroy(this.gameObject);
    }
}
