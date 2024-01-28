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
    private GameInputController _inputController;
    private LoadingUI _loadingUI;
    private GameHUD _gameHUD;
    private bool _isRestarting;
    
    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _loadingUI = SceneServiceProvider.GetService<LoadingUI>();
        _inputController = SceneServiceProvider.GetService<GameInputController>();
        _gameHUD = SceneServiceProvider.GetService<GameHUD>();
    }

    private void OnEnable()
    {
        _gameStateService[GameState.Gameplay].onEnter += GamePlay_OnEnter;
    }

    private void OnDisable() 
    {
        _gameStateService[GameState.Gameplay].onEnter -= GamePlay_OnEnter;
    }

    public void RestartLevel()
    {
        StartCoroutine(RestartLevelCoroutine());
    }

    private void GamePlay_OnEnter(GameState state)
    {
        DOTween.CompleteAll();
        SaveSystem.LoadFromSlot(1);
        _gameStateService.SetState(GameState.Gameplay);
        _gameHUD.Open();
        // if (_isRestarting) return;
        // if (_gameStateService.CurrentState == GameState.Pause) return;
        // if (_gameStateService.PreviousState != GameState.GameOver)

        // _inputController.DisablePauseInput();
        // _inputController.DisablePlayerInput();

        // StartCoroutine(RestartLevelCoroutine());
    }

    public IEnumerator RestartLevelCoroutine()
    {
        _isRestarting = true;

        _loadingUI.OpenPage();
        yield return Helper.GetWaitForSeconds(_loadingUI.GetOpenPageDuration());
        
        DOTween.CompleteAll();
        SaveSystem.LoadFromSlot(1);
        _gameStateService.SetState(GameState.Gameplay);

        _inputController.EnablePlayerInput();

        _loadingUI.ClosePage();
        yield return Helper.GetWaitForSeconds(_loadingUI.GetOpenPageDuration());

        _inputController.EnablePauseInput();
        
        _isRestarting = false;
    }
}
