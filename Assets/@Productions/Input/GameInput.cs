using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public Action OnSenterPerformed;
    public Action OnHealthPotionPerformed;
    public Action OnPausePerformed;

    private PlayerInputActions playerInputActions;

    private void Awake() 
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Senter.performed += PlayerInputAction_OnSenterPerformed;
        playerInputActions.Player.HealthPotion.performed += PlayerInputAction_OnHealthPotionPerformed;
        playerInputActions.Player.Pause.performed += PlayerInputAction_OnPausePerformed;

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
