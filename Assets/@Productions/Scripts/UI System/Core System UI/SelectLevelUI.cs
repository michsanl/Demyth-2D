using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    public class SelectLevelUI : UIPageView
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
        }

        public void ButtonGoToLevel(string levelID)
        {
            Close();
        }
    }
}
