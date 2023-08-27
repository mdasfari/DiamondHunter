using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchRamblingState : PlayerState
{
    private float gravityScale;
    protected bool isGrounded;
    protected bool isTouchingRambling;
    protected bool isRambling;
    protected int xInput;
    protected int yInput;

    public PlayerTouchRamblingState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        gravityScale = player.rb.gravityScale;
        player.rb.gravityScale = 0;
        isRambling = true;
    }

    public override void Exit()
    {
        base.Exit();
        player.rb.gravityScale = gravityScale;
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void AnimationTriggerFinished()
    {
        base.AnimationTriggerFinished();
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.CheckIfGrounded();
        isTouchingRambling = player.CheckIfRambling();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormalInputX;
        yInput = player.InputHandler.NormalInputY;

        if (isRambling)
        {
            if (!isTouchingRambling)
            {
                if (isGrounded)
                    stateMachine.ChangeState(player.IdleState);
                else
                    stateMachine.ChangeState(player.AirState);
                isRambling = false;
            }

            if (isTouchingRambling)
            {
                if (isGrounded)
                    stateMachine.ChangeState(player.IdleState);
            }

            player.SetVolcityY(playerData.wallClimbVelocity * yInput);
        }
        else
        {
            if (isTouchingRambling && yInput > 0)
                isRambling = true;
        }

        player.SetVolcityX(playerData.wallClimbVelocity * xInput);

        if (yInput != 1 && !isExistingState)
        {
            /*
            if (player.ramplingType == RamplingTypes.Rope)
                stateMachine.ChangeState(player.RamblingRopeState);
            else if (player.ramplingType == RamplingTypes.Ladder)
                stateMachine.ChangeState(player.RamblingLadderState);
            */
            //stateMachine.ChangeState(player.RamblingRopeState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
