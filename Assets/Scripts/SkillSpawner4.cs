using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSpawner4 : MonoBehaviour
{
    public GameObject skillPrefab;    // 총알 프리팹
    public Transform firePoint;        // 발사 지점
    PlayerController playerController; // PlayerController 스크립트를 참조하기 위한 변수
    public float shootCooldown = 5f;   // 발사 간격

    private float lastShootTime;        // 마지막으로 총알을 발사한 시간

    void Start()
    {
        // PlayerController 스크립트를 참조
        playerController = FindObjectOfType<PlayerController>();
        lastShootTime = -shootCooldown; // 초기값 설정
    }

    void Update()
    {
        // 총알 발사
        if (Input.GetKeyDown(KeyCode.E) && Time.time - lastShootTime > shootCooldown)
        {

            Shoot();
            lastShootTime = Time.time; // 발사 시간 갱신
        }
    }

    void Shoot()
    {
        // 플레이어가 바라보는 방향으로 총알을 발사하기 위해 플레이어의 움직임 방향을 얻어옴
        Vector2 shootDirection = playerController.GetMovementDirection();

        // 플레이어의 움직임 방향이 없는 경우 (멈춰있는 경우) 기본적으로 오른쪽으로 설정
        if (shootDirection.magnitude < 0.1f)
        {
            shootDirection = Vector2.right;
        }

        // 총알 생성
        GameObject bullet = Instantiate(skillPrefab, firePoint.position, Quaternion.identity);
        // 총알 방향 설정
        Skill4 bulletComponent = bullet.GetComponent<Skill4>();
        if (bulletComponent != null)
        {
            bulletComponent.SetDirection(shootDirection);
        }
    }
}
