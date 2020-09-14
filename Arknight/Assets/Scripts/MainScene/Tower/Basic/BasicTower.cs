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
    [Header("Unity Stuff")]
    public Image HealthBar1;
    public Image MPBar;

    public bool m_SkillActive;          // 스킬이 활성화 되어있는지
    public float m_OriginAttackDelay;    // 스킬로 영향 받는 AttackDelay

    // Start is called before the first frame update
    new void Start()
    {
        // 컴포넌트 추가
        base.Start();

        // 딜리게이트 추가
        this.GetComponentInChildren<TowerAnimationEvent>().m_Attack = new DelAttack(OnDamage);
        this.GetComponentInChildren<TowerAnimationEvent>().m_BasicTowerSkill = new DelCor(ActiveSkill);

        // Animation설정
        m_Anim.SetBool("Dead", false);

        // 리스트 추가
        m_EnemyList = new List<Enemy>();

        // 스텟 추가(적 데미지, MP실험위해 체력, 적데미지 높여놈)
        Init(500, 5, 30, 5.0f , 2.0f);
        //m_MaxMp = 50;

        // 스킬 비활성화, AttackDelay 저장(스킬 사용용)
        m_SkillActive = false;
        m_OriginAttackDelay = m_AttackDelay;
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
        // 타겟이 없으면 리턴
        if (m_Target == null) return;

        if (!m_Anim.GetBool("Dead"))
        //if (dist < m_AttackDelay && !m_Anim.GetBool("Dead"))
        {
            if(m_CurrentMp >= m_MaxMp)
            {
                m_SkillActive = true;
                m_Anim.SetTrigger("Skill");
                m_CurrentMp = 0.0f;
            }

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
                    if (!m_SkillActive)
                    {
                        // 마나 증가
                        m_CurrentMp += 1;
                    }

                    // 다시 딜레이 설정
                    m_AttackDelay = m_OriginAttackDelay;
                }
                // 적이 죽었다면
                else
                {
                    // IDLE상태로 바꿈
                    ChangeState(STATE.IDLE);
                }
            }

            // 딜레이 감소
            m_AttackDelay -= Time.deltaTime;
        }
        else // if (dist > m_AttackDist && !m_Anim.GetBool("Dead"))
        {
            ChangeState(STATE.IDLE);
        }
    }

    // 적에게 데미지 줌
    public void OnDamage(GameObject enemy)
    {
        // PlayAnimationEvent에서 공격 모션일때 이 함수 호출
        if (enemy == null) return;

        enemy.GetComponent<MonsterStat>().UpdateHP(-m_Damage);

        //적 체력바
        enemy.GetComponent<Enemy>().EnemyHealthBar();
        
        //BasicTower Mp바(이때 넣어야 타이밍이 맞음)
        MPBar.fillAmount = m_CurrentMp / m_MaxMp;
        //mpbar 값(1= maxmp)
        Debug.Log(MPBar.fillAmount.ToString());



        


    }

    //체력바
    public void BasicHealth()
    {
        HealthBar1.fillAmount = m_CurrentHp / m_MaxHp;
        //Debug.Log(HealthBar1.fillAmount.ToString());
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

    // 스킬 사용
    public override IEnumerator ActiveSkill(float timer)
    {
        float originAttackDelay = m_OriginAttackDelay;
        while (m_SkillActive)
        {
            // 공격 딜레이 반으로 감소
            m_OriginAttackDelay /= 2.0f;

            // 10초 뒤 반복문 빠져나옴
            yield return new WaitForSeconds(timer);
            m_SkillActive = false;
        }
        m_OriginAttackDelay = originAttackDelay;
    }
}
