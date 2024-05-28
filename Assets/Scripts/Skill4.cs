using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill4 : MonoBehaviour
{
    public float speed = 10f;
    public float lifespan = 0.9f;
    Vector2 direction; // �Ѿ��� ����
    private Rigidbody2D bulletRigidbody;
    private bool speedChanged = false;

    void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        // ���� �� �Ѿ��� �������� ������ �ӵ��� �̵�
        bulletRigidbody.velocity = direction.normalized * speed;

        // ���� �ð��� ������ �Ѿ��� ��Ȱ��ȭ
        Invoke("DeactivateBullet", lifespan);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = direction.normalized * speed;
        }
    }

    void DeactivateBullet()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //�浹�� ������ ���̶��     
        if (other.tag == "Enemy")
        {
            //Monster�� Die()�� ȣ���ϱ� ����
            //monster ������Ʈ�� MonsterController��ũ��Ʈ ���� ����

            // MonsterController monsterController = other.GetComponent<MonsterController>();
            // ���� ��ũ��Ʈ�� ���⿡ �ϴ� �ּ�ó��

            // if (monsterController != null)
            // {
            //  obj.GetComponent<hpbar>().Damage();  ---> �̶� ��ų�� �������� if �⺻���� damage�� 10�Ͻ� ��ų�� ���� 2���� 20���� �����ϱ�

            //�ı�
            //Destroy(gameObject);
            //  }
        }

        //�浹�� ���� ���̶��
        if (other.tag == "Wall")
        {
            //�ı�
            Destroy(gameObject);
        }
    }
}
