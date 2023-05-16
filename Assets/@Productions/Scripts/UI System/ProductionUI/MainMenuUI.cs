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

        public void ButtonPlay()
        {
            Close();
            Open<HUDUI>();
        }

        public void ButtonOption()
        {
            Close();
            // Open<OptionUI>();
        }

        public void ButtonQuit()
        {
            Application.Quit();
        }
    }
}
