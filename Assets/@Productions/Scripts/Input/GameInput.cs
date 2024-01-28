using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Core;
using UnityEngine.Events;

public class GameInput : MonoBehaviour
{
    [SerializeField] private bool enablePlayerOnStart = true;
    [SerializeField] private bool enablePauseOnStart = true;

    public UnityEvent OnSenterPerformed = new();
    public UnityEvent OnHealthPotionPerformed = new();
    public UnityEvent OnPausePerformed = new();
    public UnityEvent OnRestartPerformed = new();

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Senter.performed += PlayerInputAction_OnSenterPerformed;
        playerInputActions.Player.HealthPotion.performed += PlayerInputAction_OnHealthPotionPerformed;
        playerInputActions.Pause.Escape.performed += PlayerInputAction_OnEscapePerformed;
        playerInputActions.Restart.RestartLevel.performed += PlayerInputAction_OnRestartPerformed;

        playerInputActions.Restart.Enable();
        if (enablePlayerOnStart)
            playerInputActions.Player.Enable();
        if (enablePauseOnStart)
            playerInputActions.Pause.Enable();
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public void EnablePlayerInput()
    {
        playerInputActions.Player.Enable();
    }

    public void DisablePlayerInput()
    {
        playerInputActions.Player.Disable();
    }

    public void EnablePauseInput()
    {
        playerInputActions.Pause.Enable();
    }

    public void DisablePauseInput()
    {
        playerInputActions.Pause.Disable();
    }

    public void EnableRestartInput()
    {
        playerInputActions.Restart.Enable();
    }

    public void DisableRestartInput()
    {
        playerInputActions.Restart.Disable();
    }

    private void PlayerInputAction_OnSenterPerformed(InputAction.CallbackContext context)
    {
        OnSenterPerformed?.Invoke();
    }

    private void PlayerInputAction_OnHealthPotionPerformed(InputAction.CallbackContext context)
    {
        OnHealthPotionPerformed?.Invoke();
    }

    private void PlayerInputAction_OnEscapePerformed(InputAction.CallbackContext context)
    {
        OnPausePerformed?.Invoke();
    }

    private void PlayerInputAction_OnRestartPerformed(InputAction.CallbackContext context)
    {
        OnRestartPerformed?.Invoke();
    }
}
