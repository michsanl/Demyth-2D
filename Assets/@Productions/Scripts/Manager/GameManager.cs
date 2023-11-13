 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Cecil;
using Core;

public class GameManager : SceneService
{
    public Action OnGamePaused;
    public Action OnGameUnpaused;
    public bool IsGamePaused => isGamePaused;

    private bool isGamePaused;

    private void OnEnable()
    {
        //Context.GameInput.OnPausePerformed += GameInput_OnPausePerformed;
    }

    private void OnDisable()
    {
        //Context.GameInput.OnPausePerformed -= GameInput_OnPausePerformed;
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
            OnGamePaused?.Invoke();
        } else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke();
        }
    }
    
}
