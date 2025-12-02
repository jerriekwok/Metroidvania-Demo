using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_GroundedState : PlayerState
{
    public Player_GroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Update()
    {
        base.Update();

        if (rb.velocity.y < 0 && player.groundDetected == false)
        {
            //ÇÐ»»×´Ì¬
            stateMachine.ChangeState(player.fallState);
        }

        if (input.Player.Jump.WasPressedThisFrame())
        {
            //ÇÐ»»×´Ì¬
            stateMachine.ChangeState(player.jumpState);
        }

        if (input.Player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.basicAttackState);
        }
    }
}
