using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager; // Reference to the GameManager.

    public Vector2 RawMovementInput { get; private set; } // Raw movement input.
    public int NormalInputX { get; private set; } // Normalized X movement input.
    public int NormalInputY { get; private set; } // Normalized Y movement input.

    public bool JumpInput { get; private set; } // Flag for jump input.
    public bool JumpInputStop { get; private set; } // Flag for stopping jump input.

    [SerializeField]
    private float inputHoldTime = 0.2f; // Time to hold jump input.

    private float jumpInputStartTime; // Start time for jump input.

    public bool GrabInput { get; private set; } // Flag for grab input.

    private void Update()
    {
        checkHumpInputHoldTime(); // Check jump input hold time.
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>(); // Read raw movement input.

        // Normalize X movement input.
        NormalInputX = Mathf.Abs(RawMovementInput.x) > 0.5f ? (int)(RawMovementInput * Vector2.right).normalized.x : 0;

        // Normalize Y movement input.
        NormalInputY = Mathf.Abs(RawMovementInput.y) > 0.5f ? (int)(RawMovementInput * Vector2.up).normalized.y : 0;
    }

    public void OnGrabInput(InputAction.CallbackContext context)
    {
        GrabInput = context.started; // Set grab input flag on start.
        if (context.canceled) GrabInput = false; // Reset grab input flag on cancel.
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true; // Set jump input flag on start.
            JumpInputStop = false;
            jumpInputStartTime = Time.time; // Record jump input start time.
        }
        if (context.canceled) JumpInputStop = true; // Set jump input stop flag on cancel.
    }

    public void OnEscapeInput(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (gameManager.IsGamePaused) gameManager.ResumeGame(); // Resume game if paused.
        else gameManager.PauseGame(); // Pause game if not paused.
    }

    public void ExitJumpInput()
    {
        JumpInput = false; // Reset jump input flag.
    }

    private void checkHumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime) JumpInput = false; // Reset jump input flag if hold time exceeded.
    }
}
