using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public float CurrentHp
    {
        set
        {
            m_CurrentHp = value;
        }
        get
        {
            return m_CurrentHp;
        }
    }

    public float CurrentMp
    {
        set
        {
            m_CurrentMp = value;
        }
        get
        {
            return m_CurrentMp;
        }
    }

    public float MaxHp
    {
        get
        {
            return m_MaxHp;
        }
    }
    public float MaxMp
    {
        get
        {
            return m_MaxMp;
        }
    }

    public float Damage
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
    public Image HpBar
    {
        set
        {
            m_HpBar = value;
        }
        get
        {
            return m_HpBar;
        }
    }
    public Image MpBar
    {
        set
        {
            m_MpBar = value;
        }
        get
        {
            return m_MpBar;
        }
    }




    public BoxCollider m_BoxCollider;

    // Start is called before the first frame update
    new void Start()
    {
        // 컴포넌트 추가
        base.Start();

        // 딜리게이트 추가
        this.GetComponentInChildren<TowerAnimationEvent>().m_Attack = new DelAttack(OnDamage);
        this.GetComponentInChildren<TowerAnimationEvent>().m_BasicTowerSkill = new DelCor(ActiveSkill);

        // 콜라이더 추가
        m_BoxCollider = GetComponent<BoxCollider>();

        // Animation설정
        m_Anim.SetBool("Dead", false);

        // 리스트 추가
        m_EnemyList = new List<Enemy>();
        
        // 스텟 추가
        Init(50, 5, 2, 2.0f);

        // AttackDelay 저장(스킬 사용용)
        m_OriginAttackDelay = m_AttackDelay;

        // 처음 딜레이를 0으로 설정해서 바로 공격하게 설정
        m_AttackDelay = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();

        if (CurrentHp <= 0.0f)
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
                m_Target = null;
                break;
            case STATE.BATTLE:
                break;
            case STATE.SKILL:
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
                break;
            case STATE.BATTLE:
                Attack();
                break;
            case STATE.SKILL:
                break;
            case STATE.DEATH:
                break;
        }
    }

    // 회전
    void Rotation(GameObject enemy)
    {
        // 적이 없다면 함수 빠져나옴
        if (enemy == null) return;

        // 적과의 방향 구함
        Vector3 dir = enemy.transform.position -
            this.transform.position;

        // 해당 방향으로 회전
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
            Quaternion.LookRotation(dir), Time.smoothDeltaTime * 360.0f);

        // 업벡터 고정 (캐릭터가 눕는거 방지)
        this.transform.up = new Vector3(0, 1, 0);
    }

    // 공격
    protected override void Attack()
    {
        // 타겟이 없으면 IDLE로 변경
        if (m_Target == null) ChangeState(STATE.IDLE);

        // 죽지 않았다면
        if (!m_Anim.GetBool("Dead"))
        {
            // 마나가 찼다면 스킬사용
            if(m_CurrentMp >= m_MaxMp)
            {
                m_ActiveSkill = true;
                m_Anim.SetTrigger("Skill");
                m_CurrentMp = 0.0f;
            }
            // 안찼다면 일반공격
            else
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

                        // 스킬이 활성화가 되지 않았을 때
                        if (!m_ActiveSkill)
                        {
                            // 마나 증가
                            m_CurrentMp++;
                        }

                        // 다시 딜레이 설정
                        m_AttackDelay = m_OriginAttackDelay;
                    }
                    // 적이 죽었다면
                    else
                    {
                        // IDLE상태로 바꿈
                        ChangeState(STATE.IDLE);

                        // 콜라이더를 껐다가 켜서 적과의 충돌 재검사
                        m_BoxCollider.enabled = false;
                        m_BoxCollider.enabled = true;
                    }
                }
            }

            // 딜레이 감소
            m_AttackDelay -= Time.deltaTime;
        }
        // 죽었다면
        else
        {
            // 죽음상태로 변경
            ChangeState(STATE.DEATH);
        }
    }

    // 적에게 데미지 줌
    public void OnDamage(GameObject enemy)
    {
        // PlayAnimationEvent에서 공격 모션일때 이 함수 호출
        if (enemy == null) return;

        // 적의 현재HP를 데미지만큼 감소시킴
        enemy.GetComponent<MonsterStat>().UpdateHP(-m_Damage);

        //적 체력바
        enemy.GetComponent<Enemy>().EnemyHealthBar();

        //BasicTower Mp바(이때 넣어야 타이밍이 맞음)
        m_MpBar.fillAmount = m_CurrentMp / m_MaxMp;
    }

    // 적 추가
    public void AddEnemy(Enemy enemy)
    {
        // 매개변수가 null이면 리턴
        if (enemy == null) return;

        // 리스트에 채워줌
        m_EnemyList.Add(enemy);
    }

    private void OnCollisionEnter(Collision col)
    {
        // 충돌체의 레이어가 Enemy면
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // 타겟(적)이 없으면 
            if (m_Target == null)
            {
                // 타겟에 해당 충돌체를 넣어줌
                m_Target = col.transform.gameObject;

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

    private void OnCollisionStay(Collision col)
    {
        // 충돌체의 레이어가 Enemy면
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // 타겟(적)이 없으면 
            if (m_Target == null)
            {
                // 타겟에 해당 충돌체를 넣어줌
                m_Target = col.transform.gameObject;

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

    // 스킬 사용
    public override IEnumerator ActiveSkill(float timer)
    {
        // 원래 공격 딜레이 저장
        float originAttackDelay = m_OriginAttackDelay;

        // 스킬이 활성화 중일때
        while (m_ActiveSkill)
        {
            // 공격 딜레이 반으로 감소
            m_OriginAttackDelay /= 2.0f;

            // timer만큼 시간이 지나면
            yield return new WaitForSeconds(timer);

            // 저장해둔 원래 공격 딜레이로 변경
            m_OriginAttackDelay = originAttackDelay;

            // 마나 0으로 변경
            m_CurrentMp = 0;

            // 스킬 비활성화
            m_ActiveSkill = false;
        }
    }
}
