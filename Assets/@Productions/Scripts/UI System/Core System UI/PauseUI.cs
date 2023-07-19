using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using DG.Tweening;

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
            SceneUI.Context.HUDUI.Close();

            DialogueManager.StopAllConversations();
            SceneUI.Context.GameManager.TogglePauseGame();

            Level mainMenuLevel = SceneUI.Context.LevelManager.MainMenuLevel;
            SceneUI.Context.LevelManager.SetLevel(mainMenuLevel);

            Open<MainMenuUI>();

            SceneUI.Context.Player.gameObject.SetActive(false);
 
            SceneUI.Context.CameraNormal.transform.DOKill();
            SceneUI.Context.CameraNormal.transform.localPosition = Vector3.zero;
        }

        public void ButtonOptions()
        {
            Close();

            Open<OptionsUI>();
        }

    }
}
