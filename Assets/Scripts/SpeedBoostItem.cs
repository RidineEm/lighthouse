using UnityEngine;

public class SpeedBoostItem : MonoBehaviour
{
    public float speedMultiplier = 2f; // 속도 배수
    public float duration = 3f;        // 지속 시간

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            // 플레이어의 인벤토리에 속도 증가 아이템을 추가
            playerController.speedBoostItemCount++;
            Destroy(gameObject); // 아이템을 파괴
        }
    }
}
