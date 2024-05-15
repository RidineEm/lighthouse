using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveMap : MonoBehaviour
{
    // Start is called before the first frame update
    public string transferMapName; //이동할 맵이름       
    public Transform target; // 이동할 타겟 설정

    private Playercontroller thePlayer; 
    private Camera theCamera;
    public GameObject mapcenter;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<Playercontroller>();
        theCamera = FindObjectOfType<Camera>();
        
    }

    // 박스 콜라이더에 닿는 순간 이벤트 발생
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.name == "Player")
        {
            thePlayer.currentMapName = transferMapName;
            mapcenter = GameObject.Find(transferMapName);
            print(transferMapName + "으로 이동");
            theCamera.transform.position = new Vector3(mapcenter.transform.position.x, theCamera.transform.position.y, theCamera.transform.position.z);
            thePlayer.transform.position = target.transform.position;

        }
    }


}
