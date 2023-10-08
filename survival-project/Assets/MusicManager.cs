using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private bool isMainMenu;

    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;

    [Header("Audio Clip")]
    public AudioClip backgroundMusic;

    private void Awake()
    {
        if (isMainMenu == true)
        {
            StartMusic();
        }
    }

    public void StartMusic()
    {
        musicSource.loop = true;
        musicSource.volume = 0.1f; //Tone volume down
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }
}
