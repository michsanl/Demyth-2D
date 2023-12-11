using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Core;

public class AudioManager : SceneService
{

    public AudioClipAraSO AraAudioSource => araAudioSource;
    public AudioClipPetraSO PetraAudioSource => petraAudioSource;
    public AudioClipSriSO SriAudioSource => sriAudioSource;
    public AudioClip TuyulDash => tuyulDash;

    [SerializeField] private AudioClipAraSO araAudioSource;
    [SerializeField] private AudioClipPetraSO petraAudioSource;
    [SerializeField] private AudioClipSriSO sriAudioSource;
    [SerializeField] private AudioClip tuyulDash;
    [SerializeField] private AudioSource musicSource, soundSource;

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
