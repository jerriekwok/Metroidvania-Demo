using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    private float attackVelocityTimer; // 控制攻击中“冲刺位移”的持续时间
    private float lastTimeAttacked;    // 上一次攻击结束后记录的时间戳

    private const string BASCIATTACKINDEX = "basicAttackIndex";

    private const int FirstComboIndex = 1; // 第一段攻击在 Animator 中的 index
    private int comboIndex = 1;            // 当前要播放的连击段数
    private int comboLimit = 3;            // 最大连击段数（根据 attackVelocity 自动同步）

    private bool comboAttackQueued; // 是否在当前攻击中提前按下了下一次攻击键


    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        // combo 段数必须与 attackVelocity 数组一致，否则就调整
        if (comboLimit != player.attackVelocity.Length)
        {
            comboLimit = player.attackVelocity.Length;
            Debug.LogWarning("根据 attackVelocity 的数组长度，自动调整了 comboLimit");
        }
    }

    public override void Enter()
    {
        base.Enter();

        comboAttackQueued = false; // 每次进入攻击状态先重置

        ResetComboIndexIfNeeded(); // 若时间过长，comboIndex 会重置为 1

        IfWantToFlip(); // 攻击中根据输入方向决定是否翻转

        anim.SetInteger(BASCIATTACKINDEX, comboIndex); // 播放对应段数的攻击动画

        ApplyAttackVelocity(); // 给角色初始攻击位移（前冲）
    }


    public override void Update()
    {
        base.Update();

        HandleAttackVelocity(); // 控制攻击位移逐渐结束

        // 玩家在攻击中按下攻击键 → 标记“想连击”
        if (input.Player.Attack.WasPressedThisFrame())
        {
            QueueNextAttack();
        }

        // triggerCalled 由动画事件触发，表示“进入可连击窗口”
        // ❗ 由于动画事件与按键可能在同一帧触发，不能立刻切状态
        if (triggerCalled)
        {
            HandleStateExit();
        }
    }


    public override void Exit()
    {
        base.Exit();

        comboIndex++;                   // 攻击结束后进入下一段
        lastTimeAttacked = Time.time;   // 用于 combo 重置判断
    }


    /// <summary>
    /// 记录“想连击”的意图，但不立刻执行。
    /// 真正切换在动画事件触发后进行。
    /// </summary>
    private void QueueNextAttack()
    {
        // comboIndex 未达到上限才允许排队下一段
        if (comboIndex < comboLimit)
        {
            comboAttackQueued = true;
        }
    }


    /// <summary>
    /// 若距离上次攻击时间超过 comboResetTime，就从第一段重新开始。
    /// </summary>
    private void ResetComboIndexIfNeeded()
    {
        // 过长时间没攻击或 comboIndex 超限时重置
        if (comboIndex > comboLimit || Time.time > lastTimeAttacked + player.comboResetTime)
        {
            comboIndex = FirstComboIndex;
        }
    }


    /// <summary>
    /// 若玩家输入方向与角色朝向相反，攻击之前转向。
    /// </summary>
    private void IfWantToFlip()
    {
        if (player.moveInput.x * player.facingDir < 0)
        {
            player.Flip();
        }
    }


    /// <summary>
    /// 动画事件触发后决定退出当前状态的方式：
    /// 1. 如果按过攻击 → 延迟一帧进入下一段攻击
    /// 2. 否则 → 回 idle
    /// </summary>
    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            // ⚠ 不能在同一帧立刻切状态，否则 Animator 会冲突
            anim.SetBool(animBoolName, false);
            player.EnterAttackStateWithDelay(); // 下一帧再进入下一段攻击
        }
        else
        {
            // 没连击就回到 idle
            stateMachine.ChangeState(player.idleState);
        }
    }


    /// <summary>
    /// 控制攻击中冲刺位移的持续时间（逐渐取消）。
    /// </summary>
    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;

        // 计时结束 → 停止攻击位移
        if (attackVelocityTimer < 0)
        {
            player.SetVelocity(0, rb.velocity.y);
        }
    }


    /// <summary>
    /// 进入攻击时给玩家一个初始向前（或向上）位移，用于攻击手感。
    /// </summary>
    private void ApplyAttackVelocity()
    {
        // 当前段数的攻击位移
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];

        attackVelocityTimer = player.attackVelocityDuration; // 重新启动计时器

        // 根据角色朝向应用位移
        player.SetVelocity(attackVelocity.x * player.facingDir, attackVelocity.y);
    }
}
