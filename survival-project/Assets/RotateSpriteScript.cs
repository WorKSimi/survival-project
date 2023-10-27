using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSpriteScript : MonoBehaviour
{
    [SerializeField] private float degreesperSecond;

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, degreesperSecond) * Time.deltaTime);
    }
}
