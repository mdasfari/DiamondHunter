using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRamblingRopeState : PlayerTouchRamblingState
{
    public PlayerRamblingRopeState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}
