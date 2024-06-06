using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    //public float startingHealth = 100f;
    //public float StartingArmour = 0.0f;
    public float health { get; protected set; } // 현재 체력
    public float Armour { get; protected set; } // 현재 방어력
    public bool dead { get; protected set; } // 사망 상태
    protected virtual void OnEnable()
    {
        // 사망하지 않은 상태로 시작
        dead = false;
        // 체력을 시작 체력으로 초기화
        //health = startingHealth;
        //Armour = StartingArmour;
    }
    public virtual void OnDamage(float damage)
    {
        // 데미지만큼 체력 감소
        health -= (damage - Armour);
    }
    public virtual void RestoreHealth(float newHealth)
    {
        if (dead)
        {
            // 이미 사망한 경우 체력을 회복할 수 없음
            return;
        }
        // 체력 추가
        health += newHealth;
    }
}
