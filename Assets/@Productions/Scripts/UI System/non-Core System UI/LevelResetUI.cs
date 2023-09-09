using System;
using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;

public class LevelResetUI : MonoBehaviour
{

    private void Start() 
    {
        LevelReset.OnAnyRestartLevelEnabled += LevelReset_OnAnyRestartLevelEnabled;
        LevelReset.OnAnyRestartLevelDisabled += LevelReset_OnAnyRestartLevelDisabled;

        Hide();
    }

    private void LevelReset_OnAnyRestartLevelEnabled()
    {
        Show();
    }

    private void LevelReset_OnAnyRestartLevelDisabled()
    {
        Hide();
    }

    private void OnDestroy() 
    {
        LevelReset.OnAnyRestartLevelEnabled -= LevelReset_OnAnyRestartLevelEnabled;
        LevelReset.OnAnyRestartLevelDisabled -= LevelReset_OnAnyRestartLevelDisabled;
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
