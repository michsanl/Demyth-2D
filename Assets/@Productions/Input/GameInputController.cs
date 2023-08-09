using System.Collections;
using System.Collections.Generic;
using CustomTools.Core;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using System;

public class GameInputController : SceneService
{
    [SerializeField] private GameInput gameInput;

    protected override void OnActivate()
    {
        Context.GameManager.OnGamePaused += GameManager_OnGamePaused;
        Context.GameManager.OnGameUnpaused += GameManager_OnGameUnPaused;

        Context.LevelManager.OnOpenMainMenu += LevelManager_OnOpenMainMenu;
        Context.LevelManager.OnOpenGameLevel += LevelManager_OnOpenGameLevel;

        Context.CameraShakeController.OnCameraShakeStart += CameraShakeController_OnCameraShakeStart;
        Context.CameraShakeController.OnCameraShakeEnd += CameraShakeController_OnCameraShakeEnd;

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

    private void LevelManager_OnOpenMainMenu()
    {
        gameInput.DisablePlayerInput();
        gameInput.DisablePauseInput();
    }

    private void LevelManager_OnOpenGameLevel()
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
