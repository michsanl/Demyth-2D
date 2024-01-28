using System;
using Core;
using Demyth.Gameplay;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers;
using DG.Tweening;
using Lean.Pool;
using Demyth.UI;
using UISystem;
using System.Collections;

public class Level2RestartHandler : MonoBehaviour
{
    
    private GameStateService _gameStateService;
    private GameHUD _gameHUD;
    private bool _isRestarting;
    
    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _gameHUD = SceneServiceProvider.GetService<GameHUD>();
    }

    private void OnEnable()
    {
        _gameStateService[GameState.GameOver].onExit += GameOver_OnExit;
    }

    private void OnDisable() 
    {
        _gameStateService[GameState.GameOver].onExit -= GameOver_OnExit;
    }

    private void GameOver_OnExit(GameState state)
    {
        RestartLevel();
    }

    private void RestartLevel()
    {
        DOTween.CompleteAll();
        SaveSystem.LoadFromSlot(1);
        _gameStateService.SetState(GameState.Gameplay);
        _gameHUD.Open();
    }
}
