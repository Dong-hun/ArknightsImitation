using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Obstacle : TowerManager
{
    [Header("Unity Stuff")]
    public Image HealthBar;
    
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

    new void Start()
    {
        // 컴포넌트 추가
        m_NodeManager = GameObject.Find("NodeList").GetComponent<NodeManager>();
        m_BuildManager = GameObject.Find("BuildManager").GetComponent<BuildManager>();
        
        // 스텟 세팅
        base.Init();
    }

    void Update()
    {
        StateProcess();

        if(m_BuildManager.m_ObstacleList.Count > 1)
        {
            for (int i = 0; i < m_BuildManager.m_ObstacleList.Count; ++i)
            {
                if (m_BuildManager.m_ObstacleList[i] == null || m_BuildManager.m_ObstacleList[i] == false)
                {
                    m_BuildManager.m_ObstacleList.Remove(m_BuildManager.m_ObstacleList[i]);

                    if (m_BuildManager.m_ObstacleList.Count <= 0)
                        break;
                    else
                        --i;
                }
            }
        }

        if (m_CurrentHp <= 0.0f)
            ChangeState(STATE.DEATH);
    }

    protected override void ChangeState(STATE s)
    {
        if (m_State == s) return;
        m_State = s;

        switch(m_State)
        {
            case STATE.IDLE:
                break;
            case STATE.DEATH:
                Death();
                break;
        }
    }


    protected override void Death()
    {
        for (int i = 0; i < m_BuildManager.m_ObstacleList.Count; ++i)
        {
            if(this.gameObject == m_BuildManager.m_ObstacleList[i])
            {
                m_BuildManager.m_ObstacleList.Remove(m_BuildManager.m_ObstacleList[i]);
            }
        }

        Destroy(this.gameObject);
    }
}
