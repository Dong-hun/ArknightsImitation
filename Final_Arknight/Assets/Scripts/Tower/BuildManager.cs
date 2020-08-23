using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{

    public int m_TileX;
    public int m_TileY;
    
    //첫번째 타워 생성
    public GameObject obj1;
    //두번째 타워 생성
    public GameObject obj2;
    //세번째 타워 생성
    public GameObject obj3;

    //NodeManager스크립트의 함수,변수들 사용하기 위해 선언
    public NodeManager m_NodeMng;

    //ButtonUI 스크립트 함수,변수들 사용하기 위해 선언 (ButtonUI 스크립트에서 UI On,Off함수 호출위해 사용)
    public ButtonUI m_Button;
    


    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTileNumber(int x, int y)
    {
        m_TileX = x;
        m_TileY = y;
    }

    //1번째 타워 생성함수 (UI클릭이 일어날때 Onclick 에서발생)
    public void BuildToTower1()
    {

       //BuildToTower버튼 실행됐을때 노드 m_TileState를 Tower로 변경 
        m_NodeMng.m_TileState[m_NodeMng.NTileY, m_NodeMng.NTileX] = NodeManager.TILEINFO.TOWER;

        
        //타워 생성
        obj1 = Instantiate(Resources.Load("WhiteCube")) as GameObject;

        //타워 좌표= 노드 좌표
        obj1.GetComponent<BasicTower>().m_TileX = m_NodeMng.NTileX;
        obj1.GetComponent<BasicTower>().m_TileY = m_NodeMng.NTileY;


        //타워 생성위치
        Vector3 pos = m_NodeMng.Hit;
        
        obj1.transform.position = pos;

       
        //타워 생성 함수 비활성화
        m_Button.BuildOffButton();


    }

    //2번째 타워 생성함수 (UI클릭이 일어날때 Onclick 에서발생)
    public void BuildToTower2()
    {

       
        m_NodeMng.m_TileState[m_NodeMng.NTileY, m_NodeMng.NTileX] = NodeManager.TILEINFO.TOWER;

        


        obj2 = Instantiate(Resources.Load("RedCube")) as GameObject;

        obj2.GetComponent<BasicTower>().m_TileX = m_NodeMng.NTileX;
        obj2.GetComponent<BasicTower>().m_TileY = m_NodeMng.NTileY;

        // 해당 타일에 배치
        Vector3 pos = m_NodeMng.Hit;
       
        obj2.transform.position = pos;

        //타워 생성 함수 비활성화
        m_Button.BuildOffButton();

    }

    //3번쨰타워생성함수 (UI클릭이 일어날때 Onclick 에서발생)
    public void BuildToTower3()
    {
        m_NodeMng.m_TileState[m_NodeMng.NTileY, m_NodeMng.NTileX] = NodeManager.TILEINFO.TOWER;
        

        obj3 = Instantiate(Resources.Load("BlackCube")) as GameObject;

        obj3.GetComponent<BasicTower>().m_TileX = m_NodeMng.NTileX;
        obj3.GetComponent<BasicTower>().m_TileY = m_NodeMng.NTileY;

        // 해당 타일에 배치
        Vector3 pos = m_NodeMng.Hit;
        
        obj3.transform.position = pos;

        //타워 생성 함수 비활성화
        m_Button.BuildOffButton();

    }

    //타워생성함수
    public void TowerUpgrade()
    {
        //타워 업그레이드,삭제 UI비활성화
        m_Button.TowerOffBtn();
    }

    //타워삭제함수
    public void TowerDelete()
    {




        //삭제되는 타워위치 보기위해 디버그 해놓음(지워도 상관없음)
        Debug.LogError(m_NodeMng.NTileY + "::" + m_NodeMng.NTileX);
        
        //타워가 삭제되면 노드 m_TileState를 NONE으로 변경
        m_NodeMng.m_TileState[m_NodeMng.NTileY, m_NodeMng.NTileX] = NodeManager.TILEINFO.NONE;
        
        //타워 업그레이드,삭제 UI비활성화
        m_Button.TowerOffBtn();

        Destroy(m_NodeMng.m_Tower);

    }
}
