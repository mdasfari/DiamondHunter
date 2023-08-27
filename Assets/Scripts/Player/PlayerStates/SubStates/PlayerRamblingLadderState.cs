﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRamblingLadderState : PlayerTouchRamblingState
{
    public PlayerRamblingLadderState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
