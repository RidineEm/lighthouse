using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    
    public SpriteRenderer Img_Renderer;
    public Sprite opened;
    public Sprite closed;
    public GameObject roomManager;
    private bool isclear;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        isclear = roomManager.GetComponent<RoomManager>().isclear; //Ŭ���� ����Ȯ���ؼ�
        if (!isclear)//Ŭ���� �ƴϸ� ������
        {
            Img_Renderer.sprite = closed;
        }
        else if (isclear)//Ŭ����� ������
        {
            Img_Renderer.sprite = opened;
        }
            
    }
}
