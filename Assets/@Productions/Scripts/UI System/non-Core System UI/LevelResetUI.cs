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

        private void Start() 
        {
            TuyulChaseCutscene.OnAnyTuyulLevelComplete += OnAnyTuyulLevelComplete;
            TuyulChaseLevelReset.OnAnyTuyulLevelResetEnabled += OnAnyLevelResetEnabled;
            TuyulChaseLevelReset.OnAnyTuyulLevelResetDisabled += OnAnyLevelResetDisabled;
            LevelReset.OnAnyRestartLevelEnabled += OnAnyLevelResetEnabled;
            LevelReset.OnAnyRestartLevelDisabled += OnAnyLevelResetDisabled;

            Hide();
        }

        private void OnAnyTuyulLevelComplete()
        {
            Hide();
        }

        private void OnAnyLevelResetEnabled()
        {
            Show();
        }

        private void OnAnyLevelResetDisabled()
        {
            Hide();
        }

        private void OnDestroy() 
        {
            TuyulChaseCutscene.OnAnyTuyulLevelComplete -= OnAnyTuyulLevelComplete;
            TuyulChaseLevelReset.OnAnyTuyulLevelResetEnabled -= OnAnyLevelResetEnabled;
            TuyulChaseLevelReset.OnAnyTuyulLevelResetDisabled -= OnAnyLevelResetDisabled;
            LevelReset.OnAnyRestartLevelEnabled -= OnAnyLevelResetEnabled;
            LevelReset.OnAnyRestartLevelDisabled -= OnAnyLevelResetDisabled;
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
