using System.Collections;
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
        NONE, TOWER, OBSTACLE, STRUCTURE           // 기본, 타워, 장애물, 구조물(지형)
    }
    public TILEINFO[,] m_TileState;                // 타일 상태 담은 배열
    public Node[] m_NodeArr;                       // 노드 담길 배열

    public Vector3 Hit;                            //BuildManager 스크립트에서 타워위치보내기위해 사용
    public ButtonUI m_Button;                      //ButtonUI 스크립트에서 UI On,Off함수 호출위해 사용

    int MaxTileX = 10;                             // 타일 총 가로 갯수
    int MaxTileY = 9;                              // 타일 총 세로 갯수

    public GameObject m_SelectObject;              // 마우스 클릭시 오브젝트 담는 변수 (확인용으로 public으로함 확인 다되면 private로 변경할것)
    public GameObject m_PrevObject;                // 이전에 선택된 오브젝트
    public GameObject SelectObject
    {
        set
        {
            m_SelectObject = value;
        }
        get
        {
            return m_SelectObject;
        }
    }


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

        // 담아줄 오브젝트 초기화
        m_SelectObject = null;
        m_PrevObject = null;
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
                ClickCheck();
            }
        }
    }

    // 클릭 체크
    void ClickCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 레이에 맞은 것 싹다 조사
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // 선택된 오브젝트 담아줌
            m_SelectObject = hit.transform.gameObject;

            // 해당 오브젝트에서 가져올 좌표 담을거
            int TileX = 0;
            int TileY = 0;

            // 선택된 오브젝트의 레이어가 땅일경우
            if (m_SelectObject.layer == LayerMask.NameToLayer("Ground"))
            {
                // 해당 오보젝트의 Node컴포넌트에서 좌표 가져옴
                TileX = m_SelectObject.GetComponent<Node>().TileX;
                TileY = m_SelectObject.GetComponent<Node>().TileY;

                if (m_TileState[TileY, TileX] == TILEINFO.TOWER) 
                    return;

                // 노드의 머터리얼을 Blue로 변경(선택됬다는 표시)
                ChangeMaterialNode(TileX, TileY, "Blue");

                // 이전에 저장된 오브젝트가 없다면 바로타워설치하게
                if (m_PrevObject == null)
                {
                    // 이전 오브젝트에 현재 오브젝트 저장
                    m_PrevObject = m_SelectObject;

                    // 타워설치 버튼 활성화
                    m_Button.BuildOnButton();
                    m_Button.TowerOffBtn();
                }
                // 이전에 저장된 오브젝트가 있다면 종류별로 상호작용
                else
                {
                    // 이전의 오브젝트와 현재 오브젝트가 같거나 현재 오브젝트의 레이어가 Water면 선택취소하게 만듦
                    if (m_PrevObject == m_SelectObject ||
                        m_SelectObject.layer == LayerMask.NameToLayer("Water"))
                    {
                        // 현재 노드의 머터리얼을 Grass로 변경
                        ChangeMaterialNode(TileX, TileY, "Grass");

                        // 버튼 싹다 비활성화
                        m_Button.BuildOffButton();
                        m_Button.TowerOffBtn();

                        // 선택된 오브젝트, 이전 오브젝트 null로 초기화
                        m_SelectObject = null;
                        m_PrevObject = null;
                    }
                    // 둘다 아니라면
                    else
                    {
                        // 이전 오브젝트가 Node라면 Node에서 좌표 가져오고 타워 설치 버튼 활성화
                        if (m_PrevObject.layer == LayerMask.NameToLayer("Ground"))
                        {
                            TileX = m_PrevObject.GetComponent<Node>().TileX;
                            TileY = m_PrevObject.GetComponent<Node>().TileY;

                            m_Button.BuildOnButton();
                            m_Button.TowerOffBtn();
                        }
                        // 이전 오브젝트가 Tower라면 각 타워에서 좌표 가져오고 타워 삭제 버튼 활성화 (아래 else if문 전부)
                        else if (m_PrevObject.layer == LayerMask.NameToLayer("BasicTower"))
                        {
                            TileX = m_PrevObject.GetComponent<BasicTower>().TileX;
                            TileY = m_PrevObject.GetComponent<BasicTower>().TileY;

                            m_Button.BuildOnButton();
                            m_Button.TowerOffBtn();
                        }
                        else if (m_PrevObject.layer == LayerMask.NameToLayer("HealTower"))
                        {
                            TileX = m_PrevObject.GetComponent<HealTower>().TileX;
                            TileY = m_PrevObject.GetComponent<HealTower>().TileY;

                            m_Button.BuildOnButton();
                            m_Button.TowerOffBtn();
                        }
                        else if (m_PrevObject.layer == LayerMask.NameToLayer("Obstacle"))
                        {
                            TileX = m_PrevObject.GetComponent<Obstacle>().TileX;
                            TileY = m_PrevObject.GetComponent<Obstacle>().TileY;

                            m_Button.BuildOnButton();
                            m_Button.TowerOffBtn();
                        }

                        // 이전 노드의 머터리얼을 Grass로 초기화
                        ChangeMaterialNode(TileX, TileY, "Grass");

                        // 이전 오브젝트에 현재 오브젝트 넣어줌
                        m_PrevObject = m_SelectObject;
                    }
                }
            }
            // 땅이 아닐 경우
            else
            {
                // 현재 선택된 오브젝트의 레이어 별로 해당 스크립트에서 좌표 가져온 뒤 저장
                if (m_SelectObject.layer == LayerMask.NameToLayer("BasicTower"))
                {
                    // 해당 오보젝트의 Node컴포넌트에서 좌표 가져옴
                    TileX = m_SelectObject.GetComponent<BasicTower>().TileX;
                    TileY = m_SelectObject.GetComponent<BasicTower>().TileY;
                }
                else if (m_SelectObject.layer == LayerMask.NameToLayer("HealTower"))
                {
                    TileX = m_SelectObject.GetComponent<HealTower>().TileX;
                    TileY = m_SelectObject.GetComponent<HealTower>().TileY;
                }
                else if (m_SelectObject.layer == LayerMask.NameToLayer("Obstacle"))
                {
                    TileX = m_SelectObject.GetComponent<Obstacle>().TileX;
                    TileY = m_SelectObject.GetComponent<Obstacle>().TileY;
                }
                // 위의 타워종류가 아니면 리턴
                else
                    return;

                // 해당 좌표를 Red로 설정
                ChangeMaterialNode(TileX, TileY, "Red");

                // 이전에 저장된 오브젝트가 없다면 바로 타워 삭제버튼 활성화
                if (m_PrevObject == null)
                {
                    // 이전 오브젝트에 현재 오브젝트 저장
                    m_PrevObject = m_SelectObject;

                    // 타워 삭제 버튼 활성화
                    m_Button.BuildOffButton();
                    m_Button.TowerOnBtn();
                }
                // 이전에 저장된 오브젝트가 있다면 각 레이어 별로 상호작용
                else
                {
                    // 이전의 오브젝트와 현재 오브젝트가 같거나 현재 오브젝트의 레이어가 Water면 선택취소하게 만듦
                    if (m_PrevObject == m_SelectObject ||
                        m_SelectObject.layer == LayerMask.NameToLayer("Water"))
                    {
                        // 현재 머터리얼을 Grass로 초기화
                        ChangeMaterialNode(TileX, TileY, "Grass");

                        // 버튼 싹다 비활성화
                        m_Button.BuildOffButton();
                        m_Button.TowerOffBtn();

                        // 이전 오브젝트와 현재 선택된 오브젝트 null로 초기화
                        m_SelectObject = null;
                        m_PrevObject = null;
                    }
                    // 둘다 아니라면
                    else
                    {
                        // 이전 오브젝트가 Node라면 Node에서 좌표 가져오고 타워 설치 버튼 활성화
                        if (m_PrevObject.layer == LayerMask.NameToLayer("Ground"))
                        {
                            TileX = m_PrevObject.GetComponent<Node>().TileX;
                            TileY = m_PrevObject.GetComponent<Node>().TileY;

                            m_Button.TowerOnBtn();
                            m_Button.BuildOffButton();
                        }
                        // 이전 오브젝트가 Tower라면 각 타워에서 좌표 가져오고 타워 삭제 버튼 활성화 (아래 else if문 전부)
                        else if (m_PrevObject.layer == LayerMask.NameToLayer("BasicTower"))
                        {
                            TileX = m_PrevObject.GetComponent<BasicTower>().TileX;
                            TileY = m_PrevObject.GetComponent<BasicTower>().TileY;

                            m_Button.TowerOnBtn();
                            m_Button.BuildOffButton();
                        }
                        else if (m_PrevObject.layer == LayerMask.NameToLayer("HealTower"))
                        {
                            TileX = m_PrevObject.GetComponent<HealTower>().TileX;
                            TileY = m_PrevObject.GetComponent<HealTower>().TileY;

                            m_Button.TowerOnBtn();
                            m_Button.BuildOffButton();
                        }
                        else if (m_PrevObject.layer == LayerMask.NameToLayer("Obstacle"))
                        {
                            TileX = m_PrevObject.GetComponent<Obstacle>().TileX;
                            TileY = m_PrevObject.GetComponent<Obstacle>().TileY;

                            m_Button.TowerOnBtn();
                            m_Button.BuildOffButton();
                        }

                        ChangeMaterialNode(TileX, TileY, "Grass");
                        m_PrevObject = m_SelectObject;
                    }
                }
            }
        }
    }

    // 노드 가져오기 (삼항연산자)
    public Node GetNode(int x, int y)
    {
        return m_NodeArr[(y * 10) + x] == null ? null : m_NodeArr[(y * 10) + x];
    }

    // 노드 메터리얼 변경
    public void ChangeMaterialNode(int x, int y, string material)
    {
        GetNode(x, y).GetComponent<MeshRenderer>().material = Resources.Load("Tower/Material/" + material) as Material;
    }
}