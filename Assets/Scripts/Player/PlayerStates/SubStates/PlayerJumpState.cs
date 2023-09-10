using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int amountOfJumpsLeft;

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        Debug.Log("Can Jump: " + player.NumberOfJump.ToString());
        amountOfJumpsLeft = player.NumberOfJump;
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
        Debug.Log("Can Jump: " + (amountOfJumpsLeft > 0).ToString());
        return (amountOfJumpsLeft > 0);
    }

    public void ResetAmountOfJump()
    {
        amountOfJumpsLeft = player.NumberOfJump;
    }

    public void DecreaseAmountOfJumpLeft()
    {
        amountOfJumpsLeft--;
    }
}
