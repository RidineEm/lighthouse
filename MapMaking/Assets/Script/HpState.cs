using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpState : MonoBehaviour
{
    public Slider HpBar;
    // 플레이어 최대, 최소 HP
    public float MaxHp = 100;
    public float CurHp = 100;
    // Start is called before the first frame update
    void Start()
    {
        // 현재 체력 표시
        HpBar.value = (float)CurHp / (float)MaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHP();
    }

    private void UpdateHP()
    // 플레이어 체력 갱신 메서드
    {
        HpBar.value = Mathf.Lerp(HpBar.value, (float)CurHp / (float)MaxHp, Time.deltaTime * 10);
    }
}
