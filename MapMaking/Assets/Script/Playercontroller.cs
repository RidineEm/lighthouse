using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    static public PlayerController instance;
    public string currentMapName;// 맵이동시 사용할 저장용 맵이름
    public RoomManager roomManager;//클리어상태확인용

    private Rigidbody2D PlayerRigidbody;
    private Animator playerAnimator;
    private SpriteRenderer spriter;
    BoxCollider2D C_collider;
    public float speed = 5f;


    private bool isDead = false;
    private bool isAttack = false;
    private bool isWalk = false;
    private bool prevFlipX = false;

    // 공격 범위와 데미지
    public float attackRange = 0.35f;
    public int attackDamage = 10;
    public LayerMask enemyLayers;

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        C_collider = GetComponent<BoxCollider2D>();
        //플레이어 맵이동시 버그 방지용
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject); // 게임 오브젝트 파괴금지

            C_collider = GetComponent<BoxCollider2D>();
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);//캐릭터 2개 초과로 생기는거 방지용
        }
    }   

    void Update()
    {
        if (isDead) return;
        Attack();
    }

    private void FixedUpdate()
    {
        if (!isAttack)  // 공격 중에는 이동을 막음
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
                PlayerRigidbody.velocity = Vector2.zero;

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
        //공격범위 조정
        Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 attackOffset = new Vector2(0f, -0.1f);

        Vector2 attackCenter = playerPosition + attackOffset;
        // 공격 범위 내의 적 감지
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackCenter, attackRange, enemyLayers);


        // 적에게 데미지 주기
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        //화면에 범위 표현
        Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 attackOffset = new Vector2(0f, -0.1f);

        Vector2 attackCenter = playerPosition + attackOffset;

        Gizmos.DrawWireSphere(attackCenter, attackRange);
    }
    private void Walk()  //플레이어 이동 구현 및 애니메이션
    {
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
            C_collider.offset = new Vector2(0.05f, -0.05f);

            //이거 있으면 카메라따라댕기는걸 카메라를 플레이어 자식 오브젝트로 넣어서 구현하면
            //방향 바꿀때마다 카메라 덜덜거림 아래쪽 백터값을 다 0으로 하거나 아예 날리면 해결되긴함
            //그래서 카메라 따라댕기는거 구현방법을 바꾸던지 아님 이 아랫부분을 바꾸던지 해야할듯
            //나머지 미니맵이나 보스방 카메라는 딱히 문제없음

            //좌우 반전시 위치가 변경되어 재조정
            bool currentFlipX = xSpeed < 0;
            if (prevFlipX != currentFlipX)
            {
                spriter.flipX = currentFlipX;

                // 오브젝트 위치 이동 (좌우반전시 오브젝트가 살짝 이동하여 추가한 코드)
                if (currentFlipX)
                {
                    transform.position += new Vector3(-0.3f, 0, 0);
                }
                else
                {
                    transform.position += new Vector3(0.3f, 0, 0);
                }
                // 현재 상태를 이전 상태로 업데이트
                prevFlipX = currentFlipX;
            }
        }

        //현재 좌우 반전시 위치가 약간 변경되어서 그에 맞게 콜라이더 위치 수정
        if (xInput > 0) { C_collider.offset = new Vector2(-0.05f, -0.05f); }
    }
    private void Die()
    {
        // 아직 죽었을때 트리거는 없음
        playerAnimator.SetTrigger("Die");

        PlayerRigidbody.velocity = Vector2.zero;
        isDead = true;
    }
    private void OnTriggerEnter2D(Collider2D collision) //클리어 테스트용
    {
        if (collision.gameObject.name == "clear") //clear 버튼 건들이면
        {
            roomManager.isclear = true; //클리어 상태를 true로 변경
            print(roomManager.isclear);//확인용 print
        }
        else if (collision.gameObject.name == "notclear")//notclear 버튼 건들이면
        {
            roomManager.isclear = false;//클리어 상태를 false로 변경
            print(roomManager.isclear);//확인용 print
        }
    }
}
