using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Core;

public class AudioManager : SceneService
{

    public AraClipSO AraAudioSource => araAudioSource;
    public PetraClipSO PetraAudioSource => petraAudioSource;
    public SriClipSO SriAudioSource => sriAudioSource;
    public AudioClip TuyulDash => tuyulDash;

    [SerializeField] private AraClipSO araAudioSource;
    [SerializeField] private PetraClipSO petraAudioSource;
    [SerializeField] private SriClipSO sriAudioSource;
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
