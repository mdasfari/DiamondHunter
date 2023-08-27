using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    public Vector2 RawMovementInput { get; private set; }
    public int NormalInputX { get; private set; }
    public int NormalInputY { get; private set; }

    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }

    public bool AttackInput { get; private set; }

    public bool BombInput { get; private set; }

    [SerializeField]
    private float inputHoldTime = 0.2f;

    private float jumpInputStartTime;

    public bool GrabInput { get; private set; }

    private void Update()
    {
        checkHumpInputHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        if (Mathf.Abs(RawMovementInput.x) > 0.5f)
        {
            NormalInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        }
        else
        {
            NormalInputX = 0;
        }

        if (Mathf.Abs(RawMovementInput.y) > 0.5f)
        {
            NormalInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
        }
        else
        {
            NormalInputY = 0;
        }
    }


    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GrabInput = true;
        }

        if (context.canceled)
        {
            GrabInput = false;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if (context.performed)
        {

        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInput = true;
        }

        if (context.performed)
        {

        }

        if (context.canceled)
        {
            AttackInput = false;
        }
    }

    public void OnBombInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            BombInput = true;
        }

        if (context.performed)
        {

        }

        if (context.canceled)
        {
            BombInput = false;
        }
    }

    public void OnEscapeInput(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;

        if (gameManager.IsGamePaused)
            gameManager.ResumeGame();
        else
            gameManager.PauseGame();
    }

    public void ExitJumpInput()
    {
        JumpInput = false;
    }

    private void checkHumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
            JumpInput = false;
    }
}
