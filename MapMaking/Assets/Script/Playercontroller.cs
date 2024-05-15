using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Playercontroller : MonoBehaviour
{
    static  public Playercontroller instance;
    Rigidbody2D playerRigidbody;
    BoxCollider2D boxCollider;
    public string currentMapName;// ���̵��� ����� ����� ���̸�
    public RoomManager roomManager;//Ŭ�������Ȯ�ο�

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        
        //�÷��̾� ���̵��� ���� ������
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject); // ���� ������Ʈ �ı�����

            boxCollider = GetComponent<BoxCollider2D>();
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);//ĳ���� 2�� �ʰ��� ����°� ������
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) //Ŭ���� �׽�Ʈ��
    {
        if (collision.gameObject.name == "clear") //clear ��ư �ǵ��̸�
        {
            roomManager.isclear = true; //Ŭ���� ���¸� true�� ����
            print(roomManager.isclear);//Ȯ�ο� print
        }
        else if (collision.gameObject.name == "notclear")//notclear ��ư �ǵ��̸�
        {
            roomManager.isclear= false;//Ŭ���� ���¸� false�� ����
            print(roomManager.isclear);//Ȯ�ο� print
        }
    }

    // Update is called once per frame
    void Update()
    {   //������ �̵�����
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
     
        float xspeed = x * 8f;
        float yspeed = y * 8f;
        Vector3 velo = new Vector3(xspeed, yspeed, 0f);
        playerRigidbody.velocity = velo;
    }
}
