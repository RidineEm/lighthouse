using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BOSS : LivingEntity
{
    public Rigidbody2D target;

    Rigidbody2D rigid;
    Collider2D Collider2D;
    SpriteRenderer spriter;
    Animator EnemyAnimator;
    WaitForFixedUpdate wait;

    bool isMoving = false;
    bool isRolling = false;
    bool isAttack = false;
    private bool isWaitingForAction = false;

    public LayerMask playerLayers;
    public float AttackDamage = 10f;
    public float Speed = 1.0f;
    public float attackDelay;
    public float attackCooldown;
    public float targetingRange = 10f;
    public float attackRange = 6f;
    public float Maxhealth;
    public float nextDamageTime = 0f;
    public float damageInterval = 0.5f; // 데미지를 주는 간격 (초 단위)


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        EnemyAnimator = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
    }
    void FixedUpdate()
    {
        if (dead || EnemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit") || isWaitingForAction)
        {  //사망시 움직이지 않음 or 공격받을시 멈춤 or 공격중일때
            StopMoving();
            return;
        }
        float dis = Vector2.Distance(transform.position, target.position);  //내 위치와 타켓의 거리를 계산함

        if (dis <= targetingRange && !dead && !isWaitingForAction) //  인식범위 안에 적이 들어올 시 쫒아가기 시작함
        {
            Move();

            if (dis <= attackRange && !dead)
            {
                StopMoving();
                if (!isWaitingForAction)
                {
                    StartCoroutine(SelectAndExecuteRandomAction());
                }
            }
        }
        else
        {
            StopMoving();
            return;
        }
    }
    void Move()
    {
        if (!isMoving && !dead)
        {
            isMoving = true;
            EnemyAnimator.SetBool("Walk", true); // 이동 애니메이션 재생
            return;
        }
        float dirx = target.position.x - transform.position.x;      
        float diry = target.position.y - transform.position.y;      
        dirx = (dirx < 0) ? -1 : 1;                 //방향조절 dir의 x거리가 -라면 -1,아니면 1 + 속도 조절
        diry = (diry < 0) ? -1 : 1;
        transform.Translate(new Vector2(dirx, diry) * Speed * Time.deltaTime);
    }
    void StopMoving()
    {
        if (isMoving && !dead)
        {
            isMoving = false;
            EnemyAnimator.SetBool("Walk", false); // 이동 애니메이션 멈춤
        }
    }
    void LateUpdate()
    {
        // 나의 위치에 따라서 적이 방향을 돌림
        spriter.flipX = target.position.x > rigid.position.x;
    }
    IEnumerator SelectAndExecuteRandomAction()
    {
        StopMoving();
        isWaitingForAction = true;

        // 랜덤으로 함수 A, B, 또는 C 중 하나를 선택하여 실행
        int randomChoice = Random.Range(0, 0);
        switch (randomChoice)
        {
            case 0:
                //yield return StartCoroutine(RollAttack());
                StartCoroutine(Roll());
                break;
            case 1:
                B();
                break;
            case 2:
                C();
                break;
        }
        // 5초 동안 대기한 후 다른 행동을 허용함
        yield return new WaitForSeconds(8.0f);
        print("5초 대기");
        isWaitingForAction = false;
    }
    IEnumerator Roll()
    {
        isAttack = true;
        print("A 실행");
        EnemyAnimator.SetTrigger("RollAttackAnticipation");
        print("1번");
        
        //yield return StartCoroutine(WaitForAnimation("RollAttackAnticipation"));
        yield return new WaitForSeconds(2.06f);
        EnemyAnimator.ResetTrigger("RollAttackAnticipation");

        EnemyAnimator.SetTrigger("RollAttack"); //RollAttack
        float RollAttackEndTime = Time.time + 3.0f;
        isRolling = true;
        nextDamageTime = Time.time;
        while (Time.time < RollAttackEndTime)
        {
            Speed = 3.0f;
            EnemyAnimator.SetTrigger("RollAttack");
            float dirx = target.position.x - transform.position.x;
            float diry = target.position.y - transform.position.y;
            dirx = (dirx < 0) ? -1 : 1;                 //방향조절 dir의 x거리가 -라면 -1,아니면 1 + 속도 조절
            diry = (diry < 0) ? -1 : 1;
            transform.Translate(new Vector2(dirx, diry) * Speed * Time.deltaTime);
            print("2번");
            yield return null; // 다음 프레임까지 대기
        }
        isRolling = false;
        EnemyAnimator.ResetTrigger("RollAttack");
        EnemyAnimator.SetTrigger("RollAttackRecoil");
        print("3번");
        yield return new WaitForSeconds(0.1f);
        EnemyAnimator.ResetTrigger("RollAttackRecoil");
        Speed = 1.0f;
        isAttack = false;
        yield return true;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isRolling && collision.gameObject.CompareTag("Player"))
        {
            print("부딪힘");
            rigid.velocity = Vector2.zero;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (isRolling && collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= nextDamageTime)
            {
                PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    rigid.velocity = Vector2.zero;
                    playerController.GetComponent<PlayerController>().OnDamage(AttackDamage);  // 타겟이 데미지를 받는 메서드를 호출
                    nextDamageTime = Time.time + damageInterval;
                }
            }
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (isRolling &&  collision.gameObject.CompareTag("Player"))
        {
            print("떨어짐");
            float dirx = target.position.x - transform.position.x;
            float diry = target.position.y - transform.position.y;
            dirx = (dirx < 0) ? -1 : 1;
            diry = (diry < 0) ? -1 : 1;
            rigid.velocity = new Vector2(dirx, diry) * Speed * Time.deltaTime;
        }
    }
    void B()
    {
        print("B 실행");
    }
    void C()
    {
        print("C 실행");
    }
    public override void OnDamage(float damage)
    {
        if (isAttack)// 공격 중이면 데미지를 받지 않음
        {
            return;
        }
        base.OnDamage(damage);
        if (health > 0 && !dead)
        {
            StartCoroutine(HitAnimation()); // 맞는 애니메이션 재생
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
        StopMoving();
        StartCoroutine(DieCoroutine());
    }
    IEnumerator DieCoroutine()
    {
        // 애니메이션이 끝날 때까지 기다림
        yield return new WaitForSeconds(4.0f);
        // 오브젝트 파괴
        Destroy(gameObject);
    }
}
