using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPostion : MonoBehaviour
{
    public string startPoint; // 이동되어온 맵이름을 체크하기 위한 변수
    private PlayerController thePlayer; // 캐릭터 객체 가져오기 위한 변수
  
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>(); // 캐릭터 변수에 현재 캐릭터 객체를 할당
        if (startPoint == thePlayer.currentMapName)
        {

            
            // 캐릭터 이동
            thePlayer.transform.position = this.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
