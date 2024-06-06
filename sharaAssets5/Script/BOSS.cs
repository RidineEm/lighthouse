using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BOSS : LivingEntity
{
    public Rigidbody2D target;
    public GameObject DamageText;
    public Transform textpro;
    public GameObject HpSilder;
    public GameObject SpikePrefab;

    public float SpikeSpeed = 5.0f; // 스파이크 속도
    public Transform SpikeSpawnPoint; // 스파이크가 발사될 위치

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
    public float RollDamage = 10f;
    public float RoarDamage = 5f;
    public float Speed = 1.5f;
    public float targetingRange = 50f;
    public float attackRange = 6f;
    public float Maxhealth = 100f;
    public float nextDamageTime = 0f;
    public float damageInterval = 0.5f; // 데미지를 주는 간격 (초 단위)

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
        Armour = 0f;
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

        // 랜덤으로 함수를 실행하여 공격을 실행함
        int randomChoice = Random.Range(0, 3);
        switch (randomChoice)
        {
            case 0:
                StartCoroutine(Roll());
                break;
            case 1:
                StartCoroutine(Roar());
                break;
            case 2:
                StartCoroutine(Spike());
                break;
        }
        // 5초 동안 대기한 후 다른 행동을 허용함
        yield return new WaitForSeconds(7.0f);
        print("5초 대기");
        isWaitingForAction = false;
    }
    IEnumerator Roll()  //roll공격 함수
    {
        isAttack = true;
        print("A 실행");
        EnemyAnimator.SetTrigger("RollAttackAnticipation");
        
        yield return new WaitForSeconds(2.06f); //애니메이션 길이 만큼 대기
        EnemyAnimator.ResetTrigger("RollAttackAnticipation");

        EnemyAnimator.SetTrigger("RollAttack"); 
        float RollAttackEndTime = Time.time + 3.0f; //time에 3초를 추가하여 RollAttackEndTimㄷ에 저장
        isRolling = true;
        nextDamageTime = Time.time;
        while (Time.time < RollAttackEndTime)      //RollAttackEndTime에 저장된 시간보다 현재 시간이 커질때까지 반복
        {
            Speed = 3.5f;
            EnemyAnimator.SetTrigger("RollAttack");
            float dirx = target.position.x - transform.position.x;
            float diry = target.position.y - transform.position.y;
            dirx = (dirx < 0) ? -1 : 1;                 //방향조절 dir의 x거리가 -라면 -1,아니면 1 + 속도 조절
            diry = (diry < 0) ? -1 : 1;
            transform.Translate(new Vector2(dirx, diry) * Speed * Time.deltaTime);
            yield return null; // 다음 프레임까지 대기
        }
        isRolling = false;
        EnemyAnimator.ResetTrigger("RollAttack");

        EnemyAnimator.SetTrigger("RollAttackRecoil");
        yield return new WaitForSeconds(0.1f);
        EnemyAnimator.ResetTrigger("RollAttackRecoil");
        Speed = 1.5f;
        isAttack = false;
        yield return true;
    }
    void OnCollisionEnter2D(Collision2D collision) // OnCollisionEnter2D와 OnCollisionExit2D는 충돌했을시 멈추고 떨어졌을시 다시 움직이도록 설계하였으나 실행 안됨
    { 
        if (isRolling && collision.gameObject.CompareTag("Player"))
        {
            rigid.velocity = Vector2.zero;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (isRolling && collision.gameObject.CompareTag("Player"))     //지금 Rolling 이고 player랑만 충돌검사 실시
        {
            if (Time.time >= nextDamageTime) //nextDamageTime 이라는 공격 딜레이를 주어 데미지 검사 실시
            {
                PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    rigid.velocity = Vector2.zero;
                    playerController.GetComponent<PlayerController>().OnDamage(RollDamage);  // 타겟이 데미지를 받는 메서드를 호출
                    nextDamageTime = Time.time + damageInterval; //damageInterval값과 time을 더하여 현재시간과 0.5초의 시간을 더한 값을 nextDamageTime에 저장하여 0.5초가 지난후에 다시 if문을 호줄하여 데미지를 주기위한 계산 식
                }
            }
        }
    }
    void OnCollisionExit2D(Collision2D collision) 
    {
        if (isRolling &&  collision.gameObject.CompareTag("Player"))
        {
            float dirx = target.position.x - transform.position.x;
            float diry = target.position.y - transform.position.y;
            dirx = (dirx < 0) ? -1 : 1;
            diry = (diry < 0) ? -1 : 1;
            rigid.velocity = new Vector2(dirx, diry) * Speed * Time.deltaTime;
        }
    }
    IEnumerator Roar()      //roar공격
    {
        print("B 실행");
        isAttack = true;
        EnemyAnimator.SetTrigger("RoarAnticipation");
        yield return new WaitForSeconds(1.1f);
        EnemyAnimator.ResetTrigger("RoarAnticipation");

        float RoarAttackEndTime = Time.time + 2.0f;
        nextDamageTime = Time.time;
        while (Time.time < RoarAttackEndTime)       //Roar공격은 2초동안 반복
        {
            EnemyAnimator.SetTrigger("Roar");
            Vector2 attackOffset = new Vector2(0.0f, 0.0f);
            float attackRange = 4.0f;
            Vector2 attackCenter = (Vector2)transform.position + (spriter.flipX ? new Vector2(-attackOffset.x, attackOffset.y) : attackOffset);
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackCenter, attackRange, playerLayers);
            foreach (Collider2D Player in hitEnemies)
            {
                if (Time.time >= nextDamageTime)
                {
                    Player.GetComponent<PlayerController>().OnDamage(RoarDamage);  // 타겟이 데미지를 받는 메서드를 호출
                    nextDamageTime = Time.time + 0.2f;
                }
            }
            yield return null; // 다음 프레임까지 대기
        }
        EnemyAnimator.ResetTrigger("Roar");

        EnemyAnimator.SetTrigger("RoarRecoil");
        yield return new WaitForSeconds(1.0f);
        EnemyAnimator.ResetTrigger("RoarRecoil");
        isAttack = false;
        yield return true;
    }
    private void OnDrawGizmosSelected()
    {
        if (spriter == null)
        {
            spriter = GetComponent<SpriteRenderer>();
        }
        //화면에 범위 표현
        Vector2 attackOffset = new Vector2(0.0f, 0.0f);
        float attackRange = 4.0f;
        Vector2 attackCenter = (Vector2)transform.position + (spriter.flipX ? new Vector2(-attackOffset.x, attackOffset.y) : attackOffset);

        Gizmos.DrawWireSphere(attackCenter, attackRange);
    }
    IEnumerator Spike()
    {
        print("C 실행");
        isAttack = true;
        EnemyAnimator.SetTrigger("SpikeAttackAnticipation");
        yield return new WaitForSeconds(0.8f);
        EnemyAnimator.ResetTrigger("SpikeAttackAnticipation");

        EnemyAnimator.SetTrigger("SpikeAttack");
        FireSpikes();
        yield return new WaitForSeconds(0.3f);
        EnemyAnimator.ResetTrigger("SpikeAttack");
        
        EnemyAnimator.SetTrigger("SpikeAttackRecoil");
        yield return new WaitForSeconds(0.9f);
        EnemyAnimator.ResetTrigger("SpikeAttackRecoil");
        isAttack = false;
        yield return true;
    }
    private void FireSpikes()
    {
        int numSpikes = 20; // 발사할 스파이크 수
        float angleStep = 360f / numSpikes; // 각 스파이크 간의 각도
        float angle = 0f;

        for (int i = 0; i < numSpikes; i++)
        {
            float dirX = Mathf.Sin((angle * Mathf.PI) / 180f);
            float dirY = Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector2 dir = new Vector2(dirX, dirY).normalized;

            GameObject spike = Instantiate(SpikePrefab, SpikeSpawnPoint.position, Quaternion.identity);
            SpikeProjectile spikeProjectile = spike.GetComponent<SpikeProjectile>();
            if (spikeProjectile != null)
            {
                spikeProjectile.SetMoveDirection(dir); // 방향 설정
            }

            angle += angleStep;
        }
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
