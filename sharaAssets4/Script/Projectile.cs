using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 3.0f;
    public float damage = 8f;
    public float maxDistance = 15f;
    private Vector2 startPosition;
    private Vector2 targetPosition;

    void Start()
    {
        startPosition = transform.position; // ���� ��ġ ����
    }
    public void Launch(Vector2 target)
    {
        targetPosition = target;
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        // �̹��� �¿� ����
        if (direction.x < 0)
        {
            // ������ ���ϴ� ���
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            // �������� ���ϴ� ���
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    void Update()
    {
        // Ÿ�� �������� �̵�
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
        // ���� �Ÿ� ���
        float distanceTraveled = Vector2.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxDistance)
        {
            Destroy(gameObject); // �ִ� ���� �Ÿ� �ʰ� �� �ı�
            print("�Ÿ� �ʰ�");
            return;
        }
        // ����ü�� Ÿ�ٿ� �����ߴ��� Ȯ��
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            OnHitTarget();
        }
    }

    void OnHitTarget()
    {
        // �ʿ�� Ÿ�ٿ� �������� ����
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        foreach (Collider2D player in hitEnemies)
        {
            player.GetComponent<PlayerController>().OnDamage(damage);
            print("���� ����");
        }
        // ����ü �ı�
        Destroy(gameObject);
    }
}
