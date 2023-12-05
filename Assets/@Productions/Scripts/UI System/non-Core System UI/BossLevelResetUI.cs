using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.UI;
using Demyth.Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class BossLevelResetUI : MonoBehaviour
{
    [SerializeField] private Button _retryButton;
    [SerializeField] private UIPage _uiPage;    
    
    private GameStateService _gameStateService;
    
    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _gameStateService[GameState.GameOver].onEnter += GameStateGameOver_OnEnter;
        _gameStateService[GameState.GameOver].onExit += GameStateGameOver_OnExit;

        _retryButton.onClick.AddListener(SetGamteStateToGameplay);
        
        gameObject.SetActive(false);
    }

    private void GameStateGameOver_OnEnter(GameState state)
    {
        gameObject.SetActive(true);
    }

    private void GameStateGameOver_OnExit(GameState state)
    {
        gameObject.SetActive(false);
    }

    private void SetGamteStateToGameplay()
    {
        _gameStateService.SetState(GameState.Gameplay);
    }


}
