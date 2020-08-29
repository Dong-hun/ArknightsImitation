﻿using System.Collections;
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
    public DelDelete m_DelDeleteTower;

    // Start is called before the first frame update
    void Start()
    {
        // 매니저 가져오기
        m_NodeManager = GameObject.Find("NodeList").GetComponent<NodeManager>();
        m_BuildManager = GameObject.Find("BuildManager").GetComponent<BuildManager>();

        // Add함수 딜리게이트 추가
        m_DelAddTower = new DelAdd(AddTower);
        m_DelAddTower?.Invoke();

        // delete함수 딜리게이트 추가
        m_DelDeleteTower = new DelDelete(RemoveTower);
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
        if (m_AroundTowerList.Count == 0) return;
        
        for(int i = 0; i < m_AroundTowerList.Count; ++i)
        {

        }
    }

    // 주변에 타워가 있으면 리스트에 추가해주는 함수
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

                    // 리스트가 비어있으면
                    if (m_AroundTowerList.Count == 0)
                    {
                        // 바로 담아준다
                        m_AroundTowerList.Add(temp);
                    }
                    // 비어있지 않으면
                    else
                    {
                        // 해당 리스트 조사해서
                        for (int k = 0; k < m_AroundTowerList.Count; ++k)
                        {
                            // 해당 리스트의 원소와 같으면
                            if (m_AroundTowerList[k] == temp)
                            {
                                // 넘어감
                                continue;
                            }
                            // 해당 리스트의 원소와 다르면
                            else
                            {
                                // 리스트에 담아줌
                                m_AroundTowerList.Add(temp);
                                break;
                            }
                        }
                    }   
                }
            }
        }

        // 리스트를 돌아서 중복검사 해줌
        for(int i = 0; i < m_AroundTowerList.Count; ++i)
        {
            for(int j = i + 1; j < m_AroundTowerList.Count; ++j)
            {
                // 검사하려는 타워가 또 있다면
                if(m_AroundTowerList[i] == m_AroundTowerList[j])
                {
                    // 뒤쪽의 타워를 제거해줌
                    m_AroundTowerList.Remove(m_AroundTowerList[j]);

                    // 사이즈가 1줄었으므로 해당 원소를 다시 조사하기 위해 -1해줌
                    --j;
                }
            }
        }
    }

    // 타워 제거 함수
    public void RemoveTower(TowerManager tower)
    {
        // 리스트가 비어있으면 리턴
        if (m_AroundTowerList.Count == 0) return;

        // 리스트 돌아서
        for(int i = 0; i < m_AroundTowerList.Count; ++i)
        {
            // 해당 리스트에 해당타워가 있으면
            if(m_AroundTowerList[i].transform == tower.transform)
            {
                // 리스트에서 제거 후 빠져나옴 (더이상 검사할 필요 x)
                m_AroundTowerList.Remove(m_AroundTowerList[i]);
                break;
            }
        }
    }
}