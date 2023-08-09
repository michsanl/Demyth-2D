using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CustomTools.Core;

public class GameInput : SceneService
{
    [SerializeField] private bool enablePlayerOnStart = true;
    [SerializeField] private bool enablePauseOnStart = true;

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
        playerInputActions.Pause.Escape.performed += PlayerInputAction_OnEscapePerformed;

        if (enablePlayerOnStart)
            playerInputActions.Player.Enable();
        if (enablePauseOnStart)
            playerInputActions.Pause.Enable();
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


}
