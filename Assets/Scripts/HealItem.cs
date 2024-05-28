using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int healAmount = 20; // ȸ����

    void OnTriggerEnter2D(Collider2D other)
    {
        // �浹�� ��ü�� �÷��̾����� Ȯ��
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        PlayerController playercontroller = other.GetComponent<PlayerController>();
        if (other.tag == "Player")
        {
            // �÷��̾��� ü���� ȸ����Ű�� �� �������� �ı�
            playerHealth.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
