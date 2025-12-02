using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_JumpAttackState : PlayerState
{
    private bool touchedGround;
    private const string JUMPATTACKTRIGGER = "jumpAttackTrigger";
    public Player_JumpAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        touchedGround = false;

        player.SetVelocity(player.jumpAttackVelocity.x * player.facingDir, player.jumpAttackVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (player.groundDetected && touchedGround == false)//touchedGround的作用只让trigger在状态内只能触发一次
        {
            touchedGround = true;
            anim.SetTrigger(JUMPATTACKTRIGGER);
            player.SetVelocity(0, rb.velocity.y);
        }

        if (triggerCalled && player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
