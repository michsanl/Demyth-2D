using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTools.Core;
using System;

public class AudioManager : CoreBehaviour
{

    public AudioClipSriSO SriAudioSource => sriAudioSource;

    [SerializeField] private AudioSource musicSource, soundSource;
    [SerializeField] private AudioClipSriSO sriAudioSource;

    public void PlayMusic(AudioClip clip)
    {
        musicSource.PlayOneShot(clip);
    }

    public void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }

    public void PlayClipAtPoint(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
}
