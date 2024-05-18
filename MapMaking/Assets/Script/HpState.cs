using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpState : MonoBehaviour
{
    public Slider HpBar;
    // �÷��̾� �ִ�, �ּ� HP
    public float MaxHp = 100;
    public float CurHp = 100;
    // Start is called before the first frame update
    void Start()
    {
        // ���� ü�� ǥ��
        HpBar.value = (float)CurHp / (float)MaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHP();
    }

    private void UpdateHP()
    // �÷��̾� ü�� ���� �޼���
    {
        HpBar.value = Mathf.Lerp(HpBar.value, (float)CurHp / (float)MaxHp, Time.deltaTime * 10);
    }
}
