using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private PlayerAnimator playerAnimator;


    [Header("Attack details")]
    [SerializeField] private float attackRadius;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask whatIsEnemy;

    [Header("Movement details")]
    [SerializeField] private float moveSpeed ;
    [SerializeField] private float jumpForce;
    private bool isMoving = false;
    private bool facingRight = true;
    private bool canMove = true;
    private bool canJump = true;

    [Header("Collision details")]
    [SerializeField] private float groundCheckDistance ;
    [SerializeField] private bool isGounded;
    [SerializeField] private LayerMask whatIsGround;

   

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameInput.Instance.OnJumpPressed += GameInput_OnJumpPressed;
        GameInput.Instance.OnAttackPressed += GameInput_OnAttackPressed;
    }

    private void GameInput_OnAttackPressed(object sender, System.EventArgs e)
    {
        TryToAttack();
    }

    private void GameInput_OnJumpPressed(object sender, System.EventArgs e)
    {
        TryToJump();
    }

    private void Update()
    {
        HandleCollision();
        HandleMoveMent();
        //HandleInput();
        HandleFlip();
    }

    public void DamageEnemies()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsEnemy);

        foreach (Collider2D enemy in enemyColliders)
        {
            enemy.GetComponent<Enemy>().TakeDamage();
        }
    }
    
    public void EnableJumpAndMovement(bool enable)
    {
        
        canMove = enable;
        canJump = enable;
    }//统一管理移动和跳跃的禁用

   

    private void HandleMoveMent()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        Vector2 moveDir = new Vector2(inputVector.x, 0f);
        if (canMove)
        {

            rb.velocity = new Vector2(moveSpeed * moveDir.x, rb.velocity.y);
            isMoving = moveDir != Vector2.zero;
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
            
      

    }

    private void HandleCollision()
    {
        //地面检测
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isGounded = hit.collider != null;
        
    }

    
    private void HandleFlip()
    {
        if (rb.velocity.x > 0 && facingRight == false)
        {
            //准备向右翻转但不面向右边时 翻转
            Flip();
        }else if(rb.velocity.x < 0 && facingRight == true)
        {
            Flip();
        }
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    private void TryToJump()
    {
        if (isGounded && canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        }

    }

    private void TryToAttack()
    {
        if (isGounded)
        {
            playerAnimator.PlayAttackAnimation();
   
        }

    }

    public bool GetIsWalking()
    {
        return isMoving;
    }

    public bool GetIsGrounded()
    {
        return isGounded;
    }

    //绘制可视化射线
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
