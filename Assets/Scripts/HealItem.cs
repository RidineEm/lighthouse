using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int healAmount = 20; // ȸ����

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            // �÷��̾��� �κ��丮�� �� �������� �߰�
            playerController.healItemCount++;
            Destroy(gameObject); // �������� �ı�
        }
    }
}
