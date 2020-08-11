using System.Collections;
using System.Collections.Generic;
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

    int MaxTileX = 10;                      // 타일 가로 갯수
    int MaxTileY = 9;                       // 타일 세로 갯수

    void Start()
    {
        // 타일 배열로 저장
        m_State = new TILESTATE[MaxTileY, MaxTileX];

        // 우선 NONE으로 초기화
        for(int i = 0; i < MaxTileY; ++i)
        {
            for(int j = 0; j < MaxTileX; ++j)
            {
                m_State[i, j] = TILESTATE.NONE;
                Debug.Log(m_State[i, j]);
                //Debug.Log(m_State.Length); // 80;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    GameObject obj = Instantiate(Resources.Load("RedCube")) as GameObject;

                    Vector3 pos = hit.transform.position;
                    pos.y = obj.transform.localScale.y / 2;

                    obj.transform.position = pos;
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Tower"))
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }
}
