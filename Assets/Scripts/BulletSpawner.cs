using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;    // �Ѿ� ������
    public Transform firePoint;        // �߻� ����
    PlayerController playerController; // PlayerController ��ũ��Ʈ�� �����ϱ� ���� ����
    public float shootCooldown = 5f;   // �߻� ����

    private float lastShootTime;        // ���������� �Ѿ��� �߻��� �ð�
    private GameObject bullet;          // ������ �Ѿ� �ν��Ͻ�

    void Start()
    {
        // PlayerController ��ũ��Ʈ�� ����
        playerController = FindObjectOfType<PlayerController>();
        lastShootTime = -shootCooldown; // �ʱⰪ ����

        // �Ѿ� �������� �̸� �ν��Ͻ�ȭ�ϰ� ��Ȱ��ȭ
        bullet = Instantiate(bulletPrefab);
        bullet.SetActive(false);
    }

    void Update()
    {
        // �Ѿ� �߻�
        if (Input.GetKeyDown(KeyCode.Q) && Time.time - lastShootTime > shootCooldown)
        {
            Shoot();
            lastShootTime = Time.time; // �߻� �ð� ����
        }
    }

    void Shoot()
    {
        // �÷��̾ �ٶ󺸴� �������� �Ѿ��� �߻��ϱ� ���� �÷��̾��� ������ ������ ����
        Vector2 shootDirection = playerController.GetMovementDirection();

        // �÷��̾��� ������ ������ ���� ��� (�����ִ� ���) �⺻������ ���������� ����
        if (shootDirection.magnitude < 0.1f)
        {
            shootDirection = Vector2.right;
        }

        // �Ѿ��� �߻� �������� �̵���Ű�� ���� ���� �� Ȱ��ȭ
        bullet.transform.position = firePoint.position;
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.SetDirection(shootDirection);
        }

        bullet.SetActive(true);
    }
}
