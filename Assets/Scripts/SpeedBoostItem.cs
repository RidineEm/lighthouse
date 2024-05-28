using UnityEngine;

public class SpeedBoostItem : MonoBehaviour
{
    public float speedMultiplier = 2f; // 속도 배수
    public float duration = 3f;        // 지속 시간

    void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 객체가 플레이어인지 확인
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            // 플레이어의 속도를 증가시키고 아이템을 비활성화
            playerController.StartCoroutine(playerController.SpeedBoost(speedMultiplier, duration));
            gameObject.SetActive(false);
        }
    }
}
