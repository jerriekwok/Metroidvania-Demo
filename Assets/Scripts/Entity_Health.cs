using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Health : MonoBehaviour,IDamageable
{
    private Entity_VFX entityVfx;
    private Entity entity;

    [SerializeField] protected float maxHp = 100;
    [SerializeField] protected float currentHp;
    [SerializeField] protected bool isDead;

    [Header("On Damage Knockback")]//受伤时被击退
    [SerializeField] private float knockbackDuration = .2f;
    [SerializeField] private Vector2 onDamageKnockback = new Vector2(1.5f, 2.5f);

    [Header("重击退")]
    [Range(0,1)]
    [SerializeField] private float heavyDamageThreshold = .3f;//达到重击退所需损失的最大生命值百分比
    [SerializeField] private float heavyKnockbackDuration = .5f;
    [SerializeField] private Vector2 onHeavyDamageKnockback = new Vector2(7, 7);

    protected virtual void Awake()
    {
        currentHp = maxHp;
        entityVfx = GetComponent<Entity_VFX>();
        entity = GetComponent<Entity>();
    }
    //得到伤害造成者
    public virtual void TakeDamage(float damage,Transform damageDealer)
    {
        if (isDead)
        {
            return;
        }
        entityVfx?.PlayOnDamageVfx();
        entity.ReciveKnockback(CalculateKnockback(damage,damageDealer), CalculateDuration(damage));
        ReduceHp(damage);
    }

    protected void ReduceHp(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        entity.EntityDeath();//切换至死亡状态
    }

    private Vector2 CalculateKnockback(float damage,Transform damageDealer)
    {

        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        Vector2 knockback = IsHeavyDamage(damage) ? onHeavyDamageKnockback : onDamageKnockback;
        knockback.x *= direction;

        return knockback;
    }

    private float CalculateDuration(float damage)
    {
        return IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;
    }

    //判断是否为重击退
    private bool IsHeavyDamage(float damage) => damage / maxHp >= heavyDamageThreshold;
}
