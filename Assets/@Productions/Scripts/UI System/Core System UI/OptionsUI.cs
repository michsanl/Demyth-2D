using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

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

        private void Awake()
        {
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);

            masterVolumeSlider.value = 65f;
            musicVolumeSlider.value = 80f;
            sfxVolumeSlider.value = 80f;
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
