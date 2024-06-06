using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    //public float startingHealth = 100f;
    //public float StartingArmour = 0.0f;
    public float health { get; protected set; } // ���� ü��
    public float Armour { get; protected set; } // ���� ����
    public bool dead { get; protected set; } // ��� ����
    protected virtual void OnEnable()
    {
        // ������� ���� ���·� ����
        dead = false;
        // ü���� ���� ü������ �ʱ�ȭ
        //health = startingHealth;
        //Armour = StartingArmour;
    }
    public virtual void OnDamage(float damage)
    {
        // ��������ŭ ü�� ����
        health -= (damage - Armour);
    }
    public virtual void RestoreHealth(float newHealth)
    {
        if (dead)
        {
            // �̹� ����� ��� ü���� ȸ���� �� ����
            return;
        }
        // ü�� �߰�
        health += newHealth;
    }
}
