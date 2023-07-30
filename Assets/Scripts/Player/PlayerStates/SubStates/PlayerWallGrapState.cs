
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrapState : PlayerTouchWallState
{
    public PlayerWallGrapState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
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
        player.SetVolcityX(0f);
        player.SetVolcityY(0f);

        if (yInput > 0)
        {
            stateMachine.ChangeState(player.WallClimbState);
        }
        else if (yInput < 0f || !grabInput)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
