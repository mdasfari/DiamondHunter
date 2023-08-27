using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWeaponState : PlayerState
{
    private bool isAttacking;

    public Transform originalBoundry;
    public float radius;

    public PlayerWeaponState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
        isAttacking = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAttacking)
        {
            player.StartCoroutine(DelayWeapon());
        }
        else
        {
            stateMachine.ChangeState(player.IdleState);
        }

        /*
        if (attackInput)
        {
        */
        /*
            if (!isAttacking)
            {
                isAttacking = true;
                player.StartCoroutine(DelayWeapon());
            }
            else
            {
                player.WeaponAnim.SetBool("attack", true);
                isAttacking = true;
            }
        */
            /*
        }
            */
        /*
        else
        {
            player.WeaponAnim.SetBool("attack", false);
            stateMachine.ChangeState(player.IdleState);
        }
        */
    }

    private IEnumerator DelayWeapon()
    {
        yield return new WaitForSeconds(playerData.weaponTime);
        isAttacking = false;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = originalBoundry == null ? Vector3.zero : originalBoundry.position;
        Gizmos.DrawWireSphere(position, radius);
    }
}
