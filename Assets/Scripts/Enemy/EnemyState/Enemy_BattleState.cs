using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    private float lastTimeWasInBattle;


    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        UpdateBattleTimer();

        if (player == null)
        {
            player = enemy.GetPlayerReference();
        }
 

        if (ShouldRetreat())
        {
            //确保后撤时始终面向玩家
            rb.velocity = new Vector2(enemy.retreatVelocity.x * -DirectionToPlayer(), enemy.retreatVelocity.y);
            enemy.HandleFlip(DirectionToPlayer());
        }
    }

    public override void Update()
    {
        base.Update();
        //--- 1. 检测玩家是否仍在视野内 ---
        var hit = enemy.PlayerDetected();
        if (hit.collider != null)
        {
            UpdateBattleTimer();//记录上一次进入battle状态的时间 
        }

        if (BattleTimeIsOver())//判断战斗持续时间是否结束
        {
            Debug.Log("战斗时间结束，进入idle");
            stateMachine.ChangeState(enemy.idleState);
            
        }


        //--- 2. 更新玩家位置 ---
        //player = hit.transform;

        if (WithinAttackRange() && enemy.PlayerDetected())
        {
            stateMachine.ChangeState(enemy.attackState);
        }
        else
        {
            //朝player的位置移动
            enemy.SetVelocity(enemy.battleMoveSpeed * DirectionToPlayer(), rb.velocity.y);
        }
    }

    private void UpdateBattleTimer() => lastTimeWasInBattle = Time.time;

    private bool BattleTimeIsOver() => Time.time > lastTimeWasInBattle + enemy.battleTimeDuration;

    //得到player是否处在enemy的攻击范围内
    private bool WithinAttackRange() => DistanceToPlayer() < enemy.attackDistance;

    private bool ShouldRetreat() => DistanceToPlayer() < enemy.minRetreatDistance;

    private float DistanceToPlayer()
    {
        if (player == null)
        {
            return float.MaxValue;
        }

        return Mathf.Abs(player.position.x - enemy.transform.position.x);//计算距离的绝对值
    }

    private int DirectionToPlayer()//获取玩家的方向
    {
        if (player == null)
            return 0;

        //1表示玩家在右侧 -1表示在左侧
        return player.position.x > enemy.transform.position.x ? 1 : -1;

    }
}
