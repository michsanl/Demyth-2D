 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Cecil;
using Core;
using UnityEngine.Events;
using Demyth.Gameplay;
using PixelCrushers;

public class GameManager : SceneService
{
    public UnityEvent OnGamePaused = new();
    public UnityEvent OnGameUnpaused = new();
    public bool IsGamePaused => isGamePaused;

    private bool isGamePaused;

    private GameInputController _gameInputController;
    private GameInput _gameInput;

    private void Awake()
    {
        _gameInputController = SceneServiceProvider.GetService<GameInputController>();
        _gameInput = _gameInputController.GameInput;

        _gameInput.OnPausePerformed.AddListener(GameInput_OnPausePerformed);

        SaveSystem.SaveToSlot(0);
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
            OnGamePaused?.Invoke();
            Time.timeScale = 0f;
        } else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke();
        }
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;

        OnGameUnpaused?.Invoke();
    }
    
}
