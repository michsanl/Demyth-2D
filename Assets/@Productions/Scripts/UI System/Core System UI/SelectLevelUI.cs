using Core;
using Core.UI;
using Demyth.Gameplay;
using PixelCrushers;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UISystem
{
    public class SelectLevelUI : UIPageView
    {

        [SerializeField]
        private EnumId gameViewId;

        [Header("Level")]
        [SerializeField]
        private EnumId newLevelId;

        private UIPage _uiPage;
        private LevelManager _levelManager;
        private GameInputController _gameInputController;
        private GameInput _gameInput;
        private GameStateService _gameStateService;

        private void Awake()
        {
            _uiPage = GetComponent<UIPage>();
            _levelManager = SceneServiceProvider.GetService<LevelManager>();
            _gameInputController = SceneServiceProvider.GetService<GameInputController>();
            _gameStateService = SceneServiceProvider.GetService<GameStateService>();
            _gameInput = _gameInputController.GameInput;            
        }       

        public void ButtonGoToLevel(EnumId levelId)
        {
            _levelManager.OpenLevel(levelId);
            _uiPage.OpenPage(gameViewId);
            
            _gameStateService?.SetState(GameState.Gameplay);
        }

        public void StartNewGame()
        {
            ClearGameProgressSave();
            SaveSystem.LoadFromSlot(0);

            _levelManager.OpenLevel(newLevelId);
            _uiPage.OpenPage(gameViewId);

            _gameStateService?.SetState(GameState.Gameplay);
        }

        public void StartToLevel(EnumId levelId)
        {
            _levelManager.OpenLevel(levelId);
            _uiPage.OpenPage(gameViewId);

            _gameStateService?.SetState(GameState.Gameplay);
        }

        public void ContinueGame()
        {
            SaveSystem.LoadFromSlot(1);
            _uiPage.OpenPage(gameViewId);

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
