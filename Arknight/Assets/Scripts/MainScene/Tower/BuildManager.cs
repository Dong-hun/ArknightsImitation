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
    
    // 


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
        // 노드 좌표값 노드에서 가져옴
        int TileX = m_NodeMng.SelectObject.gameObject.GetComponent<Node>().TileX;
        int TileY = m_NodeMng.SelectObject.gameObject.GetComponent<Node>().TileY;

        // 타워 생성 후 타일정보 저장
        obj1 = Instantiate(Resources.Load("BasicTower")) as GameObject;
        obj1.transform.position = m_NodeMng.SelectObject.transform.position;
        obj1.gameObject.GetComponent<BasicTower>().SetTileNumber(TileX, TileY);

        // 해당 좌표 타워로 변경
        m_NodeMng.m_TileState[TileY, TileX] = NodeManager.TILEINFO.TOWER;

        // 설치가 끝나면 선택된 오브젝트 null로 초기화
        m_NodeMng.SelectObject = null;

        m_Button.BuildOffButton();

        //삭제되는 타워위치 보기위해 디버그 해놓음(지워도 상관없음)
        Debug.Log("Select : " + TileY + ", " + TileX);
    }

    //2번째 타워 생성함수 (UI클릭이 일어날때 Onclick 에서발생)
    public void BuildToTower2()
    {
        // 노드 좌표값 노드에서 가져옴
        int TileX = m_NodeMng.SelectObject.gameObject.GetComponent<Node>().TileX;
        int TileY = m_NodeMng.SelectObject.gameObject.GetComponent<Node>().TileY;

        // 타워 생성 후 타일정보 저장
        obj2 = Instantiate(Resources.Load("SecondTower")) as GameObject;
        obj2.transform.position = m_NodeMng.SelectObject.transform.position;
        obj2.gameObject.GetComponent<BasicTower>().SetTileNumber(TileX, TileY);

        // 해당 좌표 타워로 변경
        m_NodeMng.m_TileState[TileY, TileX] = NodeManager.TILEINFO.TOWER;

        // 설치가 끝나면 선택된 오브젝트 null로 초기화
        m_NodeMng.SelectObject = null;

        m_Button.BuildOffButton();

        //삭제되는 타워위치 보기위해 디버그 해놓음(지워도 상관없음)
        Debug.Log("Select : " + TileY + ", " + TileX);
    }

    //3번쨰타워생성함수 (UI클릭이 일어날때 Onclick 에서발생)
    public void BuildToTower3()
    {
        // 노드 좌표값 노드에서 가져옴
        int TileX = m_NodeMng.SelectObject.gameObject.GetComponent<Node>().TileX;
        int TileY = m_NodeMng.SelectObject.gameObject.GetComponent<Node>().TileY;

        // 타워 생성 후 타일정보 저장
        obj3 = Instantiate(Resources.Load("DaggerTower")) as GameObject;
        obj3.transform.position = m_NodeMng.SelectObject.transform.position;
        obj3.gameObject.GetComponent<BasicTower>().SetTileNumber(TileX, TileY);

        // 해당 좌표 타워로 변경
        m_NodeMng.m_TileState[TileY, TileX] = NodeManager.TILEINFO.TOWER;

        // 설치가 끝나면 선택된 오브젝트 null로 초기화
        m_NodeMng.SelectObject = null;

        m_Button.BuildOffButton();

        //삭제되는 타워위치 보기위해 디버그 해놓음(지워도 상관없음)
        Debug.Log("Select : " + TileY + ", " + TileX);
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
        // 해당 노드 좌표 타워에서 가져옴
        int TileX = m_NodeMng.SelectObject.gameObject.GetComponent<BasicTower>().TileX;
        int TileY = m_NodeMng.SelectObject.gameObject.GetComponent<BasicTower>().TileY;

        //삭제되는 타워위치 보기위해 디버그 해놓음(지워도 상관없음)
        Debug.Log("Delete : " + TileY + ", " + TileX);

        //타워가 삭제되면 노드 m_TileState를 NONE으로 변경
        m_NodeMng.m_TileState[TileY, TileX] = NodeManager.TILEINFO.NONE;

        //타워 업그레이드,삭제 UI비활성화
        m_Button.TowerOffBtn();

        Destroy(m_NodeMng.SelectObject);

        //설치가 끝나면 선택된 오브젝트 null로 초기화
        m_NodeMng.SelectObject = null;
    }
}
