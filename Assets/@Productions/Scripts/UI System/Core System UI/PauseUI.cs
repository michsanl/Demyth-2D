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
using MoreMountains.Tools;

namespace UISystem
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] private EnumId _gameViewId;
        [SerializeField] private EnumId _levelMenulId;
        [SerializeField] private EnumId _menuPageId;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;
        [SerializeField] private AudioMixer audioMixer;

        private UIPage _uiPage;
        private GameStateService _gameStateService;
        private LevelManager _levelManager;

        private void Awake()
        {
            _uiPage = GetComponent<UIPage>();
            _levelManager = SceneServiceProvider.GetService<LevelManager>();
            _gameStateService = SceneServiceProvider.GetService<GameStateService>();

            _gameStateService[GameState.Pause].onEnter += Pause_OnEnter;
            _gameStateService[GameState.Gameplay].onEnter += Gameplay_OnEnter;

            _resumeButton.onClick.AddListener(ButtonResume);
            _mainMenuButton.onClick.AddListener(ButtonMainMenu);

            _masterVolumeSlider.onValueChanged.AddListener(SetMMSoundMasterVolume);
            _musicVolumeSlider.onValueChanged.AddListener(SetMMSoundMusicVolume);
            _sfxVolumeSlider.onValueChanged.AddListener(SetMMSoundSfxVolume);
        }

        private void Start()
        {
            _masterVolumeSlider.value = MMSoundManager.Current.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Master, false);
            _musicVolumeSlider.value = MMSoundManager.Current.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Music, false);
            _sfxVolumeSlider.value = MMSoundManager.Current.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Sfx, false);
        }

        private void Gameplay_OnEnter(GameState state)
        {
            if (_gameStateService.PreviousState == GameState.Pause)
            {
                _uiPage.ReturnToPage(_gameViewId);
            }
        }

        private void Pause_OnEnter(GameState state)
        {
            _uiPage.OpenPage(_uiPage.PageID);
        }

        private void ButtonResume()
        {
            _gameStateService.SetState(GameState.Gameplay);
        }

        private void ButtonMainMenu()
        {
            DialogueManager.StopAllConversations();
            DOTween.CompleteAll();

            _levelManager.OpenLevel(_levelMenulId);
            _uiPage.ReturnToPage(_menuPageId);

            _gameStateService?.SetState(GameState.MainMenu);
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
