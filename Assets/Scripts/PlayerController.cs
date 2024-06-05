using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D playerRigidbody;
    public float speed = 20f;
    private float originalSpeed; // 원래 속도

    public int healItemCount = 2; // 사용 가능한 힐 아이템 개수
    public int healAmount = 20;   // 회복량

    public int speedBoostItemCount = 2; // 사용 가능한 속도 증가 아이템 개수
    public float speedMultiplier = 2f;  // 속도 배수
    public float speedBoostDuration = 3f; // 지속 시간

    private PlayerHealth playerHealth;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        originalSpeed = speed; // 시작 시 원래 속도를 저장
        playerHealth = GetComponent<PlayerHealth>(); // PlayerHealth 컴포넌트 참조
    }

    void Update()
    {
        // 플레이어 이동
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(xInput, yInput) * speed;
        Vector2 newPosition = playerRigidbody.position + movement * Time.deltaTime;
        playerRigidbody.MovePosition(newPosition);

        // 힐 아이템 사용
        if ((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) && healItemCount > 0)
        {
            playerHealth.Heal(healAmount);
            healItemCount--; // 사용한 아이템 개수 감소
            Debug.Log("Heal item used. Remaining items: " + healItemCount);
        }

        // 속도 증가 아이템 사용 (현재 UI쪽 2번 인벤토리가 완성되지 않아 더미데이터로 9번을 누르면 발동되도록 임의 설정
        if ((Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad2)) && speedBoostItemCount > 0)
        {
            StartCoroutine(SpeedBoost(speedMultiplier, speedBoostDuration));
            speedBoostItemCount--; // 사용한 아이템 개수 감소
            Debug.Log("Speed boost item used. Remaining items: " + speedBoostItemCount);
        }
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
