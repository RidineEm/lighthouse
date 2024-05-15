using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    static  public Playercontroller instance;
    Rigidbody2D playerRigidbody;
    BoxCollider2D boxCollider;
    public string currentMapName;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();

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
