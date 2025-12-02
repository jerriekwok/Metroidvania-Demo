using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_GroundedState : EnemyState
{
    public Enemy_GroundedState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetected().collider != null)
        {
            //检测到玩家切换战斗模式
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
