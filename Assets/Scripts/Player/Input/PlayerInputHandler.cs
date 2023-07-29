using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MovementInput { get;private set; }

    public void OnMoveInut(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
        Debug.Log(MovementInput);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Jump Pushed");
        }

        if (context.performed)
        {
            Debug.Log("Jump Held Down");
        }

        if (context.canceled)
        {
            Debug.Log("Jump inut released");
        }
    }
}
