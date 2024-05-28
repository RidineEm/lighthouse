using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // 플레이어의 최대 체력
    public int currentHealth;   // 플레이어의 현재 체력

    void Start()
    {
        currentHealth = maxHealth; // 시작 시 현재 체력을 최대 체력으로 설정
    }

    // 체력을 회복하는 메서드
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // 최대 체력을 초과하지 않도록 제한
        }

        Debug.Log("Player healed. Current health: " + currentHealth);
    }
}
