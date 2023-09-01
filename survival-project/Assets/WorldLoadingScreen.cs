using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldLoadingScreen : MonoBehaviour
{
    public GameObject worldLoadScreenObject;
    public Slider slider;
    public Slider totalLoadSlider;
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = 0f;
        totalLoadSlider.value = 0f;
        text.text = "Loading Surface...";
    }

    public void UpdateLoadBar(float amount)
    {
        slider.value = amount;
    }

    public void SurfaceLoaded()
    {
        slider.value = 0f;
        totalLoadSlider.value = 50f;
        text.text = "Loading Underground...";
    }

    public IEnumerator AllLoadingComplete()
    {
        text.text = "Loading Complete!!!";
        totalLoadSlider.value = 100f;
        this.gameObject.transform.position = new Vector3(100f, 100f, 0);
        yield return new WaitForSeconds(2f); //Wait 2 seconds for chunks to load.
        worldLoadScreenObject.SetActive(false);
    }
}
