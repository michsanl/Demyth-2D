using Core;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Demyth.Gameplay;
using System;

public class GameInputController : SceneService
{
    public GameInput GameInput => gameInput;

    [SerializeField] private GameInput gameInput;

    private GameStateService _gameStateService;

    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _gameStateService[GameState.MainMenu].onEnter += OnMainMenu;
        _gameStateService[GameState.Gameplay].onEnter += OnGameplay;
        _gameStateService[GameState.Pause].onEnter += OnPause;
        _gameStateService[GameState.GameOver].onEnter += OnGameOver;
    }
    
    private void Start()
    {
        DialogueManager.Instance.conversationStarted += DialogueManager_OnConversationStarted;
        DialogueManager.Instance.conversationEnded += DialogueManager_OnConversationEnded;
    }

    public void EnablePlayerInput()
    {
        gameInput.EnablePlayerInput();
    }

    public void DisablePlayerInput()
    {
        gameInput.DisablePlayerInput();
    }

    private void OnMainMenu(GameState obj)
    {
        gameInput.DisablePlayerInput();
        gameInput.DisablePauseInput();
    }

    private void OnGameplay(GameState obj)
    {
        gameInput.EnablePlayerInput();
        gameInput.EnablePauseInput();
    }

    private void OnPause(GameState state)
    {
        gameInput.DisablePlayerInput();
    }

    private void OnGameOver(GameState state)
    {
        gameInput.DisablePlayerInput();
        gameInput.DisablePauseInput();
    }
    
    private void DialogueManager_OnConversationStarted(Transform t)
    {
        gameInput.DisablePlayerInput();
    }

    private void DialogueManager_OnConversationEnded(Transform t)
    {
        if (_gameStateService.CurrentState == GameState.GameOver) return;

        gameInput.EnablePlayerInput();
    }
}
