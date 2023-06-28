using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomTools.Core;

public class GameManager : SceneService
{
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    public bool IsGamePaused => isGamePaused;

    private bool isGamePaused;

    protected override void OnActivate()
    {
        base.OnActivate();

        Context.gameInput.OnPausePerformed += GameInput_OnPausePerformed;
    }

    private void GameInput_OnPausePerformed()
    {
        TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            Context.VCamCameraShake.gameObject.SetActive(false);
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        } else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }


    
}
