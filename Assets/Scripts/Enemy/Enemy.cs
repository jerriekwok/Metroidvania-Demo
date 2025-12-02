using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    //定义状态
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    public Enemy_DeadState deadState; 

    [Header("Battle details")]
    public float battleMoveSpeed = 3;
    public float attackDistance = 2;
    public float battleTimeDuration = 5;//攻击状态的持续时间
    public float minRetreatDistance = 1;//最小撤退距离 ps:就是保持正确的攻击距离，不会和玩家重合
    public Vector2 retreatVelocity;
 
    [Header("Movement details")]
    public float idleTime = 2;
    public float moveSpeed = 1.4f;
    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1;//移动动画乘数

    [Header("Player detection")]//检测玩家
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance = 10;
    public Transform player { get; private set; }

    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;//订阅事件
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        //受到玩家死亡事件，切换状态
        stateMachine.ChangeState(idleState);
    }


    public override void EntityDeath()
    {
        base.EntityDeath();
        //切换至死亡状态
        stateMachine.ChangeState(deadState);
    }

    public void TryEnterBattleState(Transform player)
    {
        if (stateMachine.currentState == battleState || stateMachine.currentState == attackState)
            return;     
        this.player = player;
        stateMachine.ChangeState(battleState);
    }
   
    public Transform GetPlayerReference()
    {
        if (player == null)
            player = PlayerDetected().transform;
        return player;
    }

    public RaycastHit2D PlayerDetected()//检测玩家方法
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position,Vector2.right * facingDir ,playerCheckDistance,whatIsPlayer | whatIsGround);
        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer(nameof(Player)))
            return default;//什么都没检测到返回默认值
        
        return hit;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * playerCheckDistance),playerCheck.position.y));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * attackDistance), playerCheck.position.y));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * minRetreatDistance), playerCheck.position.y));
    }

   
}
