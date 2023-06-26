using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    public class MainMenuUI : UIPageView
    {

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

            SceneUI.Context.HUDUI.Open();

            // Go to level 1

            var levelDestination = SceneUI.Context.LevelManager.GetLevelByID("Level 1");
            SceneUI.Context.LevelManager.ChangeLevel(levelDestination);
        }

        public void ButtonContinue()
        {
            Close();
            Open<SelectLevelUI>();
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
    }
}
