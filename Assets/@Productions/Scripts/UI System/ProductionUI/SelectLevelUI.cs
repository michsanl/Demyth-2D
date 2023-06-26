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
            Open<MainMenuUI>();
        }

        public void ButtonGoToLevel(string levelID)
        {
            Close();
            //Open<HUDUI>();
            SceneUI.Context.HUDUI.Open();

            var levelDestination = SceneUI.Context.LevelManager.GetLevelByID(levelID);
            SceneUI.Context.LevelManager.ChangeLevel(levelDestination);
        }
    }
}
