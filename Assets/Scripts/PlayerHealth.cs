using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // �÷��̾��� �ִ� ü��
    public int currentHealth;   // �÷��̾��� ���� ü��

    void Start()
    {
        currentHealth = maxHealth; // ���� �� ���� ü���� �ִ� ü������ ����
    }

    // ü���� ȸ���ϴ� �޼���
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // �ִ� ü���� �ʰ����� �ʵ��� ����
        }

        Debug.Log("Player healed. Current health: " + currentHealth);
    }
}
