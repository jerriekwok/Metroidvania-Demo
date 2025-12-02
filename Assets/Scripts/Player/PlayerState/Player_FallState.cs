using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FallState : Player_AiredState
{
    public Player_FallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        //判断player是否在地面
        if (player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);
        }
        if (player.wallDetected)//判断player是否接触墙面
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
    }
}
