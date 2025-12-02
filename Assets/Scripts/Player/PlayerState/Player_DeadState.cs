using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DeadState : PlayerState
{
    public Player_DeadState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        input.Disable();//禁用输入
        rb.simulated = false;//关闭物理模拟
    }
}
