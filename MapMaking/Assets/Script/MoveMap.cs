using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveMap : MonoBehaviour
{
    // Start is called before the first frame update
    public string transferMapName; //이동할 맵이름       
    
    private bool isclear;  //클리어 확인용
    private PlayerController thePlayer; 
    private Camera theCamera;
    private GameObject cameraTarget;// 맵이동 카메라 타겟 설정
    private GameObject target; // 맵이동 플레이어 타겟 설정

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();//플레이어 객체 가져옴
        theCamera = FindObjectOfType<Camera>();//카메라 가져옴
        
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isclear = GameObject.Find("RoomManager").GetComponent<RoomManager>().isclear;//클리어 상태 확인용으로 가져옴
        if (collision.gameObject.name == "Player"&&isclear==true) //플레이어가 콜라이더 닿고,클리어상태일때
        {
            thePlayer.currentMapName = transferMapName; //플레이어가 이동할 맵이름 설정
            cameraTarget = GameObject.Find(transferMapName);//카메라가 이동할 타겟 값설정
            
            //플레이어가 이동할 위치(문의 좌표)가져옴
            target = GameObject.Find(transferMapName+"/Entrance");//가져온 맵이름이랑 같은 오브젝트 자식중 Entrance를 찾아서 가져옴

            print(transferMapName + "으로 이동");//확인용 
            theCamera.transform.position = new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y, theCamera.transform.position.z);//카메라 이동
            if (transferMapName == "bossmap")
            {
                theCamera.orthographicSize = 15; // 이 부분 12에서 15로 높이면 미니맵 카메라 SIZE 바뀌는데 15정도가 딱 맞음
            }
            else if (GameObject.Find(transferMapName).tag == "vertical_room")//세로방
            {
                theCamera.orthographicSize = 12;
            }
            else if (GameObject.Find(transferMapName).tag == "horizontal_room")//가로방
            {
                theCamera.orthographicSize = 10;
            }
            thePlayer.transform.position = target.transform.position;//가져온 스폰위치에다 캐릭터 이동

        }
    }


}
