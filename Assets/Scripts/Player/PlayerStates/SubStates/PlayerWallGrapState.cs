
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrapState : PlayerTouchWallState
{
    private Vector2 currentHoldPosition;
    public PlayerWallGrapState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        currentHoldPosition = player.transform.position;
        HoldPlayerPosition();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        HoldPlayerPosition();

        if (!isExistingState)
        {
            if (yInput > 0)
            {
                stateMachine.ChangeState(player.WallClimbState);
            }
            else if (yInput < 0f || !grabInput)
            {
                stateMachine.ChangeState(player.WallSlideState);
            }
        }
    }

    private void HoldPlayerPosition()
    {
        player.transform.position = currentHoldPosition;
        player.SetVolcityX(0f);
        player.SetVolcityY(0f);
    }

}
