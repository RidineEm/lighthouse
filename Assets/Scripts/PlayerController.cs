using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D playerRigidbody;
    public float speed = 20f;
    private float originalSpeed; // ���� �ӵ�

    public int healItemCount = 2; // ��� ������ �� ������ ����
    public int healAmount = 20;   // ȸ����

    public int speedBoostItemCount = 2; // ��� ������ �ӵ� ���� ������ ����
    public float speedMultiplier = 2f;  // �ӵ� ���
    public float speedBoostDuration = 3f; // ���� �ð�

    private PlayerHealth playerHealth;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        originalSpeed = speed; // ���� �� ���� �ӵ��� ����
        playerHealth = GetComponent<PlayerHealth>(); // PlayerHealth ������Ʈ ����
    }

    void Update()
    {
        // �÷��̾� �̵�
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(xInput, yInput) * speed;
        Vector2 newPosition = playerRigidbody.position + movement * Time.deltaTime;
        playerRigidbody.MovePosition(newPosition);

        // �� ������ ���
        if ((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) && healItemCount > 0)
        {
            playerHealth.Heal(healAmount);
            healItemCount--; // ����� ������ ���� ����
            Debug.Log("Heal item used. Remaining items: " + healItemCount);
        }

        // �ӵ� ���� ������ ��� (���� UI�� 2�� �κ��丮�� �ϼ����� �ʾ� ���̵����ͷ� 9���� ������ �ߵ��ǵ��� ���� ����
        if ((Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad2)) && speedBoostItemCount > 0)
        {
            StartCoroutine(SpeedBoost(speedMultiplier, speedBoostDuration));
            speedBoostItemCount--; // ����� ������ ���� ����
            Debug.Log("Speed boost item used. Remaining items: " + speedBoostItemCount);
        }
    }

    // �÷��̾��� ������ ������ ��ȯ�ϴ� �Լ�
    public Vector2 GetMovementDirection()
    {
        // �÷��̾��� �̵� �Է� ���� ��ȯ
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    // �ӵ� ���� �ڷ�ƾ
    public IEnumerator SpeedBoost(float multiplier, float duration)
    {
        speed *= multiplier; // �ӵ� ����
        Debug.Log("Speed boosted: " + speed); // ����� �α�

        yield return new WaitForSeconds(duration); // ���� �ð� ���� ���

        speed = originalSpeed; // ���� �ӵ��� ����
        Debug.Log("Speed restored: " + speed); // ����� �α�
    }
}
