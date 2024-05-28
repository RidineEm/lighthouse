using UnityEngine;

public class Skill : MonoBehaviour
{
    public float speed = 10f;
    public float lifespan = 1f;
    Vector2 direction; // 스킬의 방향
    private Rigidbody2D skillRigidbody;

    void Awake()
    {
        skillRigidbody = GetComponent<Rigidbody2D>();

    }

    void OnEnable()
    {

        // 일정 시간이 지나면 스킬을 비활성화
        Invoke("DeactivateBullet", lifespan);
    }

    // 방향 설정 메서드
    public void SetDirection(Vector2 dir)
    {
        direction = dir;
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

            //파괴
            //Destroy(gameObject);
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