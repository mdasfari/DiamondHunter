using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimbState : PlayerTouchWallState
{
    public PlayerWallClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        player.SetVolcityY(playerData.wallClimbVelocity);
        if(yInput != 1 && !isExistingState)
        {
            stateMachine.ChangeState(player.WallGrapState);
        }
    }
}
