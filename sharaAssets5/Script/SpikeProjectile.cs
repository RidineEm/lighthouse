using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpikeProjectile : MonoBehaviour
{
    public int damage = 20;
    public float speed = 5.0f; // ������ũ �ӵ�

    private Vector2 moveDirection;

    private void Start()
    {
        // �ʱ� �̹��� ȸ�� ����
        RotateSprite();
        // ������ũ �߻�
        GetComponent<Rigidbody2D>().velocity = moveDirection * speed;
        Destroy(gameObject, 5.0f);
    }

    public void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction;
        RotateSprite();
    }

    private void RotateSprite()
    {
        // �̵� ���⿡ ���� ��������Ʈ�� ȸ����Ŵ
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ��ǥ���� �������� �ֱ� ���� ó��
        if (collision.gameObject.CompareTag("Player"))
        {
            // ��� ������Ʈ�� �������� ���� �� �ִ��� Ȯ��
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.GetComponent<PlayerController>().OnDamage(damage);
            }
            Destroy(gameObject); // �浹 �� ������ũ ����
        }
    }
}
