using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldLoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject caveLoadingScreen;
    [SerializeField] private Image caveLoadingScreenImage;

    [SerializeField] private Color myColor;

    public GameObject worldLoadScreenObject;
    public Slider slider;
    public Slider totalLoadSlider;
    public TMP_Text text;

    private Color color;
    private Color transparentColor;

    private float fadeOutDuration = 1f;
    private float fadeInDuration = 1f;
    

    // Start is called before the first frame update
    void Start()
    {
        slider.value = 0f;
        totalLoadSlider.value = 0f;
        text.text = "Loading Surface...";

        color = new Color(0, 0, 0, 1);
        transparentColor = new Color(0, 0, 0, 0);
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

    private IEnumerator DoFade(Color a, Color b, float duration)
    {
        float elapsedTime = 0f;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            caveLoadingScreenImage.color = Color.Lerp(a, b, (elapsedTime / duration));
            yield return null;
        }
    }

    private IEnumerator FadeOut() // 1.0 > 0.0
    {
        yield return StartCoroutine(DoFade(color, transparentColor, fadeOutDuration));
    }

    private IEnumerator FadeIn() //0.0 > 1.0
    {
        yield return StartCoroutine(DoFade(transparentColor, color, fadeInDuration));
    }

    public void CaveFadeIn()
    {
        caveLoadingScreen.SetActive(true);
        StartCoroutine(FadeIn());
    }
    public void CaveFadeOut()
    {
        caveLoadingScreen.SetActive(true);
        StartCoroutine(FadeOut());
    }

    public IEnumerator CaveTest()
    {
        caveLoadingScreen.SetActive(true);
        StartCoroutine(FadeIn()); //Fade In to Black
        yield return new WaitForSeconds(3f); //Wait 3 seconds
        StartCoroutine(FadeOut()); //Fade Out to Not Black
    }
}
