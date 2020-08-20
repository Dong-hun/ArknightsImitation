using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Tower : TowerManager
{
    /* 타워매니저를 상속받아서 만들어지는 각 타워들의 스크립트 */
    // 나중에 스크립트 이름을 타워이름으로 바꿀것 (현재 임시, 바꾸면 이 주석 지울것)
    // 기본 공격 하는 타워

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

    // Start is called before the first frame update
    void Start()
    {
        base.Init(50, 50, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    // 타일 위치 세팅
    public void SetTileNumber(int x, int y)
    {
        m_TileX = x;
        m_TileY = y;
    }

    // 상태 변경시 한번 호출될 함수
    protected override void ChangeState(STATE s)
    {
        if (m_State == s) return;
        m_State = s;

        switch (m_State)
        {
            case STATE.IDLE:
                break;
            case STATE.BATTLE:
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
                break;
            case STATE.DIE:
                break;
        }
    }
    
    // 공격
    protected override void Attack()
    {
        
    }

    // 사망
    protected override void Die()
    {

    }

    // 적 추가
    protected override void AddEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        m_EnemyList.Add(enemy);

    }

    // 가장 가까운 적 받아오기
    protected override Enemy GetNearestEnemy()
    {
        if (m_EnemyList.Count == 0) return null;

        float dist = 999f;
        int sel = -1;

        for(int i = 0; i < m_EnemyList.Count; ++i)
        {
            // 거리 계산
            float temp = Vector3.Distance(this.transform.position, 
                m_EnemyList[i].transform.position);

            if(temp < dist)
            {
                dist = temp;
                sel = i;
            }
        }

        return m_EnemyList[sel];
    }

    // 적 제거
    protected override void RemoveEnemy(Enemy enemy)
    {
        if (enemy == null) return;

        for(int i = 0; i < m_EnemyList.Count; ++i)
        {
            if(m_EnemyList[i].transform == enemy.transform)
            {
                m_EnemyList.Remove(m_EnemyList[i]);
            }
        }
    }
    
    // 적 탐색
    void FindEnemy()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // 사정거리에 들어온 적 추가
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            AddEnemy(other.gameObject.GetComponent<Enemy>());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // 적 방향으로 바라보기
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Vector3 dir = other.transform.position - transform.position;
            dir.Normalize();

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                Quaternion.LookRotation(dir), Time.smoothDeltaTime * 360.0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 사정거리밖으로 나간 적 제거
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            RemoveEnemy(other.gameObject.GetComponent<Enemy>());
        }
    }
}
