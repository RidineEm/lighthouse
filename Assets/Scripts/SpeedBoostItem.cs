using UnityEngine;

public class SpeedBoostItem : MonoBehaviour
{
    public float speedMultiplier = 2f; // �ӵ� ���
    public float duration = 3f;        // ���� �ð�

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            // �÷��̾��� �κ��丮�� �ӵ� ���� �������� �߰�
            playerController.speedBoostItemCount++;
            Destroy(gameObject); // �������� �ı�
        }
    }
}
