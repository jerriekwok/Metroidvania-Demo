using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DashState : PlayerState
{
    private float originalGravityScale;//记录原始重力 
    private int dashDir;

    public Player_DashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        dashDir = player.moveInput.x != 0 ? (int)player.moveInput.x : player.facingDir;
        stateTimer = player.dashDuration;//设置计时器 控制冲刺持续时间

        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;//冲刺过程禁用重力
    }
    public override void Update()
    {
        base.Update();
        CancelDashIfNeeded();

        //设置速度
        player.SetVelocity(player.dashSpeed * dashDir, 0);
        
        if (stateTimer < 0)
        {
            if (player.groundDetected)
            {
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                stateMachine.ChangeState(player.fallState);
            }
        }
            
    }
    public override void Exit()
    {
        base.Exit();
        
        //重置速度
        player.SetVelocity(0, 0);
        rb.gravityScale = originalGravityScale;//恢复重力
    }

    private void CancelDashIfNeeded()
    {
        if (player.wallDetected)
        {
            if (player.groundDetected)
            {
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                stateMachine.ChangeState(player.wallSlideState);
            }
        }
       
    }
}
