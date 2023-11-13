using Core;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class GameInputController : SceneService
{
    [SerializeField] private GameInput gameInput;

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
