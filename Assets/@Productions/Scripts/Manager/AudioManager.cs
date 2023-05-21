using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTools.Core;
using System;

public class AudioManager : CoreBehaviour
{
    [SerializeField] private AudioSource musicSource, soundSource;

    public AudioClipSriSO SriAudioSource;

    public void PlayMusic(AudioClip clip)
    {
        musicSource.PlayOneShot(clip);
    }

    public void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }
}
