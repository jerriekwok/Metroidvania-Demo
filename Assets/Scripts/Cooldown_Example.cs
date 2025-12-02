using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown_Example : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private float redColorDuration = 1;

    public float currentTimeInGame;
    public float lastTimeWasDamaged;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        ChanceColorIfNeeded();
    }

    private void ChanceColorIfNeeded()
    {
        currentTimeInGame = Time.time;

        if (currentTimeInGame > lastTimeWasDamaged + redColorDuration)
        {
            if (sr.color != Color.white)
            {
                sr.color = Color.white;
            }
        }
    }

    public void TakeDamage()
    {
        sr.color = Color.red;
        lastTimeWasDamaged = Time.time;//记录受伤的时间戳
    }

   
}
