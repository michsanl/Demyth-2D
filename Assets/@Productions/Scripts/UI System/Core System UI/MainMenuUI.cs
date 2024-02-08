using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.UI;
using Demyth.Gameplay;
using Demyth.UI;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UISystem
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private GameSettingsSO _gameSettingsSO;
        [SerializeField] private GameObject _selectLevelButtons;
        [SerializeField] private Button _newGameButton;
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

            _uiPage.OnOpen.AddListener(() => { _newGameButton.Select(); });
        }

        private void Start()
        {
            _selectLevelButtons.SetActive(_gameSettingsSO.ShowLevelSelect);
            _musicController.PlayMainMenuBGM();
        }

        public void StartNewGame()
        {
            _uiPage.Return();
            _levelManager.OpenLevel(newLevelId);
            
            SaveSystem.SaveToSlot(1);
            _gameStateService?.SetState(GameState.Gameplay);

            DialogueManager.StartConversation("Intro");
        }

        public void ContinueGame()
        {
            StartCoroutine(ContinueGameCoroutine());
        }

        private IEnumerator ContinueGameCoroutine()
        {
            _musicController.FadeOutCurrentMusic(.9f);

            yield return StartCoroutine(PersistenceLoadingUI.Instance.OpenLoadingPage());

            SaveSystem.LoadFromSlot(1);
            _uiPage.Return();
            _gameHUD.InstantOpen();
            _musicController.PlayLevelBGM();
            _musicController.FadeInCurrentMusic(.9f);

            yield return StartCoroutine(PersistenceLoadingUI.Instance.CloseLoadingPage());
            
            _gameStateService?.SetState(GameState.Gameplay);
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

        public void QuitGame()
        {
            Application.Quit();
        }

        public void GoToGameplayScene()
        {
            SceneManager.LoadSceneAsync(1);
        }
    }
}
