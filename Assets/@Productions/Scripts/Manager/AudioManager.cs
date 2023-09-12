using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTools.Core;
using System;

public class AudioManager : CoreBehaviour
{

    public AudioClipAraSO AraAudioSO => araAudioSO;
    public AudioClipPetraSO PetraAudioSO => pretraAudioSO;
    public AudioClipSriSO SriAudioSO => sriAudioSO;

    [SerializeField] private AudioClipAraSO araAudioSO;
    [SerializeField] private AudioClipPetraSO pretraAudioSO;
    [SerializeField] private AudioClipSriSO sriAudioSO;
    [SerializeField] private AudioSource musicSource, soundSource;

    public void PlayMusic(AudioClip clip)
    {
        musicSource.PlayOneShot(clip);
    }

    public void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }
}
