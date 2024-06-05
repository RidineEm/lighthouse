using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill3 : LivingEntity
{
    public float speed = 10f;
    public float lifespan = 1f;
    Vector2 direction; // 총알의 방향
    private Rigidbody2D skill3Rigidbody;
    private bool speedChanged = false;
    public float skillDamage = 20f; // 스킬의 데미지

    void Awake()
    {
        skill3Rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {

        // 일정 시간이 지나면 총알을 비활성화
        Invoke("DeactivateBullet", lifespan);
    }

    // 방향 설정 메서드
    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    void DeactivateBullet()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        //충돌한 것이 벽이라면
        if (other.tag == "Wall")
        {
            //파괴
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 상대방이 적이라면
        if (collision.gameObject.tag == "Enemy")
        {
            // IDamageable 컴포넌트를 가진 적을 찾음
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

            // 적에게 데미지를 입히는 함수 호출
            damageable?.OnDamage(skillDamage);
        }
    }

}
