using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : LivingEntity
{
    private Rigidbody2D PlayerRigidbody;
    private Animator playerAnimator;
    private SpriteRenderer spriter;
    Collider2D Collider2D;
    public float speed = 5f;

    private bool isAttack = false;
    private bool isWalk = false;

    // 공격 범위와 데미지 체력
    public float attackDamage = 10f;
    public LayerMask enemyLayers;
    public float PlayerArmour = 0f;

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        Collider2D = GetComponent<Collider2D>();
        health = 100f;
        Armour = 0f;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    void Update()
    {
        if (dead) return;
        Attack();
    }
    private void FixedUpdate()
    {
        if (!isAttack || !dead)  // 공격중이 아니거나 죽지 않았을시 이동가능
        {
            Walk();
        }
    }
    private void Attack()  //공격 구현 및 애니메이션
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!isAttack)
            {
                isAttack = true;
                playerAnimator.SetBool("Attack", true);
                PlayerRigidbody.velocity = Vector2.zero;        //공격중에는 이동 불가
                //공격실행
                PerformAttack();
            }
        }

        // 애니메이션 상태 정보를 가져옴
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

        // 현재 애니메이션 상태가 'Attack'인지 확인
        if (isAttack && stateInfo.IsName("Attack"))
        {
            // 애니메이션이 끝났는지 확인
            if (stateInfo.normalizedTime >= 1.0f)
            {
                isAttack = false;
                playerAnimator.SetBool("Attack", false);
            }
        } 
    }
    private void PerformAttack()
    {
        //공격 범위 설정
        Vector2 attackOffset = new Vector2(0.5f, 0.0f);
        Vector2 attackSize = new Vector2(0.8f, 0.5f);
        //좌측 공격시 그에 맞게 공격범위를 좌측으로 이동시킴
        Vector2 attackCenter = (Vector2)transform.position + (spriter.flipX ? new Vector2(-attackOffset.x, attackOffset.y) : attackOffset);
        // 공격 범위 내의 적 감지
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackCenter, attackSize, 0f, enemyLayers);

        // 적에게 데미지 주기
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                // IDamageable 컴포넌트를 가진 적을 찾음
                IDamageable damageable = enemy.GetComponent<IDamageable>();
                // 적에게 데미지를 입히는 함수 호출
                damageable?.OnDamage(attackDamage);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (spriter == null)
        {
            spriter = GetComponent<SpriteRenderer>();
        }
        //화면에 범위 표현
        Vector2 attackOffset = new Vector2(0.5f, 0.0f);
        Vector2 attackSize = new Vector2(0.8f, 0.5f);
        Vector2 attackCenter = (Vector2)transform.position + (spriter.flipX ? new Vector2(-attackOffset.x, attackOffset.y) : attackOffset);

        Gizmos.DrawWireCube(attackCenter, attackSize);
    }
    private void Walk()  //플레이어 이동 구현 및 애니메이션
    {
        if (dead) return;

        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        float xSpeed = xInput * speed;
        float ySpeed = yInput * speed;

        //이동중 애니메이션 실행
        if (xInput != 0 || yInput != 0)
        {
            isWalk = true;
        }
        else
        {
            isWalk = false;
        }

        playerAnimator.SetBool("Walk", isWalk);

        Vector2 velocity = new Vector2(xSpeed, ySpeed);
        PlayerRigidbody.velocity = velocity;

        // 좌우반전을 이용하여 좌측 애니메이션 출력
        if (xInput != 0)
        {
            spriter.flipX = xSpeed < 0;
        }
    }
    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
        if (health > 0 && dead == false)
        {// 맞는 애니메이션 재생
            print("플레이어가 공격 받음");
        }
        else
        {
            dead = true;
            Die();
        }
    }
    public void Die()
    {
        // 아직 죽었을때 트리거는 없음
        playerAnimator.SetTrigger("Die");
        print(dead + "플레이어 사망");

        PlayerRigidbody.velocity = Vector2.zero;
    }
}
