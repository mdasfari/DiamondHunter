using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using static UnityEngine.UISystemProfilerApi;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    protected int yInput;
    private bool JumpInput;
    private bool GrabInput;
    private bool AttackInput;
    private bool isGrounded;
    private bool isTouchWall;
    private bool isTouchRambler;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.CheckIfGrounded();
        isTouchWall = player.CheckIfWalled();
        isTouchRambler = player.CheckIfRambling();
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
        yInput = player.InputHandler.NormalInputY;
        JumpInput = player.InputHandler.JumpInput;
        GrabInput = player.InputHandler.GrabInput;
        AttackInput = player.InputHandler.AttackInput;

        if (AttackInput)
        {
            stateMachine.ChangeState(player.WeaponState);
        }
        if (JumpInput && player.JumpState.CanJump())
        {
            player.InputHandler.ExitJumpInput();
            stateMachine.ChangeState(player.JumpState);
        }
        else if (isTouchRambler && yInput == 1f)
        {
            switch (player.ramplingType)
            {
                case RamplingTypes.Rope:
                    stateMachine.ChangeState(player.RamblingRopeState);
                    break;
                case RamplingTypes.Ladder:
                    stateMachine.ChangeState(player.RamblingLadderState);
                    break;
            }
        }
        else if (!isGrounded)
        {
            player.AirState.StartStickJump();
            stateMachine.ChangeState(player.AirState);
        }
        else if (isTouchWall && GrabInput && !isTouchRambler)
        {
            stateMachine.ChangeState(player.WallGrapState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
