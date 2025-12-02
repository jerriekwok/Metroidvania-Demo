using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_MoveState : Player_GroundedState
{
    public Player_MoveState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Update()
    {
        base.Update();

       
        //ÐÐÎªµÄÇÐ»»
        if (player.moveInput.x == 0 || player.wallDetected)
        {
            stateMachine.ChangeState(player.idleState);
        }

        player.SetVelocity(player.moveInput.x * player.moveSpeed, rb.velocity.y);
    }
}
