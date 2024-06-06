using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterhealthSlider : MonoBehaviour
{
    public Slider Slider;
    public float maxHP;
    public float curHP;
    private BOSS boss;
    private ShortEnemy shortEnemy;
    private RangedEnemy rangedEnemy;
    void Start()
    {
        boss = GetComponentInParent<BOSS>();
        shortEnemy = GetComponentInParent<ShortEnemy>();
        rangedEnemy = GetComponentInParent<RangedEnemy>();

        if (boss != null)
        {
            maxHP = boss.Maxhealth;
        }
        else if (shortEnemy != null)
        {
            maxHP = shortEnemy.Maxhealth;
        }
        else if (rangedEnemy != null)
        {
            maxHP = rangedEnemy.Maxhealth;
        }
    }
    void Update()
    {
        if (boss != null)
        {
            curHP = boss.health;
        }
        else if (shortEnemy != null)
        {
            curHP = shortEnemy.health;
        }
        else if (rangedEnemy != null)
        {
            curHP = rangedEnemy.health;
        }
        if (maxHP > 0) // 나눗셈 오류를 방지하기 위해 maxHP가 0이 아닌지 확인
        {
            Slider.value = curHP / maxHP;
        }
    }
}
