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
        isclear = roomManager.GetComponent<RoomManager>().isclear; //클리어 상태확인해서
        if (!isclear)//클리어 아니면 문닫음
        {
            Img_Renderer.sprite = closed;
        }
        else if (isclear)//클리어면 문열음
        {
            Img_Renderer.sprite = opened;
        }
            
    }
}
