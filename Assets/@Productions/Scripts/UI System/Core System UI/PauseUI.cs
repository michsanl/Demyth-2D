using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;

namespace UISystem
{
    public class PauseUI : UIPageView
    {
        protected override void OnInitialize()
        {
            SceneUI.Context.GameManager.OnGamePaused += GameManager_OnGamePaused;
            SceneUI.Context.GameManager.OnGameUnpaused += GameManager_OnGameUnPaused;
        }

        private void GameManager_OnGamePaused()
        {
            Open();
        }

        private void GameManager_OnGameUnPaused()
        {
            Close();
        }

        protected override void OnOpen()
        {
            Canvas.enabled = true;
        }

        protected override void OnClosed()
        {
            Canvas.enabled = false;
        }

        public void ButtonResume()
        {
            Close();
            
            SceneUI.Context.GameManager.TogglePauseGame();
        }

        public void ButtonGoToMainMenu()
        {
            Close();

            DialogueManager.StopAllConversations();
            SceneUI.Context.GameManager.TogglePauseGame();
            SceneUI.Context.GameManager.GoToMainMenu();
        }

        public void ButtonOptions()
        {
            Close();

            Open<OptionsUI>();
        }

    }
}
