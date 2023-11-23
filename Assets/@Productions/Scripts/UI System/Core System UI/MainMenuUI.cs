using Core;
using Core.UI;
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

        private LevelManager _levelManager;
        private GameInputController _gameInputController;
        private GameInput _gameInput;
        private UIPage _uiPage;

        private void Awake()
        {
            _uiPage = GetComponent<UIPage>();
            _levelManager = SceneServiceProvider.GetService<LevelManager>();
            _gameInputController = SceneServiceProvider.GetService<GameInputController>();
            _gameInput = _gameInputController.GameInput;            
        }       

        public void StartNewGame()
        {
            SaveSystem.ClearSavedGameData();

            Level firstLevel = _levelManager.GetLevelByID("Level 1");
            _levelManager.SetLevel(firstLevel);

            _uiPage.OpenPage(gameViewId);

            //SceneUI.Context.Player.ActivatePlayer();
        }

        public void StartToLevel(string levelID)
        {
            Level targetLevel = _levelManager.GetLevelByID(levelID);
            _levelManager.SetLevel(targetLevel);

            _uiPage.OpenPage(gameViewId);

            //SceneUI.Context.Player.ActivatePlayer();
        }

        public void ContinueGame()
        {
            _gameInput.EnablePlayerInput();
            _gameInput.EnablePauseInput();
            
            SaveSystem.LoadFromSlot(1);

            _uiPage.OpenPage(gameViewId);
        }

        public void QuitGame()
        {
            Application.Quit();
        }                 
    }
}
