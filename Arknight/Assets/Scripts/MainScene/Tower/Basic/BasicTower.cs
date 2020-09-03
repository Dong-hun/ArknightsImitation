using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BasicTower : TowerManager
{
    /* 타워매니저를 상속받아서 만들어지는 각 타워들의 스크립트 */
    // 기본 공격 하는 타워


    // 프로퍼티
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

    public int HP
    {
        set
        {
            m_HP = value;
        }
        get
        {
            return m_HP;
        }
    }

    public int MP
    {
        set
        {
            m_MP = value;
        }
        get
        {
            return m_MP;
        }
    }

    public int MaxHP
    {
        get
        {
            return m_MaxHp;
        }
    }
    public int MaxMP
    {
        get
        {
            return m_MaxMp;
        }
    }

    public int Damage
    {
        set
        {
            m_Damage = value;
        }
        get
        {
            return m_Damage;
        }
    }

    public Enemy m_Target;         // 현재 타겟

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        m_Anim.SetBool("Dead", false);

        m_EnemyList = new List<Enemy>();
        Init(50, 50, 600, 5.0f);

        // 타겟을 null로 초기화
        m_Target = null;

        this.GetComponentInChildren<BasicTowerAnimationEvent>().m_Attack = new DelAttack(OnDamage);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();

        if(HP <= 0.0f)
        {
            ChangeState(STATE.DEATH);
        }
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
            case STATE.DEATH:
                m_Anim.SetBool("Dead", true);
                StartCoroutine(Disappear(3.0f));
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
            case STATE.DEATH:
                break;
        }
    }

    // 회전
    void Rotation(Enemy enemy)
    {
        if (enemy == null) return;
        // 적과의 방향 구함
        Vector3 dir = enemy.transform.position -
            this.transform.position;

        // 해당 방향으로 회전
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
            Quaternion.LookRotation(dir), Time.smoothDeltaTime * 360.0f);
    }

    // 공격
    protected override void Attack()
    {
        // 타겟이 없으면 리턴
        if (m_Target == null) return;

        // 적과의 거리 구함
        float dist = Vector3.Distance(this.transform.position, m_Target.transform.position);

        if(dist < m_AttackDist && !m_Anim.GetBool("Dead"))
        {
            // 적 방향으로 회전
            Rotation(m_Target);

            // 딜레이가 0이하가 된다면
            if (m_AttackDelay <= Mathf.Epsilon)
            {
                // 제일 가까운 적이 살아있다면
                if (m_Target != null)
                {
                    // Attack 트리거 발동
                    m_Anim.SetTrigger("Attack");

                    // 다시 딜레이 설정
                    m_AttackDelay = 1.0f;
                }
                // 적이 죽었다면
                else
                {
                    // 타겟을 null로 초기화
                    m_Target = null;

                    // IDLE상태로 바꿈
                    ChangeState(STATE.IDLE);
                }
            }

            // 딜레이 감소
            m_AttackDelay -= Time.deltaTime;
        }
        else
        {
            ChangeState(STATE.IDLE);
        }    
        
    }

    // 적에게 데미지 줌
    public void OnDamage(Enemy enemy)
    {
        // PlayAnimationEvent에서 공격 모션일때 이 함수 호출
        if (enemy == null) return;

        enemy.GetComponent<MonsterStat>().UpdateHP(-m_Damage);
    }

    // 사망
    protected override void Death()
    {
        Destroy(this.gameObject);
    }

    // 적 추가
    public void AddEnemy(Enemy enemy)
    {
        // 매개변수가 null이면 리턴
        if (enemy == null) return;

        // 리스트에 채워줌
        m_EnemyList.Add(enemy);
    }

    // 적 제거
    //protected override void RemoveEnemy(Enemy enemy)
    //{
    //    if (enemy == null) return;
    //
    //    for (int i = 0; i < m_EnemyList.Count; ++i)
    //    {
    //        if (m_EnemyList[i].transform == enemy.transform)
    //        {
    //            m_EnemyList.Remove(m_EnemyList[i]);
    //        }
    //    }
    //
    //    if (m_EnemyList.Count == 0)
    //        ChangeState(STATE.IDLE);
    //}

    private void OnCollisionEnter(Collision col)
    {
        // 충돌체의 레이어가 Enemy면
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // 타겟(적)이 없으면 
            if(m_Target == null)
            {
                // 타겟에 해당 충돌체를 넣어줌
                m_Target = col.gameObject.GetComponent<Enemy>();

                // State를 Battle로 변경
                ChangeState(STATE.BATTLE);
            }
            // 타겟이 있다면
            else
            {
                // 리턴
                return;
            }
        }
    }

    




    // 가장 가까운 적 받아오기
    //protected override Enemy GetNearestEnemy()
    //{
    //    // 리스트가 비었으면 리턴
    //    if (m_EnemyList.Count == 0) return null;
    //
    //    // 첫 거리는 아주크게 설정
    //    float dist = 999f;
    //
    //    // 가장 가까운 인덱스 저장용
    //    int sel = -1;
    //
    //    // 있는 적 다 검사
    //    for (int i = 0; i < m_EnemyList.Count; ++i)
    //    {
    //        // 거리 계산
    //        float temp = Vector3.Distance(this.transform.position,
    //            m_EnemyList[i].transform.position);
    //
    //        // 계산된 거리가 이전의 가장 가까운거리보다 작다면
    //        if (temp < dist)
    //        {
    //            // 계산된 거리를 가장 가까운 거리로 바꿔주고
    //            dist = temp;
    //
    //            // 해당 인덱스 번호 저장
    //            sel = i;
    //        }
    //    }
    //
    //    // 해당 인덱스 번호의 적 리턴
    //    return m_EnemyList[sel];
    //}
}
