using Core;
using PixelCrushers;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UISystem
{
    public class MainMenuUI : MonoBehaviour
    {
        private LevelManager _levelManager;
        private GameInputController _gameInputController;
        private GameInput _gameInput;

        private void Awake()
        {
            _levelManager = SceneServiceProvider.GetService<LevelManager>();
            _gameInputController = SceneServiceProvider.GetService<GameInputController>();
            _gameInput = _gameInputController.GameInput;            
        }       

        public void StartNewGame()
        {
            SaveSystem.ClearSavedGameData();

            Level firstLevel = _levelManager.GetLevelByID("Level 1");
            _levelManager.SetLevel(firstLevel);

            //SceneUI.Context.Player.ActivatePlayer();
        }

        public void StartToLevel(string levelID)
        {
            Level targetLevel = _levelManager.GetLevelByID(levelID);
            _levelManager.SetLevel(targetLevel);

            //SceneUI.Context.Player.ActivatePlayer();
        }

        public void ContinueGame()
        {
            _gameInput.EnablePlayerInput();
            _gameInput.EnablePauseInput();
            
            SaveSystem.LoadFromSlot(1);
        }

        public void QuitGame()
        {
            Application.Quit();
        }                 
    }
}
