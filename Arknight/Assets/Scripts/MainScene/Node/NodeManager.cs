using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeManager : MonoBehaviour
{
    // 타일맵 관리 클래스
    // 해당 타일의 상태 (타워의 설치유무, 일반땅, 지형, 생성지나 도착지 등) 관리

    private enum TILEINFO                           // 타일 상태
    {
        NONE, TOWER, OBSTACLE, STRUCTURE            // 기본, 타워, 장애물, 구조물(지형)
    }
    private TILEINFO[,] m_TileState;                // 타일 상태 담은 배열
    public Node[] m_NodeArr;                        // 노드 담길 배열

    int MaxTileX = 10;                              // 타일 총 가로 갯수
    int MaxTileY = 9;                               // 타일 총 세로 갯수

    void Start()
    {
        // 배열에 노드 싹다 담아줌
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
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 좌클릭 시
        if(Input.GetMouseButtonDown(0))
        {
            // 카메라에서 ray 발사
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 레이에 맞은 것들
            RaycastHit hit;

            // 레이에 맞은 것 싹다 조사
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // 맞은 오브젝트의 레이어가 "Ground"라면 (노드라면)
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    // 해당 노드의 타일 번호 가져옴
                    int tileX = hit.transform.GetComponent<Node>().TileX;
                    int tileY = hit.transform.GetComponent<Node>().TileY;

                    // 가져온 번호의 타일 상태가 NONE이라면
                    if (m_TileState[tileY, tileX] == TILEINFO.NONE)
                    {
                        // 타워 생성
                        GameObject obj = Instantiate(Resources.Load("SwordChan")) as GameObject;
                        obj.GetComponent<Tower>().SetTileNumber(tileX, tileY);

                        // 해당 타일에 배치
                        Vector3 pos = hit.transform.position;
                        //pos.y = 2.5f;
                        obj.transform.position = pos;

                        // 해당 타일을 TOWER상태로 변경
                        m_TileState[tileY, tileX] = TILEINFO.TOWER;
                    }
                    // 그외면 리턴
                    else
                        return;
                }
                // 맞은 오브젝트의 레이어가 "Tower"라면
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Tower"))
                {
                    // 해당 타워가 설치된 타일넘버 가져옴
                    int tileX = hit.transform.gameObject.GetComponent<Tower>().TileX;
                    int tileY = hit.transform.gameObject.GetComponent<Tower>().TileY;

                    // 타워 삭제
                    Destroy(hit.transform.gameObject);

                    // 해당 타일을 NONE로 변경
                    m_TileState[tileY, tileX] = TILEINFO.NONE;
                }
            }
        }
    }

    Node GetNode(int x, int y)
    {
        return m_NodeArr[(y * 10) + x];
    }
}
