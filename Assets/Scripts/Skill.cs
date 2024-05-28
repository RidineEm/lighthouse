using UnityEngine;

public class Skill : MonoBehaviour
{
    public float speed = 10f;
    public float lifespan = 1f;
    Vector2 direction; // ��ų�� ����
    private Rigidbody2D skillRigidbody;

    void Awake()
    {
        skillRigidbody = GetComponent<Rigidbody2D>();

    }

    void OnEnable()
    {

        // ���� �ð��� ������ ��ų�� ��Ȱ��ȭ
        Invoke("DeactivateBullet", lifespan);
    }

    // ���� ���� �޼���
    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    void DeactivateBullet()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //�浹�� ������ ���̶��     
        if (other.tag == "Enemy")
        {
            //Monster�� Die()�� ȣ���ϱ� ����
            //monster ������Ʈ�� MonsterController��ũ��Ʈ ���� ����

            // MonsterController monsterController = other.GetComponent<MonsterController>();
            // ���� ��ũ��Ʈ�� ���⿡ �ϴ� �ּ�ó��

            // if (monsterController != null)
            // {
            //  obj.GetComponent<hpbar>().Damage();  ---> �̶� ��ų�� �������� if �⺻���� damage�� 10�Ͻ� ��ų�� ���� 2���� 20���� �����ϱ�

            //�ı�
            //Destroy(gameObject);
            //  }
        }

        //�浹�� ���� ���̶��
        if (other.tag == "Wall")
        {
            //�ı�
            Destroy(gameObject);
        }
    }
}