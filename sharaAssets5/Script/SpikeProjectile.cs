using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpikeProjectile : MonoBehaviour
{
    public int damage = 20;
    public float speed = 5.0f; // 스파이크 속도

    private Vector2 moveDirection;

    private void Start()
    {
        // 초기 이미지 회전 설정
        RotateSprite();
        // 스파이크 발사
        GetComponent<Rigidbody2D>().velocity = moveDirection * speed;
        Destroy(gameObject, 5.0f);
    }

    public void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction;
        RotateSprite();
    }

    private void RotateSprite()
    {
        // 이동 방향에 따라 스프라이트를 회전시킴
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 목표물에 데미지를 주기 위한 처리
        if (collision.gameObject.CompareTag("Player"))
        {
            // 대상 오브젝트가 데미지를 받을 수 있는지 확인
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.GetComponent<PlayerController>().OnDamage(damage);
            }
            Destroy(gameObject); // 충돌 후 스파이크 제거
        }
    }
}
