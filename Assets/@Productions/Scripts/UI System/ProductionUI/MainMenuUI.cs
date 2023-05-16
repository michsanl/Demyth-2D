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
