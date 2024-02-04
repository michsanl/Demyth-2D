using UnityEngine;
using Core;
using Demyth.Gameplay;
using PixelCrushers;
using System;
using DG.Tweening;

public class GameManager : SceneService
{
    private GameStateService _gameStateService;
    private GameInputController _gameInputController;
    private GameInput _gameInput;

    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _gameInputController = SceneServiceProvider.GetService<GameInputController>();
        _gameInput = _gameInputController.GameInput;

        _gameStateService[GameState.Pause].onEnter += Pause_OnEnter;
        _gameStateService[GameState.Pause].onExit += Pause_OnExit;

        _gameInput.OnPausePerformed.AddListener(GameInput_OnPausePerformed);

        DOTween.timeScale = 1f;
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void SaveGameplayProgress()
    {
        SaveSystem.SaveToSlot(1);
    }

    public void SetGameStateToGameplay()
    {
        _gameStateService.SetState(GameState.Gameplay);
    }

    public void SetGameStateToGameOver()
    {
        _gameStateService.SetState(GameState.GameOver);
    }

    public void SetGameStateToGameEnd()
    {
        _gameStateService.SetState(GameState.GameEnd);
    }

    private void Pause_OnEnter(GameState state)
    {
        Pause();
    }

    private void Pause_OnExit(GameState state)
    {
        UnPause();
    }

    private void GameInput_OnPausePerformed()
    {
        ToggleGameStatePause();
    }

    private void ToggleGameStatePause()
    {
        if (_gameStateService.CurrentState != GameState.Pause)
        {
            _gameStateService.SetState(GameState.Pause);
        }
        else
        {
            _gameStateService.SetState(GameState.Gameplay);
        }
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    private void UnPause()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }
}
