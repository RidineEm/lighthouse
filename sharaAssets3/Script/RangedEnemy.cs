using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : LivingEntity
{
    public Rigidbody2D target;

    Rigidbody2D rigid;
    Collider2D Collider2D;
    SpriteRenderer spriter;
    Animator EnemyAnimator;
    WaitForFixedUpdate wait;
    GameObject GameObject;

    bool isAttack = false;
    //bool isAttacking = false;
    bool isMoving = false;

    public LayerMask playerLayers;
    public float Maxhealth = 25f;
    public float AttackDamage = 8f;
    public float Speed = 1.5f;
    public float attackDelay = 1.5f;
    public float attackCooldown = 0f;
    public float targetingRange = 6f;
    public float attackRange;
    void Start()
    {
        GameObject = GetComponent<GameObject>();
        rigid = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        EnemyAnimator = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
        health = Maxhealth;
        Vector2 attackSize = new Vector2(0.9f, 0.3f);
        attackRange = Vector2.SqrMagnitude(attackSize);
    }
    public void Setup(FlyData flyData)
    {
        Maxhealth = flyData.Maxhealth;
        health = flyData.Maxhealth;
        Armour = flyData.Armour;
        AttackDamage = flyData.AttackDamage;
        Speed = flyData.Speed;
        attackDelay = flyData.attackDelay;
        attackCooldown = flyData.attackCooldown;
        targetingRange = flyData.targetingRange;
    }
    void FixedUpdate()
    {
        if (dead || EnemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {  //����� �������� ���� or ���ݹ����� ����
            StopMoving();
            return;
        }
        float dis = Vector2.Distance(transform.position, target.position);      //�� ��ġ�� Ÿ���� �Ÿ��� �����

        if (dis <= targetingRange && dead == false) //  �νĹ��� �ȿ� ���� ���� �� �i�ư��� ������
        {
            Move();
            if (dis <= attackRange)
            {
                StopMoving(); // �̵� ����
                Cooldown();
                if (!isAttack && attackCooldown <= 0f) // ��ٿ��� ������ ���� ���� �ƴ� ���� ����
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
        // ��ٿ� ������Ʈ
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }
    }
    void Move()
    {
        if (!isMoving)
        {
            isMoving = true;
            EnemyAnimator.SetBool("Fly", true); // �̵� �ִϸ��̼� ���
            return;
        }
        float dirx = target.position.x - transform.position.x;      // 
        float diry = target.position.y - transform.position.y;      //
        dirx = (dirx < 0) ? -1 : 1;                 //�������� dir�� x�Ÿ��� -��� -1,�ƴϸ� 1 + �ӵ� ����
        diry = (diry < 0) ? -1 : 1;
        transform.Translate(new Vector2(dirx, diry) * Speed * Time.deltaTime);
        EnemyAnimator.SetBool("Fly", true);
        attackCooldown = attackDelay;
    }
    void StopMoving()
    {
        if (isMoving)
        {
            isMoving = false;
            EnemyAnimator.SetBool("Fly", false); // �̵� �ִϸ��̼� ����
        }
    }
    void LateUpdate()
    {
        // ���� ��ġ�� ���� ���� ������ ����
        spriter.flipX = target.position.x > rigid.position.x;
    }
    public void Attack()
    {
        if (!isAttack)
        {
            isAttack = true;
            rigid.velocity = Vector2.zero;
            Vector2 attackOffset = new Vector2(-0.5f, 0.0f);
            Vector2 attackSize = new Vector2(0.6f, 0.3f);
            //���� ���ݽ� �׿� �°� ���ݹ����� �������� �̵���Ŵ
            Vector2 attackCenter = (Vector2)transform.position + (spriter.flipX ? new Vector2(-attackOffset.x, attackOffset.y) : attackOffset);
            // ���� ���� ���� �÷��̾� ����
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackCenter, attackSize, 0f, playerLayers);
            StartCoroutine(PerformAttack());

            isAttack = false;
            EnemyAnimator.SetTrigger("Attack");
            foreach (Collider2D Player in hitEnemies)
            {
                Player.GetComponent<PlayerController>().OnDamage(AttackDamage);  // Ÿ���� �������� �޴� �޼��带 ȣ��
                print("���� ����");
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (spriter == null)
        {
            spriter = GetComponent<SpriteRenderer>();
        }
        //ȭ�鿡 ���� ǥ��
        Vector2 attackOffset = new Vector2(-0.5f, 0.0f);
        Vector2 attackSize = new Vector2(0.6f, 0.3f);
        Vector2 attackCenter = (Vector2)transform.position + (spriter.flipX ? new Vector2(-attackOffset.x, attackOffset.y) : attackOffset);

        Gizmos.DrawWireCube(attackCenter, attackSize);
    }
    IEnumerator PerformAttack()
    {
        attackCooldown = attackDelay;
        yield return new WaitForSeconds(attackDelay);
        EnemyAnimator.ResetTrigger("Attack");
        yield return new WaitForSeconds(0.5f); // ���� �ִϸ��̼� ��� �ð�
    }
    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
        if (health > 0 && dead == false)
        {
            StartCoroutine(HitAnimation()); // �´� �ִϸ��̼� ���
        }
        else // ü���� 0 �����̸� ���� ó��
        {
            Collider2D.enabled = false;     //�ݶ��̴� ��Ȱ��ȭ
            rigid.simulated = false;        // ������ٵ� ��Ȱ��ȭ
            spriter.sortingOrder = 1;       //sortingOrder�� �������� �ٸ� ������Ʈ�� ���ذ� ���� �ʵ��� ����
            EnemyAnimator.SetBool("Dead", true); //�׾����� �ִϸ��̼� Ȱ��ȭ
            dead = true;
            Die(); // ü���� 0 �����̸� ���� ó��
            print("�� ���");
        }
    }
    IEnumerator HitAnimation()
    {
        EnemyAnimator.SetTrigger("Hit");    // �´� �ִϸ��̼� Ʈ����
        yield return wait;                  // ���� ������������ ������
        EnemyAnimator.ResetTrigger("Hit");
    }
    void Die()
    {
        StartCoroutine(DieCoroutine());
    }
    IEnumerator DieCoroutine()
    {
        // �ִϸ��̼��� ���� ������ ��ٸ��ϴ�. ���⼭�� 2�ʸ� ��ٸ��ϴ�.
        yield return new WaitForSeconds(2.0f);
        // ������Ʈ�� �ı��մϴ�.
        Destroy(gameObject);
        print("�� ����");
    }
}
