using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveMap : MonoBehaviour
{
    // Start is called before the first frame update
    public string transferMapName; //�̵��� ���̸�       
    
    private bool isclear;  //Ŭ���� Ȯ�ο�
    private PlayerController thePlayer; 
    private Camera theCamera;
    private GameObject cameraTarget;// ���̵� ī�޶� Ÿ�� ����
    private GameObject target; // ���̵� �÷��̾� Ÿ�� ����

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();//�÷��̾� ��ü ������
        theCamera = FindObjectOfType<Camera>();//ī�޶� ������
        
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isclear = GameObject.Find("isclear").GetComponent<RoomManager>().isclear;//Ŭ���� ���� Ȯ�ο����� ������
        if (collision.gameObject.name == "Player"&&isclear==true) //�÷��̾ �ݶ��̴� ���,Ŭ��������϶�
        {
            thePlayer.currentMapName = transferMapName; //�÷��̾ �̵��� ���̸� ����
            cameraTarget = GameObject.Find(transferMapName);//ī�޶� �̵��� Ÿ�� ������
            
            //�÷��̾ �̵��� ��ġ(���� ��ǥ)������
            target = GameObject.Find(transferMapName+"/Door");//������ ���̸��̶� ���� ������Ʈ �ڽ��� Door�� ã�Ƽ� ������

            print(transferMapName + "���� �̵�");//Ȯ�ο� 
            theCamera.transform.position = new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y, theCamera.transform.position.z);//ī�޶� �̵�
            if (transferMapName == "bossmap")
            {
                theCamera.orthographicSize = 15; // �� �κ� 12���� 15�� ���̸� �̴ϸ� ī�޶� SIZE �ٲ�µ� 15������ �� ����
            }
            thePlayer.transform.position = target.transform.position;//������ ������ġ���� ĳ���� �̵�

        }
    }


}
