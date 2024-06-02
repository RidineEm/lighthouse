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
        startPosition = transform.position; // 시작 위치 저장
    }
    public void Launch(Vector2 target)
    {
        targetPosition = target;
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        // 이미지 좌우 반전
        if (direction.x < 0)
        {
            // 왼쪽을 향하는 경우
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            // 오른쪽을 향하는 경우
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    void Update()
    {
        // 타겟 방향으로 이동
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
        // 비행 거리 계산
        float distanceTraveled = Vector2.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxDistance)
        {
            Destroy(gameObject); // 최대 비행 거리 초과 시 파괴
            print("거리 초과");
            return;
        }
        // 투사체가 타겟에 도달했는지 확인
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            OnHitTarget();
        }
    }

    void OnHitTarget()
    {
        // 필요시 타겟에 데미지를 입힘
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        foreach (Collider2D player in hitEnemies)
        {
            player.GetComponent<PlayerController>().OnDamage(damage);
            print("공격 성공");
        }
        // 투사체 파괴
        Destroy(gameObject);
    }
}
