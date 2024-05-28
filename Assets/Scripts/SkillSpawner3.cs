using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSpawner3 : MonoBehaviour
{
    public GameObject skillPrefab;    // �Ѿ� ������
    public Transform firePoint;        // �߻� ����
    PlayerController playerController; // PlayerController ��ũ��Ʈ�� �����ϱ� ���� ����
    public float shootCooldown = 5f;   // �߻� ����

    private float lastShootTime;        // ���������� �Ѿ��� �߻��� �ð�

    void Start()
    {
        // PlayerController ��ũ��Ʈ�� ����
        playerController = FindObjectOfType<PlayerController>();
        lastShootTime = -shootCooldown; // �ʱⰪ ����
    }

    void Update()
    {
        // �Ѿ� �߻�
        if (Input.GetKeyDown(KeyCode.R) && Time.time - lastShootTime > shootCooldown)
        {
            Shoot();
            lastShootTime = Time.time; // �߻� �ð� ����
        }
    }

    void Shoot()
    {
        // �÷��̾ �ٶ󺸴� �������� �Ѿ��� �߻��ϱ� ���� �÷��̾��� ������ ������ ����
        Vector2 shootDirection = playerController.GetMovementDirection();
        shootDirection = Vector2.right;

        // �Ѿ� ����
        GameObject bullet = Instantiate(skillPrefab, firePoint.position, Quaternion.identity);
    }
}
