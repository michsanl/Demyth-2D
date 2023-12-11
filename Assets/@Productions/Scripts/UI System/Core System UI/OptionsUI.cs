using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Core;
using Core.UI;
using MoreMountains.Tools;

namespace UISystem
{
    public class OptionsUI : MonoBehaviour
    {
        [SerializeField] 
        private Slider masterVolumeSlider;
        [SerializeField] 
        private Slider musicVolumeSlider;
        [SerializeField] 
        private Slider sfxVolumeSlider;
        [SerializeField] 
        private AudioMixer audioMixer;
        
        private UIPage _uiPage;

        private void Awake()
        {
            _uiPage = GetComponent<UIPage>();

            masterVolumeSlider.onValueChanged.AddListener(SetMMSoundMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(SetMMSoundMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(SetMMSoundSfxVolume);
        }

        private void Start()
        {
            masterVolumeSlider.value = MMSoundManager.Current.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Master, false);
            musicVolumeSlider.value = MMSoundManager.Current.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Music, false);
            sfxVolumeSlider.value = MMSoundManager.Current.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Sfx, false);
        }

        private void SetMMSoundMasterVolume(float volume)
        {
            MMSoundManager.Current.SetVolumeMaster(volume);
        }

        private void SetMMSoundMusicVolume(float volume)
        {
            MMSoundManager.Current.SetVolumeMusic(volume);
        }

        private void SetMMSoundSfxVolume(float volume)
        {
            MMSoundManager.Current.SetVolumeSfx(volume);
        }



        private void SetMasterVolume(float sliderValue)
        {
            float minVolumeValue = -80f;
            float volumeValue = minVolumeValue + sliderValue;

            audioMixer.SetFloat("MasterVolume", volumeValue);
        }

        private void SetMusicVolume(float sliderValue)
        {
            float minVolumeValue = -80f;
            float volumeValue = minVolumeValue + sliderValue;

            audioMixer.SetFloat("MusicVolume", volumeValue);
        }

        private void SetSFXVolume(float sliderValue)
        {
            float minVolumeValue = -80f;
            float volumeValue = minVolumeValue + sliderValue;

            audioMixer.SetFloat("SFXVolume", volumeValue);
        }
    }
}
