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
using Lean.Pool;
using Demyth.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace UISystem
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] private EnumId _gameViewId;
        [SerializeField] private EnumId _levelMenulId;
        [SerializeField] private EnumId _menuPageId;
        [Space]
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _setLanguageButton;
        [SerializeField] private Button _mainMenuButton;
        [Space]
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;
        [Space]
        [SerializeField] private TextMeshProUGUI _selectedLanguageText;

        private UIPage _uiPage;
        private GameStateService _gameStateService;
        private LevelManager _levelManager;
        private MusicController _musicController;
        private GameHUD _gameHUD;

        private void Awake()
        {
            _uiPage = GetComponent<UIPage>();
            _levelManager = SceneServiceProvider.GetService<LevelManager>();
            _gameStateService = SceneServiceProvider.GetService<GameStateService>();
            _musicController = SceneServiceProvider.GetService<MusicController>();
            _gameHUD = SceneServiceProvider.GetService<GameHUD>();
        }

        private void Start()
        {
            _gameStateService[GameState.Pause].onEnter += Pause_OnEnter;
            _gameStateService[GameState.Pause].onExit += Pause_OnExit;

            _uiPage.OnOpen.AddListener(UpdateSettingsValue);
            
            _resumeButton.onClick.AddListener(ButtonResume);
            _setLanguageButton.onClick.AddListener(ToggleLanguage);
            _mainMenuButton.onClick.AddListener(ButtonMainMenu);

            _masterVolumeSlider.onValueChanged.AddListener(SetMMSoundMasterVolume);
            _musicVolumeSlider.onValueChanged.AddListener(SetMMSoundMusicVolume);
            _sfxVolumeSlider.onValueChanged.AddListener(SetMMSoundSfxVolume);
        }

        private void ToggleLanguage()
        {
            if (Localization.isDefaultLanguage)
            {
                SetLanguageToIndonesia();
            }
            else
            {
                SetLanguageToDefault();
            }
        }

        private void Pause_OnEnter(GameState state)
        {
            _uiPage.OpenPage(_uiPage.PageID);
        }

        private void Pause_OnExit(GameState state)
        {
            _uiPage.Return();
        }

        private void ButtonResume()
        {
            _gameStateService.SetState(GameState.Gameplay);
        }

        private void ButtonMainMenu()
        {
            // _musicController.StopMusic();

            // _gameStateService?.SetState(GameState.MainMenu);

            // DialogueManager.StopAllConversations();
            // DOTween.CompleteAll();
            DOTween.KillAll();
            LeanPool.DespawnAll();

            // _levelManager.OpenLevel(_levelMenulId);
            // _gameHUD.Close();
            // _uiPage.OpenPage(_menuPageId);

            
            Time.timeScale = 1f;
            AudioListener.pause = false;
            SceneManager.LoadScene(0);

        }

        private void UpdateSettingsValue()
        {
            if (Localization.isDefaultLanguage)
            {
                _selectedLanguageText.text = "English";
            }
            else
            {
                _selectedLanguageText.text = "Indonesia";
            }

            _masterVolumeSlider.value = MMSoundManager.Current.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Master, false);
            _musicVolumeSlider.value = MMSoundManager.Current.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Music, false);
            _sfxVolumeSlider.value = MMSoundManager.Current.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Sfx, false);
        }

        private void SetMMSoundMasterVolume(float volume)
        {
            MMSoundManager.Current.SetVolumeMaster(volume);
            MMSoundManager.Current.SaveSettings();
        }

        private void SetMMSoundMusicVolume(float volume)
        {
            MMSoundManager.Current.SetVolumeMusic(volume);
            MMSoundManager.Current.SaveSettings();
        }

        private void SetMMSoundSfxVolume(float volume)
        {
            MMSoundManager.Current.SetVolumeSfx(volume);
            MMSoundManager.Current.SaveSettings();
        }

        private void SetLanguageToDefault()
        {
            DialogueManager.SetLanguage("");
            PlayerPrefs.SetString("SelectedLanguage", "default");

            _selectedLanguageText.text = "English";

            Debug.Log("set language to default");
        }

        private void SetLanguageToIndonesia()
        {
            DialogueManager.SetLanguage("id");
            PlayerPrefs.SetString("SelectedLanguage", "id");

            _selectedLanguageText.text = "Indonesia";

            Debug.Log("set language to id");
        }
    }
}
