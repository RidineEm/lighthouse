using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPostion : MonoBehaviour
{
    public string startPoint; // �̵��Ǿ�� ���̸��� üũ�ϱ� ���� ����
    private Playercontroller thePlayer; // ĳ���� ��ü �������� ���� ����
  
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<Playercontroller>(); // ĳ���� ������ ���� ĳ���� ��ü�� �Ҵ�
        if (startPoint == thePlayer.currentMapName)
        {

            
            // ĳ���� �̵�
            thePlayer.transform.position = this.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}