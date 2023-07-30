using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    private int xInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool isJumping;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool stickJumpTime;
    private bool grabInput;

    public PlayerAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfWalled();
        isTouchingWallBack = player.CheckIfWalledBack();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        checkStickJump();

        xInput = player.InputHandler.NormalInputX;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        grabInput = player.InputHandler.GrabInput;


        CheckJumpMultiplier();

        if (isGrounded && player.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        else if (jumpInput && (isTouchingWall || isTouchingWallBack))
        {
            player.WallJumpState.DetectWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (jumpInput && player.JumpState.CanJump())
        {
            player.InputHandler.ExitJumpInput();
            stateMachine.ChangeState(player.JumpState);
        }
        else if (isTouchingWall && grabInput)
        {
            stateMachine.ChangeState(player.WallGrapState);
        }
        else if (isTouchingWall && xInput == player.FaceDirection && player.CurrentVelocity.y <= 0)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
        else
        {
            player.CheckFlipFace(xInput);
            player.SetVolcityX(playerData.movementVelocity * xInput);

            player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
            player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));
        }
    }

    private void CheckJumpMultiplier()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                player.SetVolcityY(player.CurrentVelocity.y * playerData.JumpHeightMult);
                isJumping = false;
            }
            else if (player.CurrentVelocity.y <= 0f)
            {
                isJumping = false;
            }
        }
    }

    private void checkStickJump()
    {
        if (stickJumpTime && Time.time > startTime + playerData.EdgeStickyJump)
        {
            stickJumpTime = false;
            player.JumpState.DecreaseAmountOfJumpLeft();
        }
    }

    public void StartStickJump()
    {
        stickJumpTime = true;
    }

    public void SetJumping()
    {
        isJumping = true;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
