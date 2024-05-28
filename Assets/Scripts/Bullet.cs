using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifespan = 1f; // 총알이 사라지는 시간
    private Vector2 direction; // 총알의 방향
    private Rigidbody2D bulletRigidbody;

    void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        // 시작 시 총알의 방향으로 설정된 속도로 이동
        bulletRigidbody.velocity = direction.normalized * speed;

        // 일정 시간이 지나면 총알을 비활성화
        Invoke("DeactivateBullet", lifespan);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = direction.normalized * speed;
        }
    }

    void DeactivateBullet()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //충돌한 상대방이 적이라면     
        if (other.tag == "Enemy")
        {
            //Monster의 Die()를 호출하기 위해
            //monster 오브젝트의 MonsterController스크립트 정보 얻어옴

            // MonsterController monsterController = other.GetComponent<MonsterController>();
            // 당장 스크립트가 없기에 일단 주석처리

            // if (monsterController != null)
            // {
            //  obj.GetComponent<hpbar>().Damage();  ---> 이때 스킬의 데미지는 if 기본공격 damage가 10일시 스킬은 그의 2배인 20으로 설정하기

            // 해당 스킬은 관통하는 스킬로, 몬스터에 닿아도 없어지지 않고, 스킬의 생명주기만큼 적을 관통함 
            //  }
        }

        //충돌한 것이 벽이라면
        if (other.tag == "Wall")
        {
            //파괴
            Destroy(gameObject);
        }
    }
}
