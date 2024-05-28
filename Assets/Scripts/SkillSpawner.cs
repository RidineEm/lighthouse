using UnityEngine;

public class SkillSpawner : MonoBehaviour
{
    public GameObject skillPrefab;   // 총알 프리팹
    public Transform firePoint;        // 발사 지점
    PlayerController playerController; // PlayerController 스크립트를 참조하기 위한 변수
    public float shootCooldown = 6f;   // 발사 간격
    public float skillDistance = 2f; // 스킬 생성 거리

    private float lastShootTime;        // 마지막으로 총알을 발사한 시간

    void Start()
    {
        // PlayerController 스크립트를 참조
        playerController = FindObjectOfType<PlayerController>();
        lastShootTime = -shootCooldown; // 초기값 설정
    }

    void Update()
    {
        // 스킬 소환
        if (Input.GetKeyDown(KeyCode.W) && Time.time - lastShootTime > shootCooldown)
        {

            Shoot();
            lastShootTime = Time.time; // 생성 시간 갱신
        }
    }

    void Shoot()
    {
        // 플레이어가 바라보는 방향으로 스킬을 소환하기 위해 플레이어의 움직임 방향을 얻어옴
        Vector2 skillDirection = transform.right;
        Vector2 spawnPosition = (Vector2)transform.position + skillDirection * skillDistance;

        // 플레이어의 움직임 방향이 없는 경우 (멈춰있는 경우) 기본적으로 오른쪽으로 설정
        if (skillDirection.magnitude < 0.1f)
        {
            skillDirection = Vector2.right;
        }

        // 스킬 생성
        GameObject Skill2 = Instantiate(skillPrefab, firePoint.position, Quaternion.identity);
        // 스킬 방향 설정
        Skill skillComponent = Skill2.GetComponent<Skill>();

        if (skillComponent != null)
        {
            skillComponent.SetDirection(skillDirection);
        }
    }
}