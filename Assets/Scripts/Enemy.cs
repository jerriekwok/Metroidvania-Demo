using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected string enemyName;


    private void Update()
    {
        
    }

    private void MoveAround()
    {

    }

    protected virtual void Attack()
    {

    }

    public void TakeDamage()
    {

    }
}
