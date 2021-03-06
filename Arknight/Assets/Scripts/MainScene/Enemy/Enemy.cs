﻿using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    //6. 보스 패턴 (1. 5초동안 기모아서 한방(무조건한방에 다죽임),2.체력이 낮고 빠른 보스3.체력이 5배이고 3바퀴 더돌고 들어가는 보스4.광역공격)
    //7. 장애물 부시기  경로..재계산 여까지 대충
    //8.레인지 시스템
    public enum STATE
    {
         CREATE, TARGET1, TAGET2, ATTACK, BATTLE, DEAD, TOWERATTACK
    }
    public NavMeshAgent m_Navi;
    public STATE m_STATE;
    public GameObject m_Target;
    public NavMeshPath m_Path;
    public MonsterStat m_Monsterinfo;
    public Obstacle m_Enemy;
    float attackdelay = 3.0f;
    public Animator m_Anim;

    public BuildManager m_Buildmanager;
    [Header("Unity Stuff")]
    public Image HealthBar3;

    Vector3 objpos;
    Vector3 objpos2;
    Vector3 Goalpos;

    /* 생성할 적 클래스 */
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = GameObject.Find("Start").GetComponent<Transform>().position;
        objpos = GameObject.Find("Plane (80)").GetComponent<Transform>().position;
        objpos2 = GameObject.Find("End").GetComponent<Transform>().position;
       // Goalpos = GameObject.Find("End").GetComponent<Transform>().position;
        m_STATE = STATE.CREATE;
        ChangeSTATE(STATE.TARGET1);
        m_Buildmanager = GameObject.Find("BuildManager").GetComponent<BuildManager>();
        
        //Vector3 DESTPOS = m_Navi.destination;
        m_Navi = GetComponent<NavMeshAgent>();
        m_Path = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
        Death();
    }

    void ChangeSTATE(STATE s)
    {
        if (m_STATE == s) return;
        m_STATE = s;
        switch (m_STATE)
        {
            case STATE.CREATE:

                
                break;
            case STATE.TARGET1:

                m_Navi.SetDestination(objpos);


                break;
            case STATE.TAGET2:
                //m_Navi.SetDestination(obj2.transform.position);
                m_Navi.SetDestination(objpos2);

                break;

            case STATE.ATTACK:
                Obstacle[] enemyList = GameObject.FindObjectsOfType<Obstacle>();

                float tempDist = 999f;
                int sel = -1;

                for (int i = 0; i < enemyList.Length; ++i)
                {
                    float dist = Vector3.Distance(enemyList[i].transform.position, this.transform.position);

                    if (dist < tempDist)
                    {
                        tempDist = dist;
                        sel = i;
                    }
                }
                m_Enemy = enemyList[sel];

                break;
            //    m_Navi.SetDestination(Goalpos);
            case STATE.BATTLE:
                break;
            case STATE.TOWERATTACK:
                m_Navi.SetDestination(m_Target.transform.position);
                break;
            case STATE.DEAD:
                Destroy(this.gameObject);
                EnemyUI obj = GameObject.Find("Enemy").GetComponent<EnemyUI>();
                obj.m_KillCount++;

                break;

        }
    }
    void StateProcess()
    {

        switch (m_STATE)
        {
            case STATE.TARGET1:
                // 현재 위치에서 목적지까지의 거리 (변하지 않음)
                // 현재 위치에서 SetNavigation의 destination까지의 거리(길이 막히면 변함)를 저장함.
                float T = Vector3.Distance(this.transform.position, objpos);
                float s = Vector3.Distance(this.transform.position, m_Navi.destination);

                if (!m_Navi.pathPending) //계산완료 후 이동
                {
                    // 남은 거리가 멈추는 거리보다 작거나 같으면
                    if (m_Navi.remainingDistance <= m_Navi.stoppingDistance)
                    {
                        if (!m_Navi.hasPath || m_Navi.velocity.sqrMagnitude == 0.0f)
                        {
                            //상태를 멈춤상태로 만들거나 2번째 상태로 만드는게 나을듯. ㅇㅇ 맞어
                            ChangeSTATE(STATE.TAGET2);
                        }
                    }
                    // 위에서 구한 두 길이의 차가 설정치 이상 난다면 (길이 막히면 Navi의 destination이 막힌지점으로 변경됨)
                    else if (T - s > 1.5f)
                    {
                        ChangeSTATE(STATE.ATTACK);
                    }
                }
                break;
            case STATE.TAGET2:
                float T2 = Vector3.Distance(this.transform.position, objpos2);
                float s2 = Vector3.Distance(this.transform.position, m_Navi.destination);
                if (!m_Navi.pathPending) //계산완료 후 이동
                {
                    if (m_Navi.remainingDistance <= m_Navi.stoppingDistance)
                    {
                        if (!m_Navi.hasPath || m_Navi.velocity.sqrMagnitude == 0.0f)
                        {
                            //상태를 멈춤상태로 만들거나 2번째 상태로 만드는게 나을듯. ㅇㅇ 맞어
                           // ChangeSTATE(STATE.TAGET2);
                        }
                    }
                    else if (T2 - s2 > 1.5f)
                    {
                        ChangeSTATE(STATE.ATTACK);
                    }

                }
                break;
            case STATE.ATTACK:
                if (m_Enemy != null)
                {
                    float dist = Vector3.Distance(this.transform.position, m_Enemy.transform.position);

                    if (dist > 5.5f)
                    {
                        m_Navi.SetDestination(m_Enemy.transform.position);
                    }
                    else if (dist < 5.5f)
                    {
                        ChangeSTATE(STATE.BATTLE);
                    }
                    // Debug.Log(dist);
                }
                else if (m_Enemy == null)
                {
                    ChangeSTATE(STATE.TAGET2);
                }
                break;
            case STATE.BATTLE:

                if (attackdelay <= Mathf.Epsilon)
                {
                    if(m_Enemy != null)
                    {
                        m_Anim.SetTrigger("Attack");
                        m_Enemy.GetComponent<Obstacle>().UpdateHpBar();
                        attackdelay = 2.0f;
                        Onattack();
                    }
                    else
                    {
                        ChangeSTATE(STATE.TAGET2);
                    }
                }
                attackdelay -= Time.smoothDeltaTime;


                break;
            case STATE.TOWERATTACK:
                if (m_Target != null) //출돌한 물체 가있다면
                { 
                    if (attackdelay <= Mathf.Epsilon) //일정 공속에 따라
                    {
                        m_Anim.SetTrigger("Attack");

                        if (m_Target.layer == LayerMask.NameToLayer("BasicTower")) //기본타워면
                        {
                            m_Target.GetComponent<BasicTower>().UpdateHp(-m_Monsterinfo.MonsterAttack);
                            m_Target.GetComponent<BasicTower>().UpdateHpBar();
                        }                            //BasicTower 체력바

                        else if (m_Target.layer == LayerMask.NameToLayer("HealTower")) //힐타워면
                        {
                            m_Target.GetComponent<HealTower>().UpdateHp(-m_Monsterinfo.MonsterAttack);
                            //HealTower 체력바
                            m_Target.GetComponent<HealTower>().UpdateHpBar();
                        }

                        attackdelay = 2.0f;
                    }

                }
                else //충돌한 물체가 없으면 타겟 2로 이동
                {
                    ChangeSTATE(STATE.TAGET2);
                }
                attackdelay -= Time.smoothDeltaTime;

                break;
        }

    }
    //충돌하면 경로 재계산하고 ATTACK로 바뀌고 적군 제거하고 다시 이동.


    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("BasicTower") || collision.gameObject.layer == LayerMask.NameToLayer("HealTower")) //공격대상일때만 변화
        {
            m_Target = collision.gameObject; //충돌한 물체가 타겟이다.
            ChangeSTATE(STATE.TOWERATTACK);
        }
        else
            return;
    }


    void Onattack(Obstacle enemy)
    {
        m_Enemy.GetComponent<Obstacle>().UpdateHp(-m_Monsterinfo.MonsterAttack);
        // //   Debug.Log(m_Monsterinfo.MonsterAttack);
    }



    protected void OnBattle(Obstacle enemy)
    {

        if (enemy == null) return;
        m_Enemy = enemy;
        ChangeSTATE(STATE.ATTACK);
        //    Debug.Log("공격1");
        //
        //
    }
     

    //

    protected void OnBattle(Transform enemy)
    {
        m_Enemy = enemy.gameObject.GetComponentInChildren<Obstacle>();
        if (m_Enemy == null) return;
        ChangeSTATE(STATE.ATTACK);
        Debug.Log("공격2");
        //
        //
    }

    void Onattack()
    {
        //아 이런. 이걸 
        if (m_Enemy != null)
        {
            m_Enemy.GetComponent<Obstacle>().UpdateHp(-m_Monsterinfo.MonsterAttack);

            //큐프
            Debug.Log("공격3");
        }//   Debug.Log(m_Monsterinfo.MonsterAttack);
    }
    public void OnDamage(int dmg)
    {

        if (!m_Monsterinfo.UpdateHP(-dmg))
        {

            Debug.Log("사망");
        }
    }

    void Death()
    {
        
        if (this.m_Monsterinfo.CurrentHP <= 0)
        {
            m_Anim.SetTrigger("Dead");

            StartCoroutine( MonDead());
        }
    
    }

    //적체력바
    public void EnemyHealthBar()
    {
        HealthBar3.fillAmount = m_Monsterinfo.CurrentHP / m_Monsterinfo.MaxHp;
        //Debug.Log(HealthBar3.fillAmount.ToString());
    }

    IEnumerator MonDead()
    {
        yield return new WaitForSeconds(2.0f);
        {
            ChangeSTATE(STATE.DEAD);
        }
    }

}
