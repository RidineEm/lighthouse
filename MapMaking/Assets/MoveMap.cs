using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveMap : MonoBehaviour
{
    // Start is called before the first frame update
    public string transferMapName; //�̵��� ���̸�       
    public Transform target; // �̵��� Ÿ�� ����

    private Playercontroller thePlayer; 
    private Camera theCamera;
    public GameObject mapcenter;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<Playercontroller>();
        theCamera = FindObjectOfType<Camera>();
        
    }

    // �ڽ� �ݶ��̴��� ��� ���� �̺�Ʈ �߻�
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.name == "Player")
        {
            thePlayer.currentMapName = transferMapName;
            mapcenter = GameObject.Find(transferMapName);
            print(transferMapName + "���� �̵�");
            theCamera.transform.position = new Vector3(mapcenter.transform.position.x, theCamera.transform.position.y, theCamera.transform.position.z);
            thePlayer.transform.position = target.transform.position;

        }
    }


}
