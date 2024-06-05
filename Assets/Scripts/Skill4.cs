using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill4 : LivingEntity
{
    public float speed = 10f;
    public float lifespan = 0.9f;
    Vector2 direction; // �Ѿ��� ����
    private Rigidbody2D bulletRigidbody;
    private bool speedChanged = false;
    public float skillDamage = 20f; // ��ų�� ������

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

    void OnTriggerEnter2D(Collider2D other)
    {

        //�浹�� ���� ���̶��
        if (other.tag == "Wall")
        {
            //�ı�
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹�� ������ ���̶��
        if (collision.gameObject.tag == "Enemy")
        {
            // IDamageable ������Ʈ�� ���� ���� ã��
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

            // ������ �������� ������ �Լ� ȣ��
            damageable?.OnDamage(skillDamage);
        }
    }
}
