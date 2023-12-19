using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundObject : MonoBehaviour
{
    //This objects purpose is to play a sound when its created, and destroy itself when its done.
    //Useful for quickly playing sound effects.

    [SerializeField] private AudioSource source;
    public AudioClip clip;


    private void Awake()
    {
        source.PlayOneShot(clip);
        StartCoroutine(WaitForSound(clip));
    }

    private IEnumerator WaitForSound(AudioClip Sound)
    {
        yield return new WaitUntil(() => source.isPlaying == false);
        Destroy(this.gameObject);
    }
}
