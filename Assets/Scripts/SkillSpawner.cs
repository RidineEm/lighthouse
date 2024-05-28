using UnityEngine;

public class SkillSpawner : MonoBehaviour
{
    public GameObject skillPrefab;   // �Ѿ� ������
    public Transform firePoint;        // �߻� ����
    PlayerController playerController; // PlayerController ��ũ��Ʈ�� �����ϱ� ���� ����
    public float shootCooldown = 6f;   // �߻� ����
    public float skillDistance = 2f; // ��ų ���� �Ÿ�

    private float lastShootTime;        // ���������� �Ѿ��� �߻��� �ð�

    void Start()
    {
        // PlayerController ��ũ��Ʈ�� ����
        playerController = FindObjectOfType<PlayerController>();
        lastShootTime = -shootCooldown; // �ʱⰪ ����
    }

    void Update()
    {
        // ��ų ��ȯ
        if (Input.GetKeyDown(KeyCode.W) && Time.time - lastShootTime > shootCooldown)
        {

            Shoot();
            lastShootTime = Time.time; // ���� �ð� ����
        }
    }

    void Shoot()
    {
        // �÷��̾ �ٶ󺸴� �������� ��ų�� ��ȯ�ϱ� ���� �÷��̾��� ������ ������ ����
        Vector2 skillDirection = transform.right;
        Vector2 spawnPosition = (Vector2)transform.position + skillDirection * skillDistance;

        // �÷��̾��� ������ ������ ���� ��� (�����ִ� ���) �⺻������ ���������� ����
        if (skillDirection.magnitude < 0.1f)
        {
            skillDirection = Vector2.right;
        }

        // ��ų ����
        GameObject Skill2 = Instantiate(skillPrefab, firePoint.position, Quaternion.identity);
        // ��ų ���� ����
        Skill skillComponent = Skill2.GetComponent<Skill>();

        if (skillComponent != null)
        {
            skillComponent.SetDirection(skillDirection);
        }
    }
}