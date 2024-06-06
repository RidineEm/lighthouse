using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : LivingEntity
{
    public Rigidbody2D target;
    public GameObject DamageText;
    public Transform textpro;
    public GameObject HpSilder;

    Rigidbody2D rigid;
    Collider2D Collider2D;
    SpriteRenderer spriter;
    Animator EnemyAnimator;
    WaitForFixedUpdate wait;
    public GameObject FlyprojectilePrefab;

    bool isAttack = false;
    bool isMoving = false;

    public LayerMask playerLayers;
    public float Maxhealth;
    public float AttackDamage;
    public float Speed;
    public float attackDelay;
    public float attackCooldown;
    public float targetingRange;
    public float attackRange;
    public void Setup(FlyData flyData)
    {
        Maxhealth = flyData.Maxhealth;
        Armour = flyData.Armour;
        AttackDamage = flyData.AttackDamage;
        Speed = flyData.Speed;
        attackDelay = flyData.attackDelay;
        attackCooldown = flyData.attackCooldown;
        targetingRange = flyData.targetingRange;
        attackRange = flyData.attackRange;
    }
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            target = player.GetComponent<Rigidbody2D>();
        }
        rigid = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        EnemyAnimator = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
        health = Maxhealth;
    }
    void FixedUpdate()
    {
        if (dead || EnemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {  //사망시 움직이지 않음 or 공격받을시 멈춤
            StopMoving();
            return;
        }
        float dis = Vector2.Distance(transform.position, target.position);      //내 위치와 타켓의 거리를 계산함

        if (dis <= targetingRange && dead == false) //  인식범위 안에 적이 들어올 시 쫒아가기 시작함
        {
            Move();
            if (dis <= attackRange)
            {
                StopMoving(); // 이동 멈춤
                Cooldown();
                if (!isAttack && attackCooldown <= 0f) // 쿨다운이 끝나고 공격 중이 아닐 때만 공격
                {
                    Attack();
                }
            }
        }
        else
        {
            StopMoving();
            return;
        }
    }
    void Cooldown()
    {
        // 쿨다운 업데이트
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }
    }
    void Move()
    {
        if (!isMoving && dead == false)
        {
            isMoving = true;
            EnemyAnimator.SetBool("Fly", true); // 이동 애니메이션 재생
            return;
        }
        float dirx = target.position.x - transform.position.x;      // 
        float diry = target.position.y - transform.position.y;      //
        dirx = (dirx < 0) ? -1 : 1;                 //방향조절 dir의 x거리가 -라면 -1,아니면 1 + 속도 조절
        diry = (diry < 0) ? -1 : 1;
        transform.Translate(new Vector2(dirx, diry) * Speed * Time.deltaTime);
        attackCooldown = attackDelay;
    }
    void StopMoving()
    {
        if (isMoving && dead == false)
        {
            isMoving = false;
            EnemyAnimator.SetBool("Fly", false); // 이동 애니메이션 멈춤
        }
    }
    void LateUpdate()
    {
        // 나의 위치에 따라서 적이 방향을 돌림
        spriter.flipX = target.position.x > rigid.position.x;
    }
    public void Attack()
    {
        if (!isAttack && dead == false)
        {
            isAttack = true;
            rigid.velocity = Vector2.zero;
            StartCoroutine(PerformAttack());

            // 투사체 생성 및 발사
            GameObject projectile = Instantiate(FlyprojectilePrefab, transform.position, Quaternion.identity);
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.Launch(target.position); // 타겟의 정보를 발사체에게 넘김

            isAttack = false;
            EnemyAnimator.SetTrigger("Attack");
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (spriter == null)
        {
            spriter = GetComponent<SpriteRenderer>();
        }
        //화면에 범위 표현
        Vector2 attackOffset = new Vector2(0.0f, 0.0f);
        Vector2 attackCenter = (Vector2)transform.position + (spriter.flipX ? new Vector2(-attackOffset.x, attackOffset.y) : attackOffset);

        Gizmos.DrawWireSphere(attackCenter, attackRange);
    }
    IEnumerator PerformAttack()
    {
        attackCooldown = attackDelay;
        yield return new WaitForSeconds(attackDelay);
        EnemyAnimator.ResetTrigger("Attack");
        yield return new WaitForSeconds(0.5f); // 공격 애니메이션 재생 시간
    }
    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
        if (health > 0 && dead == false)
        {
            StartCoroutine(HitAnimation()); // 맞는 애니메이션 재생
            HpSilder.GetComponent<MonsterhealthSlider>().curHP = health;
            GameObject text = Instantiate(DamageText);
            text.transform.position = textpro.position;
            text.GetComponent<DamageText>().damage = damage;
        }
        else // 체력이 0 이하이면 죽음 처리
        {
            Collider2D.enabled = false;     //콜라이더 비활성화
            rigid.simulated = false;        // 리지드바디 비활성화
            spriter.sortingOrder = 1;       //sortingOrder를 내림으로 다른 오브젝트에 방해가 되지 않도록 변경
            EnemyAnimator.SetBool("Dead", true); //죽었을때 애니메이션 활성화
            dead = true;
            Die(); // 체력이 0 이하이면 죽음 처리
        }
    }
    IEnumerator HitAnimation()
    {
        EnemyAnimator.SetTrigger("Hit");    // 맞는 애니메이션 트리거
        yield return wait;                  // 다음 물리프레임의 딜레이
        EnemyAnimator.ResetTrigger("Hit");
    }
    void Die()
    {
        StartCoroutine(DieCoroutine());
    }
    IEnumerator DieCoroutine()
    {
        // 애니메이션이 끝날 때까지 기다림
        yield return new WaitForSeconds(2.0f);
        // 오브젝트 파괴
        Destroy(gameObject);
    }
}
