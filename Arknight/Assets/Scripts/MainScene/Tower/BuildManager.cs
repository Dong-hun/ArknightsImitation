﻿using System.Collections;
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
    public List<GameObject> m_TowerList;

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
        obj1 = Instantiate(Resources.Load("Tower/Basic/BasicTower")) as GameObject;
        obj1.transform.position = m_NodeMng.SelectObject.transform.position;
        obj1.gameObject.GetComponent<BasicTower>().TileX = TileX;
        obj1.gameObject.GetComponent<BasicTower>().TileY = TileY;

        // 타워 리스트에 저장
        m_TowerList.Add(obj1);

        // 해당 좌표 타워로 변경
        m_NodeMng.m_TileState[TileY, TileX] = NodeManager.TILEINFO.TOWER;

        // 주변 힐타워의 AroundList(힐 할 주변 대상담는 리스트)에 담아줌
        CheckAroundHealTower();

        // 현재 노드의 머터리얼을 다시 Grass로 변경
        m_NodeMng.GetNode(TileX, TileY).GetComponent<MeshRenderer>().material = Resources.Load("Tower/Material/Grass") as Material;

        // 설치가 끝나면 선택된 오브젝트 null로 초기화
        m_NodeMng.m_PrevObject = null;
        m_NodeMng.SelectObject = null;

        m_Button.BuildOffButton();
        
    }

    //2번째 타워 생성함수 (UI클릭이 일어날때 Onclick 에서발생)
    public void BuildToHealTower()
    {
        // 노드 좌표값 노드에서 가져옴
        int TileX = m_NodeMng.SelectObject.gameObject.GetComponent<Node>().TileX;
        int TileY = m_NodeMng.SelectObject.gameObject.GetComponent<Node>().TileY;

        // 타워 생성 후 타일정보 저장
        obj2 = Instantiate(Resources.Load("Tower/Heal/HealTower")) as GameObject;
        obj2.transform.position = m_NodeMng.SelectObject.transform.position;
        obj2.gameObject.GetComponent<HealTower>().TileX = TileX;
        obj2.gameObject.GetComponent<HealTower>().TileY = TileY;

        //타워 리스트에 저장
        m_TowerList.Add(obj2);

        // 해당 좌표 타워로 변경
        m_NodeMng.m_TileState[TileY, TileX] = NodeManager.TILEINFO.TOWER;

        // 주변 힐타워의 AroundList(힐 할 주변 대상담는 리스트)에 담아줌
        CheckAroundHealTower();

        // 현재 노드의 머터리얼을 다시 Grass로 변경
        m_NodeMng.GetNode(TileX, TileY).GetComponent<MeshRenderer>().material = Resources.Load("Tower/Material/Grass") as Material;

        // 설치가 끝나면 선택된 오브젝트 null로 초기화
        m_NodeMng.m_PrevObject = null;
        m_NodeMng.SelectObject = null;

        m_Button.BuildOffButton();

    }

    //장애물타워생성함수 (UI클릭이 일어날때 Onclick 에서발생)
    public void BuildToObstacle()
    {
        // 노드 좌표값 노드에서 가져옴
        int TileX = m_NodeMng.SelectObject.gameObject.GetComponent<Node>().TileX;
        int TileY = m_NodeMng.SelectObject.gameObject.GetComponent<Node>().TileY;

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

        // 해당 좌표 타워로 변경
        m_NodeMng.m_TileState[TileY, TileX] = NodeManager.TILEINFO.OBSTACLE;

        // 현재 노드의 머터리얼을 다시 Grass로 변경
        m_NodeMng.GetNode(TileX, TileY).GetComponent<MeshRenderer>().material = Resources.Load("Tower/Material/Grass") as Material;

        // 설치가 끝나면 선택된 오브젝트 null로 초기화
        m_NodeMng.m_PrevObject = null;
        m_NodeMng.SelectObject = null;

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
        int TileX = 0;      // 해당 타워가 설치된 좌표X
        int TileY = 0;      // 해당 타워가 설치된 좌표Y

        // 선택된 오브젝트가 기본타워일때
        if (m_NodeMng.SelectObject.layer == LayerMask.NameToLayer("BasicTower"))
        {
            // 해당 노드 좌표 타워에서 가져옴
            TileX = m_NodeMng.SelectObject.gameObject.GetComponent<BasicTower>().TileX;
            TileY = m_NodeMng.SelectObject.gameObject.GetComponent<BasicTower>().TileY;

            // 리스트 돌아서
            for (int i = 0; i < m_TowerList.Count; ++i)
            {
                // 선택된 오브젝트(타워)가 리스트의 번호와 같으면
                if (m_TowerList[i].transform == m_NodeMng.SelectObject.transform)
                {
                    // 리스트안의 해당 오브젝트 제거
                    m_TowerList.Remove(m_TowerList[i]);
                    break;
                }
            }

            
        }
        // 선택된 오브젝트가 힐타워일때
        else if (m_NodeMng.SelectObject.layer == LayerMask.NameToLayer("HealTower"))
        {
            // 해당 노드 좌표 타워에서 가져옴
            TileX = m_NodeMng.SelectObject.gameObject.GetComponent<HealTower>().TileX;
            TileY = m_NodeMng.SelectObject.gameObject.GetComponent<HealTower>().TileY;

            // 리스트 돌아서
            for (int i = 0; i < m_TowerList.Count; ++i)
            {
                // 선택된 오브젝트(타워)가 리스트의 번호와 같으면
                if (m_TowerList[i].transform == m_NodeMng.SelectObject.transform)
                {
                    // 리스트안의 해당 오브젝트 제거
                    m_TowerList.Remove(m_TowerList[i]);
                    break;
                }
            }
        }
        // 선택된 오브젝트가 장애물일때
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

        // 힐타워의 주변타워리스트에 해당 타워들이 담겼는지 조사해서 리스트에서도 삭제
        for(int i = 0; i < m_TowerList.Count; ++i)
        {
            if(m_TowerList[i].layer == LayerMask.NameToLayer("HealTower"))
            {
                m_TowerList[i].GetComponent<HealTower>().m_DelDeleteTower?.Invoke(m_NodeMng.SelectObject);
            }
        }



        // 타워가 삭제되면 노드 m_TileState를 NONE으로 변경
        m_NodeMng.m_TileState[TileY, TileX] = NodeManager.TILEINFO.NONE;

        // 타워 업그레이드,삭제 UI비활성화
        m_Button.TowerOffBtn();

        // 오브젝트 파괴
        Destroy(m_NodeMng.SelectObject);

        // 현재 노드의 머터리얼을 다시 Grass로 변경
        m_NodeMng.GetNode(TileX, TileY).GetComponent<MeshRenderer>().material = Resources.Load("Tower/Material/Grass") as Material;

        //설치가 끝나면 선택된 오브젝트와 이전 노드 null로 초기화
        m_NodeMng.m_PrevObject = null;
        m_NodeMng.SelectObject = null;
    }

    // 좌표로 타워 가져오기
    public GameObject GetTowerCoordinates(int x, int y)
    {
        // 타워리스트가 비었다면 리턴
        if (m_TowerList.Count == 0) return null;

        // 타워 전부 조사
        for(int i = 0; i < m_TowerList.Count; ++i)
        {
            // 타일 넘버
            int TileX = 0;
            int TileY = 0;

            // 해당 타워의 레이어별로 스크립트에서 좌표 가져오기
            if(m_TowerList[i].layer == LayerMask.NameToLayer("BasicTower"))
            {
                TileX = m_TowerList[i].GetComponent<BasicTower>().TileX;
                TileY = m_TowerList[i].GetComponent<BasicTower>().TileY;
            }
            else if (m_TowerList[i].layer == LayerMask.NameToLayer("HealTower"))
            {
                TileX = m_TowerList[i].GetComponent<HealTower>().TileX;
                TileY = m_TowerList[i].GetComponent<HealTower>().TileY;
            }

            // 가져온 타워의 좌표와 얻으려는 타일좌표가 일치하면
            if(TileX == x && TileY == y)
            {
                // 해당 좌표의 타워 리턴
                return m_TowerList[i];
            }
        }
        return null;
    }

    // 설치가 되면 주변에 힐타워가 있는지 조사하는 함수
    void CheckAroundHealTower()
    {
        // 타워리스트가 비었다면 리턴
        if (m_TowerList.Count == 0) return;

        // 타워 리스트 원소갯수만큼 돌아서
        for(int i = 0; i < m_TowerList.Count; ++i)
        {
            // 해당 원소가 힐타워라면 힐타워의 AroundList에 더해지는 델리게이트 실행
            if (m_TowerList[i].layer == LayerMask.NameToLayer("HealTower"))
                m_TowerList[i].GetComponent<HealTower>().m_DelAddTower?.Invoke();
        }
    }
}