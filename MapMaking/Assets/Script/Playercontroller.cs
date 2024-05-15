using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Playercontroller : MonoBehaviour
{
    static  public Playercontroller instance;
    Rigidbody2D playerRigidbody;
    BoxCollider2D boxCollider;
    public string currentMapName;// 맵이동시 사용할 저장용 맵이름
    public RoomManager roomManager;//클리어상태확인용

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        
        //플레이어 맵이동시 버그 방지용
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject); // 게임 오브젝트 파괴금지

            boxCollider = GetComponent<BoxCollider2D>();
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);//캐릭터 2개 초과로 생기는거 방지용
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) //클리어 테스트용
    {
        if (collision.gameObject.name == "clear") //clear 버튼 건들이면
        {
            roomManager.isclear = true; //클리어 상태를 true로 변경
            print(roomManager.isclear);//확인용 print
        }
        else if (collision.gameObject.name == "notclear")//notclear 버튼 건들이면
        {
            roomManager.isclear= false;//클리어 상태를 false로 변경
            print(roomManager.isclear);//확인용 print
        }
    }

    // Update is called once per frame
    void Update()
    {   //간단한 이동구현
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
     
        float xspeed = x * 8f;
        float yspeed = y * 8f;
        Vector3 velo = new Vector3(xspeed, yspeed, 0f);
        playerRigidbody.velocity = velo;
    }
}
