using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D playerRigidbody;
    public float speed = 20f;
    private float originalSpeed; // ���� �ӵ�

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        originalSpeed = speed; // ���� �� ���� �ӵ��� ����
    }

    void Update()
    {
        // �÷��̾� �̵�
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(xInput, yInput) * speed;
        Vector2 newPosition = playerRigidbody.position + movement * Time.deltaTime;
        playerRigidbody.MovePosition(newPosition);
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
