using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTower : TowerManager
{
    // 주변의 타워 담긴 리스트
    public List<GameObject> m_AroundTowerList;

    // 주변에 타워 설치되면 주변 타워 리스트에 담아주는 함수 델리게이트
    public DelVoid m_DelAddTower;

    // 주변의 타워가 제거되면 주변의 타워가 담긴 리스트에서도 제거해주는 함수 델리게이트
    public DelDelete m_DelDeleteTower;

    // Start is called before the first frame update
    new void Start()
    {
        // 컴포넌트 추가
        base.Start();

        this.GetComponentInChildren<TowerAnimationEvent>().m_Recovery = new DelRecovery(RecoveryTower);
        this.GetComponentInChildren<TowerAnimationEvent>().m_Death = new DelDeath(Disappear);

        // 초기화
        Init(50, 10, 2, 5.0f, 3.0f);

        // Add함수 델리게이트 추가
        m_DelAddTower = new DelVoid(AddTower);
        m_DelAddTower?.Invoke();

        // delete함수 델리게이트 추가
        m_DelDeleteTower = new DelDelete(RemoveTower);

        // Animation의 Dead값 false로 설정
        m_Anim.SetBool("Dead", false);
        m_Anim.SetBool("Attack", false);
    }
    // Update is called once per frame
    void Update()
    {
        StateProcess();

        if (HP <= 0.0f)
        {
            ChangeState(STATE.DEATH);
        }
    }
    protected override void ChangeState(STATE s)
    {
        if (m_State == s) return;
        m_State = s;

        switch (m_State)
        {
            case STATE.IDLE:
                StopCoroutine(this.GetComponentInChildren<TowerAnimationEvent>().m_Recovery());
                m_Anim.SetBool("Attack", false);
                break;
            case STATE.BATTLE:
                m_Anim.SetBool("Attack", true);
                break;
            case STATE.DEATH:
                m_Anim.SetBool("Dead", true);
                break;
        }
    }

    // 프레임 마다 업데이트 할 함수
    protected override void StateProcess()
    {
        switch (m_State)
        {
            case STATE.IDLE:
                Idle();
                break;
            case STATE.BATTLE:
                //Attack();
                break;
            case STATE.DEATH:
                break;
        }
    }

    //대기 상태일때 돌릴 업데이트
    void Idle()
    {
        // 주변에 타워가 없으면 리턴
        if (m_AroundTowerList.Count == 0) return;

        // 주변의 타워를 전부 조사함
        for(int i = 0; i < m_AroundTowerList.Count; ++i)
        {
            // 주변 타워의 체력 담을 변수
            int maxHp = 0;
            int currentHp = 0;

            // 주변 타워의 레이어 별로 스크립트에서 체력 가져옴
            if(m_AroundTowerList[i].layer == LayerMask.NameToLayer("BasicTower"))
            {
                maxHp = m_AroundTowerList[i].GetComponent<BasicTower>().MaxHP;
                currentHp = m_AroundTowerList[i].GetComponent<BasicTower>().HP;
            }
            else if (m_AroundTowerList[i].layer == LayerMask.NameToLayer("HealTower"))
            {
                maxHp = m_AroundTowerList[i].GetComponent<HealTower>().MaxHP;
                currentHp = m_AroundTowerList[i].GetComponent<HealTower>().HP;
            }

            // 현재 HP가 최대 HP보다 작다면
            if(currentHp < maxHp)
            {
                // State를 Battle로 변경 후 함수 빠져나옴
                m_Target = m_AroundTowerList[i];
                ChangeState(STATE.BATTLE);
                break;
            }
            // 아니라면
            else
            {
                // 다음거 검사
                continue;
            }
        }
    }
    
    // 공격하는 함수 (HealTower는 힐을 해줌)
    IEnumerator RecoveryTower()
    {
        while (m_Target.GetComponent<BasicTower>().HP >= m_Target.GetComponent<BasicTower>().MaxHP)
        {
            m_Target.GetComponent<BasicTower>().HP += m_Damage;

            yield return null;
        }
        ChangeState(STATE.IDLE);
        yield return null;
    }

    // 주변에 타워가 있으면 리스트에 추가해주는 함수
    void AddTower()
    {
        // Y축 좌표 검사
        for (int i = TileY - 1; i < TileY + 2; ++i)
        {
            // 타일 범위 벗어나면 넘어감 (세로)
            if (i < 0 || i > 8)
                continue;

            // X축 좌표 검사
            for (int j = TileX - 1; j < TileX + 2; ++j)
            {
                // 타일 범위 벗어나면 넘어감 (가로)
                if (j < 0 || j > 9)
                    continue;

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

    // 주변에 있는 타워 제거되면 리스트에서도 지워주는 함수
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