using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    private void Awake() 
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }
    
    // output vector nya normalized
    public Vector2 GetMovementVector ()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        
        return inputVector;
    }

    // output vector nya ga normalized
    public Vector2 GetMovementVectorPassThrough()
    {
        Vector2 inputVector = playerInputActions.Player.MovePassThrough.ReadValue<Vector2>();

        return inputVector;
    }
}
