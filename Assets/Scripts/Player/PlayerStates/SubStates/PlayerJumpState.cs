using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int amountOfJumpsLeft;

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        amountOfJumpsLeft = playerData.amountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVolcityY(playerData.jumpVelocity);
        isAbilityDone = true;
        DecreaseAmountOfJumpLeft();
        player.AirState.SetJumping();
    }

    public bool CanJump()
    {
        return (amountOfJumpsLeft > 0);
    }

    public void ResetAmountOfJump()
    {
        amountOfJumpsLeft = playerData.amountOfJumps;
    }

    public void DecreaseAmountOfJumpLeft()
    {
        amountOfJumpsLeft--;
    }
}
