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
        _gameStateService[GameState.MainMenu].onEnter += OnMainMenu_Enter;
        _gameStateService[GameState.MainMenu].onExit += OnMainMenu_Exit;
        _gameStateService[GameState.Pause].onEnter += OnPause_Enter;
        _gameStateService[GameState.Pause].onExit += OnPause_Exit;
        _gameStateService[GameState.GameOver].onEnter += OnGameOver_Enter;
        _gameStateService[GameState.GameOver].onExit += OnGameOver_Exit;
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

    private void OnMainMenu_Enter(GameState obj)
    {
        gameInput.DisablePlayerInput();
        gameInput.DisablePauseInput();
    }

    private void OnMainMenu_Exit(GameState obj)
    {
        gameInput.EnablePauseInput();
        gameInput.EnablePlayerInput();
    }

    private void OnPause_Enter(GameState state)
    {
        gameInput.DisablePlayerInput();
    }

    private void OnPause_Exit(GameState state)
    {
        if (DialogueManager.isConversationActive) return;

        gameInput.EnablePlayerInput();
    }

    private void OnGameOver_Enter(GameState state)
    {
        gameInput.DisablePlayerInput();
        gameInput.DisablePauseInput();
    }

    private void OnGameOver_Exit(GameState state)
    {
        gameInput.EnablePlayerInput();
        gameInput.EnablePauseInput();
    }
    
    private void DialogueManager_OnConversationStarted(Transform t)
    {
        gameInput.DisablePlayerInput();
    }

    private void DialogueManager_OnConversationEnded(Transform t)
    {
        if (_gameStateService.CurrentState == GameState.GameOver) return;
        if (_gameStateService.CurrentState == GameState.Pause) return;

        gameInput.EnablePlayerInput();
    }
}
