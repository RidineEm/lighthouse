using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/FrogData", fileName = "Frog Data")]
public class FrogData : ScriptableObject
{
    public float Maxhealth = 50f;
    public float AttackDamage = 10f;
    public float Armour = 0f;
    public float Speed = 1.0f;
    public float attackDelay = 2.0f;

    public Vector2 attackRange = new Vector2(1f, 1f);
}
