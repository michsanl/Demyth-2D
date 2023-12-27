using Core;
using Core.UI;
using Demyth.Gameplay;
using PixelCrushers;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UISystem
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField]
        private EnumId gameViewId;

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
            _uiPage.OpenPage(gameViewId);
            
            SaveSystem.SaveToSlot(1);
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
            _uiPage.Return();
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
