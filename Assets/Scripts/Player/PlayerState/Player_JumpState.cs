using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_JumpState : Player_AiredState
{
    public Player_JumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(rb.velocity.x, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

                                  //防止和跳斩状态下yVelocity的改变产生冲突
        if (rb.velocity.y < 0 && stateMachine.currentState != player.jumpAttackState)
        {
            //切换状态
            stateMachine.ChangeState(player.fallState);
        }
    }
}
