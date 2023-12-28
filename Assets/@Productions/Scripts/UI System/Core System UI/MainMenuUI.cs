using System;
using Core;
using Core.UI;
using Demyth.Gameplay;
using Demyth.UI;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UISystem
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField]
        private EnumId gameViewId;
        [SerializeField]
        private GameHUD _gameHUD;

        [Header("Level")]
        [SerializeField]
        private EnumId newLevelId;

        private UIPage _uiPage;
        private LevelManager _levelManager;
        private GameStateService _gameStateService;

        private void Awake()
        {
            _uiPage = GetComponent<UIPage>();
            _levelManager = SceneServiceProvider.GetService<LevelManager>();
            _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        }

        public void StartNewGame()
        {
            ClearGameProgressSave();
            SaveSystem.LoadFromSlot(0);

            _levelManager.OpenLevel(newLevelId);
            _uiPage.Return();
            
            SaveSystem.SaveToSlot(1);
            _gameStateService?.SetState(GameState.Gameplay);

            DialogueManager.StartConversation("Intro");
        }

        public void StartToLevel(EnumId levelId)
        {
            _levelManager.OpenLevel(levelId);
            _uiPage.Return();
            _gameHUD.gameObject.SetActive(true);
            _gameHUD.Open();

            _gameStateService?.SetState(GameState.Gameplay);
        }

        public void ContinueGame()
        {
            SaveSystem.LoadFromSlot(1);
            _uiPage.Return();
            _gameHUD.gameObject.SetActive(true);
            _gameHUD.Open();

            _gameStateService?.SetState(GameState.Gameplay);
        }

        public void QuitGame()
        {
            Application.Quit();
        }               

        private static void ClearGameProgressSave()
        {
            SaveSystem.DeleteSavedGameInSlot(1);
            SaveSystem.DeleteSavedGameInSlot(2);
            SaveSystem.DeleteSavedGameInSlot(3);
        }  
    }
}
