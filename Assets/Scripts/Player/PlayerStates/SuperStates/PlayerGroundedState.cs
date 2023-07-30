using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    private bool JumpInput;
    private bool GrabInput;
    private bool isGrounded;
    private bool isTouchWall;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.CheckIfGrounded();
        isTouchWall = player.CheckIfWalled();
    }

    public override void Enter()
    {
        base.Enter();
        player.JumpState.ResetAmountOfJump();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.InputHandler.NormalInputX;
        JumpInput = player.InputHandler.JumpInput;
        GrabInput = player.InputHandler.GrabInput;

        if (JumpInput && player.JumpState.CanJump())
        {
            player.InputHandler.ExitJumpInput();
            stateMachine.ChangeState(player.JumpState);
        }
        else if (!isGrounded)
        {
            player.AirState.StartStickJump();
            stateMachine.ChangeState(player.AirState);
        }
        else if (isTouchWall && GrabInput)
        {
            stateMachine.ChangeState(player.WallGrapState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
