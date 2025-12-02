using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour , IDamageable
{
    private const string CHESTOPEN = "chestOpen";
    private Animator anim => GetComponentInChildren<Animator>();
    private Rigidbody2D rb => GetComponentInChildren<Rigidbody2D>();
    private Entity_VFX fx => GetComponent<Entity_VFX>();

    [Header("Open details")]
    [SerializeField] private Vector2 knockback;

    public void TakeDamage(float damage, Transform damageDealer)
    {
        //播放开箱动画
        fx.PlayOnDamageVfx();
        anim.SetBool(CHESTOPEN, true);

        rb.velocity = knockback;
        //绕Z轴随机旋转角度
        rb.angularVelocity = Random.Range(-200f, 200f);
            
    }

   
}
