using Core;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Demyth.Gameplay;
using System;

public class GameInputController : SceneService
{
    public GameInput GameInput => gameInput;

    [SerializeField] 
    private GameInput gameInput;

    private GameStateService _gameStateService;

    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _gameStateService[GameState.MainMenu].onEnter += OnMainMenu;
        _gameStateService[GameState.Gameplay].onEnter += OnGameplay;
    }

    private void Start()
    {
        DialogueManager.Instance.conversationStarted += DialogueManager_OnConversationStarted;
        DialogueManager.Instance.conversationEnded += DialogueManager_OnConversationEnded;
    }

    private void GameManager_OnGamePaused()
    {
        gameInput.DisablePlayerInput();
    }

    private void GameManager_OnGameUnPaused()
    {
        gameInput.EnablePlayerInput();
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

    private void CameraShakeController_OnCameraShakeStart()
    {
        gameInput.DisablePlayerInput();
    }

    private void CameraShakeController_OnCameraShakeEnd()
    {
        gameInput.EnablePlayerInput();
    }

    private void DialogueManager_OnConversationStarted(Transform t)
    {
        gameInput.DisablePlayerInput();
    }

    private void DialogueManager_OnConversationEnded(Transform t)
    {
        gameInput.EnablePlayerInput();
    }
}
