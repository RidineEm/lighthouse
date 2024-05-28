using UnityEngine;

public class SpeedBoostItem : MonoBehaviour
{
    public float speedMultiplier = 2f; // �ӵ� ���
    public float duration = 3f;        // ���� �ð�

    void OnTriggerEnter2D(Collider2D other)
    {
        // �浹�� ��ü�� �÷��̾����� Ȯ��
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            // �÷��̾��� �ӵ��� ������Ű�� �������� ��Ȱ��ȭ
            playerController.StartCoroutine(playerController.SpeedBoost(speedMultiplier, duration));
            gameObject.SetActive(false);
        }
    }
}
