using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using DG.Tweening;
using PixelCrushers;
using UnityEngine.Audio;
using UnityEngine.UI;
using Core;
using Core.UI;
using Demyth.Gameplay;

namespace UISystem
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] private EnumId _gameViewId;
        [SerializeField] private EnumId _levelMenulId;
        [SerializeField] private EnumId _menuPageId;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private AudioMixer audioMixer;

        private UIPage _uiPage;
        private GameManager _gameManager;
        private GameStateService _gameStateService;
        private LevelManager _levelManager;

        private void Awake()
        {
            _uiPage = GetComponent<UIPage>();
            _levelManager = SceneServiceProvider.GetService<LevelManager>();
            _gameManager = SceneServiceProvider.GetService<GameManager>();
            _gameStateService = SceneServiceProvider.GetService<GameStateService>();

            _gameManager.OnGamePaused.AddListener(GameManager_OnGamePaused);
            _gameManager.OnGameUnpaused.AddListener(GameManager_OnGameUnPaused);

            _resumeButton.onClick.AddListener(ButtonResume);
            _mainMenuButton.onClick.AddListener(ButtonMainMenu);

            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);

            masterVolumeSlider.value = 65f;
            musicVolumeSlider.value = 80f;
            sfxVolumeSlider.value = 80f;
        }

        internal void GameManager_OnGamePaused()
        {
            Open();
        }

        private void GameManager_OnGameUnPaused()
        {
            Close();
        }

        private void Open()
        {
            _uiPage.OpenPage(_uiPage.PageID);
        }

        private void Close()
        {
            _uiPage.ReturnToPage(_gameViewId);
        }

        private void ButtonResume()
        {
            _gameManager.TogglePauseGame();
            Close();
        }

        private void ButtonMainMenu()
        {
            _gameManager.TogglePauseGame();

            DialogueManager.StopAllConversations();
            DOTween.CompleteAll();

            SaveSystem.SaveToSlotImmediate(1);

            _levelManager.OpenLevel(_levelMenulId);
            _uiPage.ReturnToPage(_menuPageId);

            _gameStateService?.SetState(GameState.MainMenu);
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
