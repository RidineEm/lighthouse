using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.WebRequestMethods;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using UnityEditor.Experimental.GraphView;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D target;

    Rigidbody2D rigid;
    Collider2D Collider2D;
    SpriteRenderer spriter;
    Animator EnemyAnimator;
    WaitForFixedUpdate wait;
    GameObject GameObject;


    bool isLive = true;
    bool isDead = false;
    bool isAttack = false;
    bool isHit = false;
    bool isMoving = false;
    public LayerMask playerLayers;

    public float health = 50f;
    public float Maxhealth = 50f;
    public float AttackDamage = 10f;
    public float Armour = 1f;
    public float Speed = 1.0f;
    public float attackDelay = 1.5f;
    public float attackCooldown = 0f;
    public float targetingRange = 5f;
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
        Vector2 attackSize = new Vector2(1.0f, 0.3f);
        attackRange = Vector2.SqrMagnitude(attackSize);
    }

    void FixedUpdate()
    {
        if (!isLive || EnemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {  //����� �������� ���� or ���ݹ����� ����
            StopMoving();
            return;
        }

        float dis = Vector2.Distance(transform.position, target.position);

        if (dis <= targetingRange) //  �νĹ��� �ȿ� ���� ���� �� �i�ư��� ������
        {
            Move();
        }
        if (dis <= attackRange)
        {
            print("���ݹ����ȿ� ����");
            StopMoving(); // �̵� ����
            Cooldown();
            if (!isAttack && attackCooldown <= 0f) // ��ٿ��� ������ ���� ���� �ƴ� ���� ����
            {
                Attack();
            }
        }
        else
        {
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
            EnemyAnimator.SetBool("Run", true); // �̵� �ִϸ��̼� ���
            return;
        }
        float dirx = target.position.x - transform.position.x;      // 
        float diry = target.position.y - transform.position.y;      //
        dirx = (dirx < 0) ? -1 : 1;                 //�������� dir�� x�Ÿ��� -��� -1,�ƴϸ� 1 + �ӵ� ����
        diry = (diry < 0) ? -1 : 1;
        transform.Translate(new Vector2(dirx, diry) * Speed * Time.deltaTime);
        EnemyAnimator.SetBool("Run", true);
        attackCooldown = attackDelay;
    }
    void StopMoving()
    {
        if (isMoving)
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
    public void TakeDamage(float takedamage)
    {
        //������ ���
        health -= (takedamage - Armour);
        if (health > 0)
        {
            StartCoroutine(HitAnimation()); // �´� �ִϸ��̼� ���
        }
        else // ü���� 0 �����̸� ���� ó��
        {
            isLive = false;
            Collider2D.enabled = false;     //�ݶ��̴� ��Ȱ��ȭ
            rigid.simulated = false;        // ������ٵ� ��Ȱ��ȭ
            spriter.sortingOrder = 1;       //sortingOrder�� �������� �ٸ� ������Ʈ�� ���ذ� ���� �ʵ��� ����
            EnemyAnimator.SetBool("Dead", true); //�׾����� �ִϸ��̼� Ȱ��ȭ
            isDead = true;
            Dead(); // ü���� 0 �����̸� ���� ó��
            print("�� ���");
        }
    }
    IEnumerator HitAnimation()
    {
        isHit = true;
        EnemyAnimator.SetTrigger("Hit");    // �´� �ִϸ��̼� Ʈ����
        yield return wait;                  // ���� ������������ ������
        EnemyAnimator.ResetTrigger("Hit");
        isHit = false;
        //print("�� ��Ʈ");
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
            EnemyAnimator.SetTrigger("FrogAttack");
            foreach (Collider2D Player in hitEnemies)
            {
                Player.GetComponent<PlayerController>().TakeDamage(AttackDamage);  // Ÿ���� �������� �޴� �޼��带 ȣ��
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
        EnemyAnimator.ResetTrigger("FrogAttack");
        yield return new WaitForSeconds(0.5f); // ���� �ִϸ��̼� ��� �ð�
    }

    IEnumerator Dead()
    {
        // ���� ����Ͽ����� �ִϸ��̼��� ���������� Ȯ���ϰ� Dead�������� Ȯ��
        while (!EnemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dead") ||
                       EnemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        Destroy(gameObject); // �ִϸ��̼��� ���� �� ������Ʈ�� ����
    }
}
