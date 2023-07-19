using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    public class MainMenuUI : UIPageView
    {
        [SerializeField] private GameObject mainButtonParent;
        [SerializeField] private GameObject selectLevelButtonParent;

        protected override void OnOpen()
        {
            Canvas.enabled = true;
        }

        protected override void OnClosed()
        {    
            Canvas.enabled = false;
        }

        public void ButtonNewGame()
        {
            Close();

            Level firstLevel = SceneUI.Context.LevelManager.GetLevelByID("Level 1");
            SceneUI.Context.LevelManager.SetLevel(firstLevel);
            SceneUI.Context.HUDUI.Open();

            SceneUI.Context.Player.ActivatePlayer();
        }

        public void ButtonGoToLevel(string levelID)
        {
            Close();

            Level targetLevel = SceneUI.Context.LevelManager.GetLevelByID(levelID);
            SceneUI.Context.LevelManager.SetLevel(targetLevel);
            SceneUI.Context.HUDUI.Open();

            SceneUI.Context.Player.ActivatePlayer();

            mainButtonParent.SetActive(true);
            selectLevelButtonParent.SetActive(false);
        }

        public void ButtonContinue()
        {
            mainButtonParent.SetActive(false);
            selectLevelButtonParent.SetActive(true);
        }

        public void ButtonOption()
        {

        }

        public void ButtonQuit()
        {
            Application.Quit();
        }
    }
}
