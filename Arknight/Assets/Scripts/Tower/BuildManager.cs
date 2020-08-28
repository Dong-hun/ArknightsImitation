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

    public List<Obstacle> m_ObstacleList;

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
        obj1 = Instantiate(Resources.Load("Tower/BasicTower")) as GameObject;
        obj1.transform.position = m_NodeMng.SelectObject.transform.position;
        obj1.gameObject.GetComponent<BasicTower>().TileX = TileX;
        obj1.gameObject.GetComponent<BasicTower>().TileY = TileY;

        // 해당 좌표 타워로 변경
        m_NodeMng.m_TileState[TileY, TileX] = NodeManager.TILEINFO.TOWER;

        // 색깔 되돌리기
        m_NodeMng.GetNode(TileX, TileY).GetComponent<MeshRenderer>().material = Resources.Load("Tower/Material/Grass") as Material;

        // 설치가 끝나면 선택된 오브젝트 null로 초기화
        m_NodeMng.m_PrevNode = null;
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
        obj2 = Instantiate(Resources.Load("Tower/SecondTower")) as GameObject;
        obj2.transform.position = m_NodeMng.SelectObject.transform.position;
        obj2.gameObject.GetComponent<BasicTower>().TileX = TileX;
        obj2.gameObject.GetComponent<BasicTower>().TileY = TileY;

        // 해당 좌표 타워로 변경
        m_NodeMng.m_TileState[TileY, TileX] = NodeManager.TILEINFO.TOWER;

        // 색깔 되돌리기
        m_NodeMng.GetNode(TileX, TileY).GetComponent<MeshRenderer>().material = Resources.Load("Tower/Material/Grass") as Material;

        // 설치가 끝나면 선택된 오브젝트 null로 초기화
        m_NodeMng.m_PrevNode = null;
        m_NodeMng.SelectObject = null;

        m_Button.BuildOffButton();

        //삭제되는 타워위치 보기위해 디버그 해놓음(지워도 상관없음)
        Debug.Log("Select : " + TileY + ", " + TileX);
    }

    //장애물타워생성함수 (UI클릭이 일어날때 Onclick 에서발생)
    public void BuildToObstacle()
    {
        // 노드 좌표값 노드에서 가져옴
        int TileX = m_NodeMng.SelectObject.gameObject.GetComponent<Node>().TileX;
        int TileY = m_NodeMng.SelectObject.gameObject.GetComponent<Node>().TileY;

<<<<<<< HEAD:Arknight/Assets/Scripts/MainScene/Tower/BuildManager.cs
        // 선택된 타일에 깔고 타워가 묻히지 않게 하기 위해 타일 포지션값 가져온 뒤에 보정함
        Vector3 pos = m_NodeMng.SelectObject.transform.position;
        pos.y = 1.5f;

        // 보정된 위치에 타워 생성 후 타일정보 저장
        obj3 = Instantiate(Resources.Load("Tower/Obstacle")) as GameObject;
        obj3.transform.position = pos;
        obj3.gameObject.GetComponent<Obstacle>().TileX = TileX;
        obj3.gameObject.GetComponent<Obstacle>().TileY = TileY;

        // 리스트에 추가
        m_ObstacleList.Add(obj3.GetComponent<Obstacle>());
=======
        // 타워 생성 후 타일정보 저장
        obj3 = Instantiate(Resources.Load("DaggerTower")) as GameObject;
        obj3.transform.position = m_NodeMng.m_SelectObject.transform.position;
        obj3.gameObject.GetComponent<BasicTower>().SetTileNumber(TileX, TileY);
>>>>>>> e8fd898eee8f41673716ee97de14823cd2db1fda:Arknight/Assets/Scripts/Tower/BuildManager.cs

        // 해당 좌표 타워로 변경
        m_NodeMng.m_TileState[TileY, TileX] = NodeManager.TILEINFO.TOWER;

        // 색깔 되돌리기
        m_NodeMng.GetNode(TileX, TileY).GetComponent<MeshRenderer>().material = Resources.Load("Tower/Material/Grass") as Material;

        // 설치가 끝나면 선택된 오브젝트 null로 초기화
        m_NodeMng.m_PrevNode = null;
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
        int TileX = 0;      // 해당 타워가 설치된 좌표X
        int TileY = 0;      // 해당 타워가 설치된 좌표Y

        // 선택된 오브젝트가 타워일때
        if (m_NodeMng.SelectObject.layer == LayerMask.NameToLayer("Tower"))
        {
            // 해당 노드 좌표 타워에서 가져옴
            TileX = m_NodeMng.SelectObject.gameObject.GetComponent<BasicTower>().TileX;
            TileY = m_NodeMng.SelectObject.gameObject.GetComponent<BasicTower>().TileY;
        }
        else if (m_NodeMng.SelectObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            // 해당 노드 좌표 가져옴 (Obstacle)
            TileX = m_NodeMng.SelectObject.gameObject.GetComponent<Obstacle>().TileX;
            TileY = m_NodeMng.SelectObject.gameObject.GetComponent<Obstacle>().TileY;

            // 리스트 돌아서
            for (int i = 0; i < m_ObstacleList.Count; ++i)
            {
                // 선택된 오브젝트(장애물)가 리스트의 번호와 같으면
                if (m_ObstacleList[i].transform == m_NodeMng.SelectObject.transform)
                {
                    // 리스트안의 해당 오브젝트 제거
                    m_ObstacleList.Remove(m_ObstacleList[i]);
                    break;
                }
            }
        }
        // 타워가 삭제되면 노드 m_TileState를 NONE으로 변경
        m_NodeMng.m_TileState[TileY, TileX] = NodeManager.TILEINFO.NONE;

        // 타워 업그레이드,삭제 UI비활성화
        m_Button.TowerOffBtn();

        // 오브젝트 파괴
        Destroy(m_NodeMng.SelectObject);

        //설치가 끝나면 선택된 오브젝트 null로 초기화
        m_NodeMng.GetNode(TileX, TileY).GetComponent<MeshRenderer>().material = Resources.Load("Tower/Material/Grass") as Material;
        m_NodeMng.m_PrevNode = null;
        m_NodeMng.SelectObject = null;
    }
}
