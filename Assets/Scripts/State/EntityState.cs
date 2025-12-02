using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityState //行为类，负责进入状态
{
    protected StateMachine stateMachine;
    protected string stateName;

    public EntityState(StateMachine stateMachine,string stateName)
    {
        this.stateMachine = stateMachine;
        this.stateName = stateName;
    }

    public virtual void Enter()
    {
        Debug.Log("I enter " + stateName);
    }

    public virtual void Update()
    {
        Debug.Log("I run update of " + stateName);
    }

    public virtual void Exit()
    {
        Debug.Log("I exit " + stateName);
    }
}
