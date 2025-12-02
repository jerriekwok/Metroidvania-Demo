using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private const string PLAYER_ISGROUNDED = "IsGrounded";
    private const string PLAYER_JUMPFALL = "yVelocity";
    private const string PLAYER_IDLEMOVE = "xVelocity";
    private const string PLAYER_ATTACK = "Attack";
    [SerializeField] private Player player;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = player.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Vector2 Velocity = rb.velocity;
        animator.SetFloat(PLAYER_JUMPFALL, Velocity.y);
        animator.SetFloat(PLAYER_IDLEMOVE, Velocity.x);

        animator.SetBool(PLAYER_ISGROUNDED, player.GetIsGrounded());

    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger(PLAYER_ATTACK);
    }

    //被attack的动画事件引用
    private void DisableMovementAndJump() => player.EnableJumpAndMovement(false);

    private void EnableMovementAndJump() => player.EnableJumpAndMovement(true);

    public void DamageEnemies() => player.DamageEnemies();
}
