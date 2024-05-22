using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D PlayerRigidbody;
    private Animator playerAnimator;
    private SpriteRenderer spriter;
    Collider2D Collider2D;
    public float speed = 5f;

    private bool isDead = false;
    private bool isAttack = false;
    private bool isWalk = false;

    // ���� ������ ������ ü��
    public float attackDamage = 10f;
    public LayerMask enemyLayers;
    public float HP;
    public float MaxHP = 100f;
    public float PlayerArmour = 0f;

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        Collider2D = GetComponent<Collider2D>();
        HP = MaxHP;
    }   

    void Update()
    {
        if (isDead) return;
        Attack();
    }

    private void FixedUpdate()
    {
        if (!isAttack)  // ���� �߿��� �̵��� ����
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
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackCenter, attackSize, 0f, enemyLayers); // ���� ���� ���� �� ����

        // ������ ������ �ֱ�
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
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
    public void TakeDamage(float takedamage)
    {
        HP -= (takedamage - PlayerArmour);
        print("�÷��̾ ���� ����");
    }
    private void Walk()  //�÷��̾� �̵� ���� �� �ִϸ��̼�
    {
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
    private void Die()
    {
        // ���� �׾����� Ʈ���Ŵ� ����
        playerAnimator.SetBool("Die",true);

        PlayerRigidbody.velocity = Vector2.zero;
        isDead = true;
    }
}
