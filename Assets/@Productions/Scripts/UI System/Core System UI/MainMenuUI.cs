using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.UI;
using Demyth.Gameplay;
using Demyth.UI;
using MoreMountains.Tools;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UISystem
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private GameSettingsSO _gameSettingsSO;
        [SerializeField] private Button _continueButton;
        [SerializeField] private TextMeshProUGUI _continueButtonText;
        [SerializeField] private GameObject _selectLevelButtons;
        [Header("Level")]
        [SerializeField] private EnumId newLevelId;

        private UIPage _uiPage;
        private LevelManager _levelManager;
        private GameStateService _gameStateService;
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
            _selectLevelButtons.SetActive(_gameSettingsSO.ShowLevelSelect);
            _musicController.PlayMainMenuBGM();
            SetupContinueButtonVisual();
        }

        public void StartNewGame()
        {
            _uiPage.OnClosed.AddListener(NewGameButton);
            _uiPage.Return();
        }

        public void ContinueGame()
        {
            _uiPage.OnClosed.AddListener(ContinueButton);
            _uiPage.Return();
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void StartToLevel(EnumId levelId)
        {
            _levelManager.OpenLevel(levelId);
            _uiPage.Return();
            _gameHUD.Open();

            SaveSystem.SaveToSlot(1);
            _gameStateService?.SetState(GameState.Gameplay);

            _musicController.PlayLevelBGM();
            _musicController.FadeInCurrentMusic(1.5f);
        }

        private void NewGameButton()
        {
            _gameStateService?.SetState(GameState.Gameplay);
            DialogueManager.StartConversation("Intro");
        }

        private void ContinueButton()
        {
            StartCoroutine(ContinueGameCoroutine());
        }

        private IEnumerator ContinueGameCoroutine()
        {
            _musicController.FadeOutCurrentMusic(.9f);

            yield return StartCoroutine(PersistenceLoadingUI.Instance.OpenLoadingPage());

            SaveSystem.LoadFromSlot(1);
            _gameHUD.InstantOpen();
            _musicController.PlayLevelBGM();
            _musicController.FadeInCurrentMusic(.9f);

            yield return StartCoroutine(PersistenceLoadingUI.Instance.CloseLoadingPage());
            
            _gameStateService?.SetState(GameState.Gameplay);
        }

        private void SetupContinueButtonVisual()
        {
            if (SaveSystem.HasSavedGameInSlot(1))
            {
                _continueButton.enabled = true;
                _continueButtonText.alpha = 1f;
            }
            else
            {
                _continueButton.enabled = false;
                _continueButtonText.alpha = 0.5f;
            }
        }
    }
}
