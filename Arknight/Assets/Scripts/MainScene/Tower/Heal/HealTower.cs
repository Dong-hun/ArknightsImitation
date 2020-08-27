using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTower : TowerManager
{
    public int TileX
    {
        set
        {
            m_TileX = value;
        }
        get
        {
            return m_TileX;
        }
    }
    public int TileY
    {
        set
        {
            m_TileY = value;
        }
        get
        {
            return m_TileY;
        }
    }
    public List<GameObject> m_AroundTowerList;

    public DelAdd m_DelAddTower;

    // Start is called before the first frame update
    void Start()
    {
        // 매니저 가져오기
        m_NodeManager = GameObject.Find("NodeList").GetComponent<NodeManager>();
        m_BuildManager = GameObject.Find("BuildManager").GetComponent<BuildManager>();


        m_DelAddTower = new DelAdd(AddTower);
        m_DelAddTower?.Invoke();
    }
    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }
    protected override void ChangeState(STATE s)
    {
        if (m_State == s) return;
        m_State = s;

        switch (m_State)
        {
            case STATE.IDLE:
                break;
            case STATE.BATTLE:
                m_Anim.SetTrigger("Attack");
                break;
            case STATE.DIE:
                break;
        }
    }

    // 프레임 마다 업데이트 할 함수
    protected override void StateProcess()
    {
        switch (m_State)
        {
            case STATE.IDLE:
                break;
            case STATE.BATTLE:
                Attack();
                break;
            case STATE.DIE:
                break;
        }
    }

    protected override void Idle()
    {

    }

    void AddTower()
    {
        // Y축 좌표 검사
        for (int i = TileY - 1; i < TileY + 2; ++i)
        {
            // X축 좌표 검사
            for (int j = TileX - 1; j < TileX + 2; ++j)
            {
                // 해당 좌표의 노드가 없다면 다음 노드 검사
                if (m_NodeManager.GetNode(j, i) == null) continue;

                // 해당 좌표의 노드상태가 TOWER라면 (타워가 설치됬다면)
                if (m_NodeManager.m_TileState[i, j] == NodeManager.TILEINFO.TOWER)
                {
                    // 해당 좌표의 타워를 저장
                    GameObject temp = m_BuildManager.GetTowerCoordinates(j, i);
                    if (m_AroundTowerList.Count == 0)
                    {
                        m_AroundTowerList.Add(temp);
                    }
                    else
                    {
                        for (int k = 0; k < m_AroundTowerList.Count; ++k)
                        {
                            if (m_AroundTowerList[k] == temp)
                            {
                                continue;
                            }
                            else
                            {
                                m_AroundTowerList.Add(temp);
                                break;
                            }
                        }
                    }   
                }
                else
                {
                    continue;
                }
            }
        }

        for(int i = 0; i < m_AroundTowerList.Count; ++i)
        {
            for(int j = i + 1; j < m_AroundTowerList.Count; ++j)
            {
                if(m_AroundTowerList[i] == m_AroundTowerList[j])
                {
                    m_AroundTowerList.Remove(m_AroundTowerList[j]);
                    --j;
                }

            }
        }




    }

    void RemoveListTower(GameObject tower)
    {
        if (m_AroundTowerList.Count == 0) return;

        for(int i = 0; i < m_AroundTowerList.Count; ++i)
        {
            if(m_AroundTowerList[i].transform == tower.transform)
            {
                m_AroundTowerList.Remove(m_AroundTowerList[i]);
                break;
            }
        }
    }

}
