using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WallSlideState : PlayerState
{
    public Player_WallSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
          
        HandleWallSlide();

        if (input.Player.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.wallJumpState);
        }

        

        if (player.wallDetected == false)
        {
            stateMachine.ChangeState(player.fallState);
        }

        if (player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);

            if (player.facingDir != player.moveInput.x)
            {
                player.Flip();//只有在按下与player朝向相反的按键才会转身
            }
            
        }
    }

    private void HandleWallSlide()//控制墙壁滑行
    {
        if (player.moveInput.y < 0)//根据输入的方向向量判断
        {
            player.SetVelocity(player.moveInput.x, rb.velocity.y);
        }
        else
        {
            player.SetVelocity(player.moveInput.x, rb.velocity.y * player.wallSlideSlowMultiplier);
        }
    }
}
