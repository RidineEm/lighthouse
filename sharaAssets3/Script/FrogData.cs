using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/FrogData", fileName = "Frog Data")]

public class FrogData : ScriptableObject
{
    public float Maxhealth = 50f;
    public float AttackDamage = 10f;
    public float Armour = 1.0f;
    public float Speed = 1.0f;
    public float attackDelay = 1.0f;
    public float attackCooldown = 0f;
    public float targetingRange = 5f;
    public Vector2 attackSize = new Vector2(0.9f, 0.3f);
}
