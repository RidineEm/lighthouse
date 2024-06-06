using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.WebRequestMethods;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using UnityEditor.Experimental.GraphView;

public class ShortEnemy : LivingEntity
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

    bool isAttack = false;
    bool isMoving = false;

    public LayerMask playerLayers;
    public float AttackDamage;
    public float Speed;
    public float attackDelay;
    public float attackCooldown;
    public float targetingRange;
    public float attackRange;
    public float Maxhealth;
     public void Setup(FrogData frogData)
    {
        Maxhealth = frogData.Maxhealth;
        Armour = frogData.Armour;
        AttackDamage = frogData.AttackDamage;
        Speed = frogData.Speed;
        attackDelay = frogData.attackDelay;
        attackCooldown = frogData.attackCooldown;
        targetingRange = frogData.targetingRange;
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
        Vector2 attackSize = new Vector2(0.9f, 0.3f);
        attackRange = Vector2.SqrMagnitude(attackSize);
        health = Maxhealth;

    }
   

    void FixedUpdate()
    {
        if (dead || EnemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {  //����� �������� ���� or ���ݹ����� ����
            StopMoving();
            return;
        }
        float dis = Vector2.Distance(transform.position, target.position);  //�� ��ġ�� Ÿ���� �Ÿ��� �����

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
        if (!isMoving && dead == false)
        {
            isMoving = true;
            EnemyAnimator.SetBool("Run", true); // �̵� �ִϸ��̼� ���
            return;
        }
        float dirx = target.position.x - transform.position.x;      // 
        float diry = target.position.y - transform.position.y;      //
        dirx = (dirx < 0) ? -1 : 1;                 //�������� dir�� x�Ÿ��� -��� -1,�ƴϸ� 1 + �ӵ� ����
        diry = (diry < 0) ? -1 : 1;
        transform.Translate(new Vector2(dirx, diry) * Speed * Time.deltaTime);
        attackCooldown = attackDelay;
    }
    void StopMoving()
    {
        if (isMoving && dead == false)
        {
            isMoving = false;
            EnemyAnimator.SetBool("Run", false); // �̵� �ִϸ��̼� ����
        }
    }
    void LateUpdate()
    {
        // ���� ��ġ�� ���� ���� ������ ����
        spriter.flipX = target.position.x > rigid.position.x;
    }
    public void Attack()
    {
        if (!isAttack && dead == false)
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
            HpSilder.GetComponent<MonsterhealthSlider>().curHP = health;
            GameObject text = Instantiate(DamageText);
            text.transform.position = textpro.position;
            text.GetComponent<DamageText>().damage = damage;
        }
        else // ü���� 0 �����̸� ���� ó��
        {
            Collider2D.enabled = false;     //�ݶ��̴� ��Ȱ��ȭ
            rigid.simulated = false;        // ������ٵ� ��Ȱ��ȭ
            spriter.sortingOrder = 1;       //sortingOrder�� �������� �ٸ� ������Ʈ�� ���ذ� ���� �ʵ��� ����
            EnemyAnimator.SetBool("Dead", true); //�׾����� �ִϸ��̼� Ȱ��ȭ
            dead = true;
            Die(); // ü���� 0 �����̸� ���� ó��
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
        // �ִϸ��̼��� ���� ������ ��ٸ�
        yield return new WaitForSeconds(2.0f);
        // ������Ʈ �ı�
        Destroy(gameObject);
    }
}
