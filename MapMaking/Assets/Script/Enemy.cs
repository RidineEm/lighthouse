using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1.0f;
    public Rigidbody2D target;

    bool isLive = true;
    Rigidbody2D rigid;
    SpriteRenderer spriter;

    public int health = 100;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (isLive) { return; } //����� �������� ���� �׽�Ʈ������ ���� true����
        //��ġ�� �Ÿ� = Ÿ����ġ - ���� ��ġ
        Vector2 dirvec = target.position - rigid.position; 

        //���� = ��ġ���̸� ����ȭ ��Ŵ
        Vector2 nextvec = dirvec.normalized * speed * Time.fixedDeltaTime;
        //�̵�
        rigid.MovePosition(rigid.position + nextvec);
        rigid.velocity = Vector2.zero;
    }

    public void TakeDamage(int damage)
    {
        //������ ���
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void LateUpdate()
    {
        // ���� ��ġ�� ���� ���� ������ ����
        spriter.flipX = target.position.x > rigid.position.x;
    }

    void Die()
    {
        // ���� ��� ó��
        Destroy(gameObject);
    }
}
