using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : EntityState//状态类 ：角色当前处在一个“行为模式”
{
    protected Player player;
    protected PlayerInputSet input;//输入系统

   

    public PlayerState(Player player,StateMachine stateMachine,string animBoolName): base(stateMachine,animBoolName)
    {
        this.player = player;

        anim = player.anim;
        rb = player.rb;
        input = GameInput.Instance.playerInput;
    }

    public override void Update()
    {
        base.Update();

        //Debug.Log("I run update of " + animBoolName);

        //任何状态下都可以进入dashState
        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            stateMachine.ChangeState(player.dashState);
        }
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        anim.SetFloat("yVelocity", rb.velocity.y);//jump&fall的动画

    }

    private bool CanDash()
    {
        if (player.wallDetected)
        {
            return false;
        }

        if (stateMachine.currentState == player.dashState)
        {
            return false;
        }

        return true;
    }
}
