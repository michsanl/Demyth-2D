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
            SceneUI.Context.GameManager.StartGame("Level 1");
        }

        public void ButtonContinue()
        {
            mainButtonParent.SetActive(false);
            selectLevelButtonParent.SetActive(true);
        }

        public void ButtonOption()
        {
            Close();
            Open<OptionsUI>();
        }

        public void ButtonQuit()
        {
            Application.Quit();
        }

        public void ButtonGoToLevel(string levelID)
        {
            SceneUI.Context.GameManager.StartGame(levelID);

            mainButtonParent.SetActive(true);
            selectLevelButtonParent.SetActive(false);
        }
    }
}
