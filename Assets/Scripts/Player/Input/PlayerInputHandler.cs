using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawMovementInput { get;private set; }
    public int NormalInputX { get; private set; }
    public int NormalInputY { get; private set; }

    public bool JumpInput { get; private set; }

    public void OnMoveInut(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();
        NormalInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        NormalInputY = (int)(RawMovementInput * Vector2.right).normalized.y;
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
        }

        if (context.performed)
        {
            
        }

        if (context.canceled)
        {
            
        }
    }

    public void ExitJumpInput()
    {
        JumpInput = false;
    }
}
