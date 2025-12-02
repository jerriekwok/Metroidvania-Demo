using System.Collections;
using UnityEngine;
using System;

public class Player : Entity
{
    //事件
    public static event Action OnPlayerDeath;

    //定义行为
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_DeadState deadState { get; private set; }

    public Vector2 moveInput { get; private set; }
     
    [Header("Attack details")]
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration = .1f;//攻击时位移持续时间
    public float comboResetTime = 2;
    private Coroutine queuedAttackCo;//用于保存当前的攻击协程

    [Header("Move Detail")]
    public float moveSpeed;
    //private bool facingRight = true;
    //public int facingDir { get; private set; } = 1;
    public float jumpForce;
    public Vector2 wallJumpForce;//walljump的角度

    [Range(0, 1)]
    public float inAirMoveMultiplier = .7f;
    [Range(0, 1)]
    public float wallSlideSlowMultiplier = .7f;
    [Space]
    public float dashDuration = .25f;
    public float dashSpeed = 20;

    protected override void Awake()
    {

        base.Awake();
        idleState = new Player_IdleState(this, stateMachine, "idle");
        moveState = new Player_MoveState(this, stateMachine, "move");
        fallState = new Player_FallState(this, stateMachine, "jumpFall");
        jumpState = new Player_JumpState(this, stateMachine, "jumpFall");
        wallSlideState = new Player_WallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "jumpFall");
        dashState = new Player_DashState(this, stateMachine, "dash");
        basicAttackState = new Player_BasicAttackState(this, stateMachine, "basicAttack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "jumpAttack");
        deadState = new Player_DeadState(this, stateMachine, "dead");
    }

    protected override void Start()
    {
        //初始化状态机
        stateMachine.Initialize(idleState);

        GameInput.Instance.onPlayerMove += GameInput_onPlayerMove;
        GameInput.Instance.onPlayerIdle += GameInput_onPlayerIdle;
    }

    public override void EntityDeath()
    {
        base.EntityDeath();

        OnPlayerDeath?.Invoke();//触发死亡事件
        stateMachine.ChangeState(deadState);
    }

    #region 延迟一帧后进入下一个攻击状态
    public void EnterAttackStateWithDelay()
    {
        if (queuedAttackCo != null)
        {
            StopCoroutine(EnterAttackStateWithDelayCo());
        }
        queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
    }
    public IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame(); //等待这一帧即将渲染画面之前 再继续执行。
        stateMachine.ChangeState(basicAttackState);
    }
    #endregion

    private void GameInput_onPlayerIdle(object sender, System.EventArgs e)
    {
        moveInput = Vector2.zero;
    }

    private void GameInput_onPlayerMove(object sender, System.EventArgs e)
    {
        moveInput = GameInput.Instance.GetMoveValueNormailize();
        //Debug.Log(moveInput);

    }
}