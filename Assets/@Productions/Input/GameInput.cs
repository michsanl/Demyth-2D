using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CustomTools.Core;
using PixelCrushers.DialogueSystem;

public class GameInput : SceneService
{
    public Action OnSenterPerformed;
    public Action OnHealthPotionPerformed;
    public Action OnPausePerformed;

    private PlayerInputActions playerInputActions;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Senter.performed += PlayerInputAction_OnSenterPerformed;
        playerInputActions.Player.HealthPotion.performed += PlayerInputAction_OnHealthPotionPerformed;
        playerInputActions.Player.Pause.performed += PlayerInputAction_OnPausePerformed;
        DialogueManager.Instance.conversationStarted += DialogueManager_OnConversationStarted;
        DialogueManager.Instance.conversationEnded += DialogueManager_OnConversationEnded;

        playerInputActions.Player.Enable();
    }

    private void DialogueManager_OnConversationStarted(Transform t)
    {
        playerInputActions.Player.Disable();
    }

    private void DialogueManager_OnConversationEnded(Transform t)
    {
        playerInputActions.Player.Enable();
    }

    private void PlayerInputAction_OnSenterPerformed(InputAction.CallbackContext context)
    {
        OnSenterPerformed?.Invoke();
    }

    private void PlayerInputAction_OnHealthPotionPerformed(InputAction.CallbackContext context)
    {
        OnHealthPotionPerformed?.Invoke();
    }

    private void PlayerInputAction_OnPausePerformed(InputAction.CallbackContext context)
    {
        OnPausePerformed?.Invoke();
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }
}
