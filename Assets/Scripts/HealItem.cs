using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int healAmount = 20; // 회복량

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            // 플레이어의 인벤토리에 힐 아이템을 추가
            playerController.healItemCount++;
            Destroy(gameObject); // 아이템을 파괴
        }
    }
}
