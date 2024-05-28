using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int healAmount = 20; // 회복량

    void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 객체가 플레이어인지 확인
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        PlayerController playercontroller = other.GetComponent<PlayerController>();
        if (other.tag == "Player")
        {
            // 플레이어의 체력을 회복시키고 힐 아이템을 파괴
            playerHealth.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
