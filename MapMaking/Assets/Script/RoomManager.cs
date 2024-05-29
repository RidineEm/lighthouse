using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public bool isclear;//클리어 확인
    public GameObject clearbutton;
    public GameObject notclearbutton;
    public List<GameObject> nums = new List<GameObject>();
    public GameObject StartRoom;
    public GameObject room1;
    public GameObject room2;
    public GameObject room3;
    public GameObject room4;
    public GameObject room5;
    public GameObject room6;

    public Transform exit0_1;
    public Transform exit0_2;
    public Transform exit1_1;
    public Transform exit1_2;
    public Transform exit2_1;
    public Transform exit2_2;
    public Transform exit3_1;
    public Transform exit3_2;
    public Transform exit4_1;
    public Transform exit4_2;
    public Transform exit5_1;
    public Transform exit5_2;
    public Transform exit6_1;
    public Transform exit6_2;

    public MoveMap mapname1;
    public MoveMap mapname2;


    // Start is called before the first frame update
    void Start()
    {
        isclear=false;//처음에는 클리어 false로 설정
        
        for (int i = 0; i < 6; i++)
        {
            int room = Random.Range(0, nums.Count);
            GameObject selectedRoom = nums[room];  // 선택된 값을 변수에 저장
            nums.RemoveAt(room);  // 요소 제거

            switch (i)
            {
                case 0:
                    room1 = selectedRoom;
                    exit1_1 = room1.transform.Find("Exit1");
                    exit1_2 = room1.transform.Find("Exit2");
                    break;
                case 1:
                    room2 = selectedRoom;
                    exit2_1 = room2.transform.Find("Exit1");
                    exit2_2 = room2.transform.Find("Exit2");
                    break;
                case 2:
                    room3 = selectedRoom;
                    exit3_1 = room3.transform.Find("Exit1");
                    exit3_2 = room3.transform.Find("Exit2");
                    break;
                case 3:
                    room4 = selectedRoom;
                    exit4_1 = room4.transform.Find("Exit1");
                    exit4_2 = room4.transform.Find("Exit2");
                    break;
                case 4:
                    room5 = selectedRoom;
                    exit5_1 = room5.transform.Find("Exit1");
                    exit5_2 = room5.transform.Find("Exit2");
                    break;
                case 5:
                    room6 = selectedRoom;
                    exit6_1 = room6.transform.Find("Exit1");
                    exit6_2 = room6.transform.Find("Exit2");
                    break;
                
            }

        }
               for (int i = 0; i < 6; i++)
        {
            switch (i)
            {
                case 0:
                    mapname1 = exit1_1.GetComponent<MoveMap>();
                    mapname2 = exit1_2.GetComponent<MoveMap>();
                    mapname1.transferMapName = room3.name;
                    mapname2.transferMapName = room4.name;
                    break;
                case 1:
                    mapname1 = exit2_1.GetComponent<MoveMap>();
                    mapname2 = exit2_2.GetComponent<MoveMap>();
                    mapname1.transferMapName = room3.name;
                    mapname2.transferMapName = room4.name;
                    break;
                case 2:
                    mapname1 = exit3_1.GetComponent<MoveMap>();
                    mapname2 = exit3_2.GetComponent<MoveMap>();
                    mapname1.transferMapName = room5.name;
                    mapname2.transferMapName = room6.name;
                    break;
                case 3:
                    mapname1 = exit4_1.GetComponent<MoveMap>();
                    mapname2 = exit4_2.GetComponent<MoveMap>();
                    mapname1.transferMapName = room5.name;
                    mapname2.transferMapName = room6.name;
                    break;
                case 4:
                    mapname1 = exit5_1.GetComponent<MoveMap>();
                    mapname2 = exit5_2.GetComponent<MoveMap>();
                    mapname1.transferMapName = "bossmap";
                    mapname2.transferMapName = "bossmap";
                    break;
                case 5:
                    mapname1 = exit6_1.GetComponent<MoveMap>();
                    mapname2 = exit6_2.GetComponent<MoveMap>();
                    mapname1.transferMapName = "bossmap";
                    mapname2.transferMapName = "bossmap";
                    break;

            }
        }
        exit0_1 = StartRoom.transform.Find("Exit1");
        exit0_2 = StartRoom.transform.Find("Exit2");
        mapname1 = exit0_1.GetComponent<MoveMap>();
        mapname2 = exit0_2.GetComponent<MoveMap>();
        mapname1.transferMapName = room1.name;
        mapname2.transferMapName = room2.name;
    }
    

}
