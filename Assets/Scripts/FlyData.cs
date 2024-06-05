using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/FlyData", fileName = "Fly Data")]

public class FlyData : ScriptableObject
{
    public float Maxhealth = 30f;
    public float AttackDamage = 15f;
    public float Armour = 0.0f;
    public float Speed = 1.0f;
    public float attackDelay = 1.0f;
    public float attackCooldown = 0f;
    public float targetingRange = 7.0f;
    public float attackRange = 5.0f;
}
