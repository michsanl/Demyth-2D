using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    public class PauseUI : UIPageView
    {

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

            Player player = SceneUI.Context.Player;
            if (player.IsGamePaused)
            {
                SceneUI.Context.Player.ToggleGamePause();
            }
        }

        public void ButtonGoToMainMenu()
        {
            Close();
            Open<MainMenuUI>();

            SceneUI.Context.HUDUI.Close();

            var levelDestination = SceneUI.Context.LevelManager.GetLevelByID("Level 1");
            SceneUI.Context.LevelManager.SetLevel(levelDestination);

            SceneUI.Context.Player.ToggleGamePause();
        }

        public void ButtonOptions()
        {
            Close();

            Open<OptionsUI>();
        }

    }
}
