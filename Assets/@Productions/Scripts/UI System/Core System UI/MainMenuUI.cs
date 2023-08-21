using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UISystem
{
    public class MainMenuUI : UIPageView
    {
        [SerializeField] private GameObject mainPage;
        [SerializeField] private GameObject selectLevelPage;
        [SerializeField] private GameObject optionPage;
        [Space]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [Space]
        [SerializeField] private AudioMixer audioMixer;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);

            masterVolumeSlider.value = 65f;
            musicVolumeSlider.value = 80f;
            sfxVolumeSlider.value = 80f;
        }

        protected override void OnOpen()
        {
            Canvas.enabled = true;
        }

        protected override void OnClosed()
        {    
            Canvas.enabled = false;
        }

        public void ButtonNewGame()
        {
            Close();

            Level firstLevel = SceneUI.Context.LevelManager.GetLevelByID("Level 1");
            SceneUI.Context.LevelManager.SetLevel(firstLevel);
            SceneUI.Context.HUDUI.Open();

            SceneUI.Context.Player.ActivatePlayer();
        }

        public void ButtonGoToLevel(string levelID)
        {
            Close();

            Level targetLevel = SceneUI.Context.LevelManager.GetLevelByID(levelID);
            SceneUI.Context.LevelManager.SetLevel(targetLevel);
            SceneUI.Context.HUDUI.Open();

            SceneUI.Context.Player.ActivatePlayer();

            mainPage.SetActive(true);
            selectLevelPage.SetActive(false);
        }

        public void ButtonContinue()
        {
            mainPage.SetActive(false);
            selectLevelPage.SetActive(true);
        }

        public void ButtonContinueNew()
        {
            Close();

            SceneUI.Context.HUDUI.Open();
            SceneUI.Context.GameInput.EnablePlayerInput();
            SceneUI.Context.GameInput.EnablePauseInput();
            
            PixelCrushers.SaveSystem.LoadFromSlot(1);
        }

        public void ButtonOption()
        {
            mainPage.SetActive(false);
            optionPage.SetActive(true);
        }

        public void ButtonOptionReturn()
        {
            optionPage.SetActive(false);
            mainPage.SetActive(true);
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
    
        public void ButtonQuit()
        {
            Application.Quit();
        }
    }
}
