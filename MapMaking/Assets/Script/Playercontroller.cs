using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    static public PlayerController instance;
    public string currentMapName;// ���̵��� ����� ����� ���̸�
    public RoomManager roomManager;//Ŭ�������Ȯ�ο�

    private Rigidbody2D PlayerRigidbody;
    private Animator playerAnimator;
    private SpriteRenderer spriter;
    BoxCollider2D C_collider;
    public float speed = 5f;


    private bool isDead = false;
    private bool isAttack = false;
    private bool isWalk = false;
    private bool prevFlipX = false;

    // ���� ������ ������
    public float attackRange = 0.35f;
    public int attackDamage = 10;
    public LayerMask enemyLayers;

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        C_collider = GetComponent<BoxCollider2D>();
        //�÷��̾� ���̵��� ���� ������
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject); // ���� ������Ʈ �ı�����

            C_collider = GetComponent<BoxCollider2D>();
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);//ĳ���� 2�� �ʰ��� ����°� ������
        }
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
                PlayerRigidbody.velocity = Vector2.zero;

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
        //���ݹ��� ����
        Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 attackOffset = new Vector2(0f, -0.1f);

        Vector2 attackCenter = playerPosition + attackOffset;
        // ���� ���� ���� �� ����
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackCenter, attackRange, enemyLayers);


        // ������ ������ �ֱ�
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        //ȭ�鿡 ���� ǥ��
        Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 attackOffset = new Vector2(0f, -0.1f);

        Vector2 attackCenter = playerPosition + attackOffset;

        Gizmos.DrawWireSphere(attackCenter, attackRange);
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
            C_collider.offset = new Vector2(0.05f, -0.05f);

            //�̰� ������ ī�޶������°� ī�޶� �÷��̾� �ڽ� ������Ʈ�� �־ �����ϸ�
            //���� �ٲܶ����� ī�޶� �����Ÿ� �Ʒ��� ���Ͱ��� �� 0���� �ϰų� �ƿ� ������ �ذ�Ǳ���
            //�׷��� ī�޶� ������°� ��������� �ٲٴ��� �ƴ� �� �Ʒ��κ��� �ٲٴ��� �ؾ��ҵ�
            //������ �̴ϸ��̳� ������ ī�޶�� ���� ��������

            //�¿� ������ ��ġ�� ����Ǿ� ������
            bool currentFlipX = xSpeed < 0;
            if (prevFlipX != currentFlipX)
            {
                spriter.flipX = currentFlipX;

                // ������Ʈ ��ġ �̵� (�¿������ ������Ʈ�� ��¦ �̵��Ͽ� �߰��� �ڵ�)
                if (currentFlipX)
                {
                    transform.position += new Vector3(-0.3f, 0, 0);
                }
                else
                {
                    transform.position += new Vector3(0.3f, 0, 0);
                }
                // ���� ���¸� ���� ���·� ������Ʈ
                prevFlipX = currentFlipX;
            }
        }

        //���� �¿� ������ ��ġ�� �ణ ����Ǿ �׿� �°� �ݶ��̴� ��ġ ����
        if (xInput > 0) { C_collider.offset = new Vector2(-0.05f, -0.05f); }
    }
    private void Die()
    {
        // ���� �׾����� Ʈ���Ŵ� ����
        playerAnimator.SetTrigger("Die");

        PlayerRigidbody.velocity = Vector2.zero;
        isDead = true;
    }
    private void OnTriggerEnter2D(Collider2D collision) //Ŭ���� �׽�Ʈ��
    {
        if (collision.gameObject.name == "clear") //clear ��ư �ǵ��̸�
        {
            roomManager.isclear = true; //Ŭ���� ���¸� true�� ����
            print(roomManager.isclear);//Ȯ�ο� print
        }
        else if (collision.gameObject.name == "notclear")//notclear ��ư �ǵ��̸�
        {
            roomManager.isclear = false;//Ŭ���� ���¸� false�� ����
            print(roomManager.isclear);//Ȯ�ο� print
        }
    }
}
