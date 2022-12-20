using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlashEffect : MonoBehaviour
{
    [SerializeField] private SimpleFlash flashEffect;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashEffect.Flash();
        }
    }
}
