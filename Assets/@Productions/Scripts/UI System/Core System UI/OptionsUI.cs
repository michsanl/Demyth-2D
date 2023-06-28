using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    public class OptionsUI : UIPageView
    {

        protected override void OnOpen()
        {
            Canvas.enabled = true;
        }

        protected override void OnClosed()
        {    
            Canvas.enabled = false;
        }

        public void ButtonBack()
        {
            Close();
            Open<MainMenuUI>();
        }

    }
}
