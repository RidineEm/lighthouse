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
        if (isLive) { return; } //사망시 움직이지 않음 테스트용으로 현재 true상태
        //위치간 거리 = 타겟위치 - 나의 위치
        Vector2 dirvec = target.position - rigid.position; 

        //방향 = 위치사이를 정규화 시킴
        Vector2 nextvec = dirvec.normalized * speed * Time.fixedDeltaTime;
        //이동
        rigid.MovePosition(rigid.position + nextvec);
        rigid.velocity = Vector2.zero;
    }

    public void TakeDamage(int damage)
    {
        //데미지 계산
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void LateUpdate()
    {
        // 나의 위치에 따라서 적이 방향을 돌림
        spriter.flipX = target.position.x > rigid.position.x;
    }

    void Die()
    {
        // 적의 사망 처리
        Destroy(gameObject);
    }
}
