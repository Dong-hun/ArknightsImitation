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

    DelVoid m_DelAddTower;

    // Start is called before the first frame update
    void Start()
    {
        m_NodeManager = GameObject.Find("NodeList").GetComponent<NodeManager>();
        m_BuildManager = GameObject.Find("BuildManager").GetComponent<BuildManager>();
        m_DelAddTower += AddTower;
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
        for(int i = TileY - 1; i < TileY + 2; ++i)
        {
            for(int j = TileX - 1; j < TileX + 2; ++j)
            {
                if (m_NodeManager.GetNode(j, i) == null) continue;
                if (m_NodeManager.GetNode(j, i) == m_NodeManager.GetNode(TileX, TileY)) continue;

                if(m_NodeManager.m_TileState[i, j] == NodeManager.TILEINFO.TOWER)
                {
                    
                }

            }
        }

        // 내 주변 8칸 노드를 조사해서
        // 장애물이 아닌 타워가 있으면
        // 리스트에 추가
    }
}
