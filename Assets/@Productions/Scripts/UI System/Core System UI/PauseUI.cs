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
            base.OnInitialize();

            SceneUI.Context.gameManager.OnGamePaused += GameManager_OnGamePaused;
            SceneUI.Context.gameManager.OnGameUnpaused += GameManager_OnGameUnpaused;
        }

        private void GameManager_OnGamePaused(object sender, EventArgs e)
        {
            Canvas.enabled = true;
        }

        private void GameManager_OnGameUnpaused(object sender, EventArgs e)
        {
            Canvas.enabled = false;
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
            
            SceneUI.Context.gameManager.TogglePauseGame();
        }

        public void ButtonGoToMainMenu()
        {
            Close();

            DialogueManager.StopAllConversations();
            SceneUI.Context.gameManager.TogglePauseGame();
            SceneManager.LoadScene(0);
        }

        public void ButtonOptions()
        {
            Close();

            Open<OptionsUI>();
        }

    }
}
