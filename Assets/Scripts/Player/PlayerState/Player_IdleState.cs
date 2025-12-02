using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_IdleState : Player_GroundedState
{
    public Player_IdleState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        //每次进到该状态重置速度
        player.SetVelocity(0, rb.velocity.y);
    }
    public override void Update()
    {
        base.Update();

        if (player.moveInput.x == player.facingDir && player.wallDetected)
        {
            return;//防止player面向墙壁也能进入movestate
        }

        if (player.moveInput.x != 0)
        {
            stateMachine.ChangeState(player.moveState);
        }

      
    }
}
