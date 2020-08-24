﻿using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodeManager : MonoBehaviour
{
    // 타일맵 관리 클래스
    // 해당 타일의 상태 (타워의 설치유무, 일반땅, 지형, 생성지나 도착지 등) 관리
    public enum TILEINFO                           // 타일 상태
    {
        NONE, TOWER, OBSTACLE, STRUCTURE            // 기본, 타워, 장애물, 구조물(지형)
    }
    public TILEINFO[,] m_TileState;                // 타일 상태 담은 배열
    public Node[] m_NodeArr;                        // 노드 담길 배열

    public Vector3 Hit;                            //BuildManager 스크립트에서 타워위치보내기위해 사용
    public ButtonUI m_Button;                      //ButtonUI 스크립트에서 UI On,Off함수 호출위해 사용

    public GameObject m_Tower;                     //타워 delete하기위해 선택된object를 m_Tower에 넣어준다.

    int MaxTileX = 10;                              // 타일 총 가로 갯수
    int MaxTileY = 9;                               // 타일 총 세로 갯수

    public GameObject m_SelectObject;               // 마우스 클릭시 오브젝트 담는 변수

    void Start()
    {
        // 배열에 노드 싹다 담아줌\
        m_NodeArr = gameObject.GetComponentsInChildren<Node>();


        // 타일 배열로 저장
        m_TileState = new TILEINFO[MaxTileY, MaxTileX];

        // 각 노드를 NONE으로 초기화 후에 번호 부여
        for (int i = 0; i < MaxTileY; ++i)
        {
            for (int j = 0; j < MaxTileX; ++j)
            {
                m_TileState[i, j] = TILEINFO.NONE;
                m_NodeArr[(i * 10) + j].GetComponent<Node>().TileX = j;
                m_NodeArr[(i * 10) + j].GetComponent<Node>().TileY = i;
            }
        }

        // 담아줄 오브젝트 초기화
        m_SelectObject = null;

    }

    // Update is called once per frame
    void Update()
    {


        // 마우스 좌클릭 시
        if (Input.GetMouseButtonDown(0))
        {
            //ispointerovergameobject가 false일때: 마우스가 안겹칠때 (ispointerovergameobject가 true일때: pointer가 ui위에 있을때)

            //ui랑 ground랑 겹칠때 Build()함수가 실행 안된다.
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                Build();
            }
        }
    }



    public void Build()
    {
        // 카메라에서 ray 발사
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 레이에 맞은 것들
        RaycastHit hit;

        // 레이에 맞은 것 싹다 조사
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // 맞은 오브젝트 담아줌
            m_SelectObject = hit.transform.gameObject;

            // 맞은 오브젝트의 레이어가 "Ground"라면 (노드라면)
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                int TileX = hit.transform.gameObject.GetComponent<Node>().TileX;
                int TileY = hit.transform.gameObject.GetComponent<Node>().TileY;

                // 해당 노드의 타일 번호 가져옴
                //int tileX = hit.transform.GetComponent<Node>().TileX;
                //int tileY = hit.transform.GetComponent<Node>().TileY;
                //
                ////해당 노드 타일번호를 console로 보기위해 임의로 넣어놓음(지워도 상관없음)
                //Debug.Log(m_TileState[tileY, tileX].ToString() + tileY + "" + tileX);
                //
                ////노드 tileY값과 tileX값을 NTileY,NTileX 에 넣어줌 (나중에 BuildManager 스크립트에서 m_TileState를 변경하기 위해사용)
                //NTileY = tileY;
                //NTileX = tileX;
                //
                ////BuildManager 스크립트에서 선택한 타워위치보내기위해 사용
                //Hit = hit.transform.position;
                //
                // 가져온 번호의 타일 상태가 NONE이라면
                if (m_TileState[TileY, TileX] == TILEINFO.NONE)
                {
                    Debug.Log(TileX + ", " + TileY);
                    //업그레이드, 삭제 UI비활성화
                    m_Button.TowerOffBtn();

                    //타워생성 버튼 UI활성화O
                    m_Button.BuildOnButton();

                    //버튼 클릭하면 -> BuildManager 스크립트에서  build(생성)함수 실행-> m_TileState를 Tower로 변경
                }
                //이건 혹시몰라서 만들어 놨어요
                else if (m_TileState[TileY, TileX] == TILEINFO.TOWER)
                {
                    m_Button.BuildOffButton();
                }
                // 그외면 리턴Hit
                else
                    return;
            }


            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Tower")) // 나중에 이 레이어를 각 타워별로 elseif 만들어야됨
            {
                //충돌한 타워의 번호를 가져온다.(담는다)
                int TileX = hit.transform.gameObject.GetComponent<BasicTower>().m_TileX;
                int TileY = hit.transform.gameObject.GetComponent<BasicTower>().m_TileY;

                if (m_TileState[TileY, TileX] == TILEINFO.TOWER)
                {
                    //업그레이드, 삭제 UI활성화
                    m_Button.TowerOnBtn();

                    //타워 설치 UI 비활성화
                    m_Button.BuildOffButton();
                }

                //이건 혹시몰라서 만들어 놨어요
                else if (m_TileState[TileY, TileX] == TILEINFO.NONE)
                {
                    return;
                }
            }
        }
    }

    Node GetNode(int x, int y)
    {
        return m_NodeArr[(y * 10) + x];
    }
}

