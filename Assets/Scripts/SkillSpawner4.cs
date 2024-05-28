using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSpawner4 : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.E) && Time.time - lastShootTime > shootCooldown)
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

        // �Ѿ� ����
        GameObject bullet = Instantiate(skillPrefab, firePoint.position, Quaternion.identity);
        // �Ѿ� ���� ����
        Skill4 bulletComponent = bullet.GetComponent<Skill4>();
        if (bulletComponent != null)
        {
            bulletComponent.SetDirection(shootDirection);
        }
    }
}
