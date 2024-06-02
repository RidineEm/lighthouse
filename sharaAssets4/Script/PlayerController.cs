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

    // ���� ������ ������ ü��
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
        if (!isAttack || !dead)  // �������� �ƴϰų� ���� �ʾ����� �̵�����
        {
            Walk();
        }
    }
    private void Attack()  //���� ���� �� �ִϸ��̼�
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!isAttack)
            {
                isAttack = true;
                playerAnimator.SetBool("Attack", true);
                PlayerRigidbody.velocity = Vector2.zero;        //�����߿��� �̵� �Ұ�
                //���ݽ���
                PerformAttack();
            }
        }

        // �ִϸ��̼� ���� ������ ������
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

        // ���� �ִϸ��̼� ���°� 'Attack'���� Ȯ��
        if (isAttack && stateInfo.IsName("Attack"))
        {
            // �ִϸ��̼��� �������� Ȯ��
            if (stateInfo.normalizedTime >= 1.0f)
            {
                isAttack = false;
                playerAnimator.SetBool("Attack", false);
            }
        } 
    }
    private void PerformAttack()
    {
        //���� ���� ����
        Vector2 attackOffset = new Vector2(0.5f, 0.0f);
        Vector2 attackSize = new Vector2(0.8f, 0.5f);
        //���� ���ݽ� �׿� �°� ���ݹ����� �������� �̵���Ŵ
        Vector2 attackCenter = (Vector2)transform.position + (spriter.flipX ? new Vector2(-attackOffset.x, attackOffset.y) : attackOffset);
        // ���� ���� ���� �� ����
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackCenter, attackSize, 0f, enemyLayers);

        // ������ ������ �ֱ�
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                // IDamageable ������Ʈ�� ���� ���� ã��
                IDamageable damageable = enemy.GetComponent<IDamageable>();
                // ������ �������� ������ �Լ� ȣ��
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
        //ȭ�鿡 ���� ǥ��
        Vector2 attackOffset = new Vector2(0.5f, 0.0f);
        Vector2 attackSize = new Vector2(0.8f, 0.5f);
        Vector2 attackCenter = (Vector2)transform.position + (spriter.flipX ? new Vector2(-attackOffset.x, attackOffset.y) : attackOffset);

        Gizmos.DrawWireCube(attackCenter, attackSize);
    }
    private void Walk()  //�÷��̾� �̵� ���� �� �ִϸ��̼�
    {
        if (dead) return;

        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        float xSpeed = xInput * speed;
        float ySpeed = yInput * speed;

        //�̵��� �ִϸ��̼� ����
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

        // �¿������ �̿��Ͽ� ���� �ִϸ��̼� ���
        if (xInput != 0)
        {
            spriter.flipX = xSpeed < 0;
        }
    }
    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
        if (health > 0 && dead == false)
        {// �´� �ִϸ��̼� ���
            print("�÷��̾ ���� ����");
        }
        else
        {
            dead = true;
            Die();
        }
    }
    public void Die()
    {
        // ���� �׾����� Ʈ���Ŵ� ����
        playerAnimator.SetTrigger("Die");
        print(dead + "�÷��̾� ���");

        PlayerRigidbody.velocity = Vector2.zero;
    }
}
