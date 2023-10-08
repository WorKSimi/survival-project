using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        var num = Random.Range(0, sprites.Length); //Get random member of array
        spriteRenderer.sprite = sprites[num]; //Set sprite renderer to a random one.
    }
}
