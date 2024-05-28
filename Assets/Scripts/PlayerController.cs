using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D playerRigidbody;
    public float speed = 20f;
    private float originalSpeed; // 원래 속도

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        originalSpeed = speed; // 시작 시 원래 속도를 저장
    }

    void Update()
    {
        // 플레이어 이동
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(xInput, yInput) * speed;
        Vector2 newPosition = playerRigidbody.position + movement * Time.deltaTime;
        playerRigidbody.MovePosition(newPosition);
    }

    // 플레이어의 움직임 방향을 반환하는 함수
    public Vector2 GetMovementDirection()
    {
        // 플레이어의 이동 입력 값을 반환
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    // 속도 증가 코루틴
    public IEnumerator SpeedBoost(float multiplier, float duration)
    {
        speed *= multiplier; // 속도 증가
        Debug.Log("Speed boosted: " + speed); // 디버깅 로그

        yield return new WaitForSeconds(duration); // 지속 시간 동안 대기

        speed = originalSpeed; // 원래 속도로 복원
        Debug.Log("Speed restored: " + speed); // 디버깅 로그
    }
}
