using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy => GetComponent<Enemy>();

    public override void TakeDamage(float damage, Transform damageDealer)
    {
        base.TakeDamage(damage, damageDealer);

        if (isDead)//如果已经死亡直接返回
            return;

        if (damageDealer.GetComponent<Player>() != null)
        {
            //当伤害造成者为player，改变enemy的状态
            enemy.TryEnterBattleState(damageDealer);
        }
    }
}
