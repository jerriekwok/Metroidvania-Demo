using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine//×´Ì¬¹ÜÀíÆ÷
{
    //µ±Ç°×´Ì¬
    public EntityState currentState { get; private set; }
    public bool canChangeState;

    //³õÊ¼»¯×´Ì¬
    public void Initialize(EntityState startState)
    {
        canChangeState = true;
        currentState = startState;
        currentState.Enter();
    }

    //ÇÐ»»×´Ì¬
    public void ChangeState(EntityState newState)
    {
        if (canChangeState == false)
            return;

        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void UpdateActiveState()
    {
        currentState.Update();
    }

    public void SwitchOffStateMachine() => canChangeState = false;
}
