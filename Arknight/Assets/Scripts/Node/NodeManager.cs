using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeManager : MonoBehaviour
{
    // 타일맵 관리 클래스
    // 해당 타일의 상태 (타워의 설치유무, 일반땅, 지형, 생성지나 도착지 등) 관리

    public enum TILESTATE                   // 타일 상태
    {
        NONE, TOWER, OBSTACLE, STRUCTURE    // 기본, 타워, 장애물, 구조물(지형)
    }

    public TILESTATE[,] m_State;            // 타일 상태 담은 배열

    public Node[] m_NodeArr;

    int MaxTileX = 10;                      // 타일 총 가로 갯수
    int MaxTileY = 9;                       // 타일 총 세로 갯수

    void Start()
    {
        m_NodeArr = gameObject.GetComponentsInChildren<Node>();                        // 배열에 노드 싹다 담아줌

        // 타일 배열로 저장
        m_State = new TILESTATE[MaxTileY, MaxTileX];

        // NONE으로 초기화, 각 노드에 번호 부여
        for (int i = 0; i < MaxTileY; ++i)
        {
            for (int j = 0; j < MaxTileX; ++j)
            {
                m_State[i, j] = TILESTATE.NONE;
                m_NodeArr[(i * 10) + j].GetComponent<Node>().SetTileNumber(j, i);
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
                    int tileX = hit.transform.GetComponent<Node>().m_TileX;
                    int tileY = hit.transform.GetComponent<Node>().m_TileY;

                    // 가져온 번호의 타일 상태가 NONE이라면
                    if (m_State[tileY, tileX] == TILESTATE.NONE)
                    {
                        // 타워 생성
                        GameObject obj = Instantiate(Resources.Load("SwordChan")) as GameObject;
                        obj.GetComponent<Tower>().SetTileNumber(tileX, tileY);

                        // 해당 타일에 배치
                        Vector3 pos = hit.transform.position;
                        //pos.y = 2.5f;
                        obj.transform.position = pos;

                        // 해당 타일을 TOWER상태로 변경
                        m_State[tileY, tileX] = TILESTATE.TOWER;
                    }
                    // 그외면 리턴
                    else
                        return;
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Tower"))
                {
                    int tileX = hit.transform.gameObject.GetComponent<Tower>().m_TileX;
                    int tileY = hit.transform.gameObject.GetComponent<Tower>().m_TileY;
                    Destroy(hit.transform.gameObject);
                    m_State[tileY, tileX] = TILESTATE.NONE;
                }
                
                //if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                //{
                //    GameObject obj = Instantiate(Resources.Load("Obstacle")) as GameObject;
                //    Vector3 pos = hit.transform.position;
                //    pos.y = 2.5f;
                //    obj.transform.position = pos;
                //    m_State[hitTileY, hitTileX] = TILESTATE.TOWER;
                //
                //}
                //else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Tower"))
                //{
                //    Destroy(hit.transform.gameObject);
                //}
            }
        }
    }
}
