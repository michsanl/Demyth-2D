using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using DG.Tweening;
using PixelCrushers;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UISystem
{
    public class PauseUI : UIPageView
    {

        

        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;


        protected override void OnInitialize()
        {
            SceneUI.Context.GameManager.OnGamePaused += GameManager_OnGamePaused;
            SceneUI.Context.GameManager.OnGameUnpaused += GameManager_OnGameUnPaused;

            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);

            masterVolumeSlider.value = 65f;
            musicVolumeSlider.value = 80f;
            sfxVolumeSlider.value = 80f;
        }

        private void GameManager_OnGamePaused()
        {
            Open();
            gameObject.SetActive(true);
        }

        private void GameManager_OnGameUnPaused()
        {
            Close();
            gameObject.SetActive(false);
        }

        protected override void OnOpen()
        {
            Canvas.enabled = true;
        }

        protected override void OnClosed()
        {
            Canvas.enabled = false;
        }

        public void ButtonResume()
        {
            Close();
            
            SceneUI.Context.GameManager.TogglePauseGame();
        }

        public void ButtonGoToMainMenu()
        {
            Close();

            DialogueManager.StopAllConversations();
            SceneUI.Context.GameManager.TogglePauseGame();

            Level mainMenuLevel = SceneUI.Context.LevelManager.MainMenuLevel;
            SceneUI.Context.LevelManager.SetLevel(mainMenuLevel);

            Open<MainMenuUI>();

            SceneUI.Context.Player.gameObject.SetActive(false);
 
            SceneUI.Context.CameraNormal.transform.DOKill();
            SceneUI.Context.CameraNormal.transform.localPosition = Vector3.zero;
        }

        public void ButtonMainMenu()
        {
            DialogueManager.StopAllConversations();
            SceneUI.Context.GameManager.TogglePauseGame();
            DOTween.KillAll();

            SaveSystem.SaveToSlotImmediate(1);

            SaveSystem.RestartGame("Demyth Game");
        }

        public void ButtonOptions()
        {
            Close();

            Open<OptionsUI>();
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
