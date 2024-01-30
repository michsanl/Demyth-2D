using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.UI;
using UISystem;
using UnityEngine;

namespace Demyth.UI
{
    public class LevelResetUI : MonoBehaviour
    {

        
        private Level3RestartHandler _level3Restart;
        private Level5RestartHandler _level5Restart;
        private Level6RestartHandler _level6Restart;

        private void Awake()
        {
            _level3Restart = SceneServiceProvider.GetService<Level3RestartHandler>();
            _level5Restart = SceneServiceProvider.GetService<Level5RestartHandler>();
            _level6Restart = SceneServiceProvider.GetService<Level6RestartHandler>();

            _level3Restart.OnRestartHandlerEnabled += Level3Restart_OnRestartHandlerEnabled;
            _level3Restart.OnRestartHandlerDisabled += Level3Restart_OnRestartHandlerDisabled;
            _level5Restart.OnRestartHandlerEnabled += Level5Restart_OnRestartHandlerEnabled;
            _level5Restart.OnRestartHandlerDisabled += Level5Restart_OnRestartHandlerDisabled;
            _level6Restart.OnRestartHandlerEnabled += Level6Restart_OnRestartHandlerEnabled;
            _level6Restart.OnRestartHandlerDisabled += Level6Restart_OnRestartHandlerDisabled;
        }

        private void Start() 
        {
            Hide();
        }

        // this prevents weird null reference exception on game quit
        private void OnDestroy()
        {
            _level3Restart.OnRestartHandlerDisabled -= Level3Restart_OnRestartHandlerDisabled;
            _level5Restart.OnRestartHandlerDisabled -= Level5Restart_OnRestartHandlerDisabled;
            _level6Restart.OnRestartHandlerDisabled -= Level6Restart_OnRestartHandlerDisabled;
        }

        private void Level6Restart_OnRestartHandlerEnabled()
        {
            Show();
        }

        private void Level6Restart_OnRestartHandlerDisabled()
        {
            Hide();
        }

        private void Level3Restart_OnRestartHandlerEnabled()
        {
            Show();
        }

        private void Level3Restart_OnRestartHandlerDisabled()
        {
            Hide();
        }

        private void Level5Restart_OnRestartHandlerEnabled()
        {
            Show();
        }

        private void Level5Restart_OnRestartHandlerDisabled()
        {
            Hide();
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
