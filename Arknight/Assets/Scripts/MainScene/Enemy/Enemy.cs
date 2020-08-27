using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //만들어야 하는것
    //1. 몬스터가 자동으로 목적지로 이동한다.
    //2. 몬스터가 이동하다가 장애물을 만나면 어떠한 이동 패턴을 보인다.
    //3.  haspath,pathPending 이용하고
    //4. 상태기계 상태로 전환해
    // 5.적 생성... (코루틴으로 생성..시간 몇초마다..) 몇 마리가 죽었으면 나온다. ㅇㅋ.

    //6. 보스 패턴 (1. 5초동안 기모아서 한방(무조건한방에 다죽임),2.체력이 낮고 빠른 보스3.체력이 5배이고 3바퀴 더돌고 들어가는 보스4.광역공격)
    //7. 장애물 부시기  경로..재계산 여까지 대충
    //8.레인지 시스템
    public enum STATE
    {
        CREATE, TARGET1, TAGET2, ATTACK,BATTLE ,DEAD, GOAL
    }
    public NavMeshAgent m_Navi;
    public STATE m_STATE;
    public NavMeshPath m_Path;
    public MonsterStat m_Monsterinfo;
    public Obstacles m_Enemy;
    float attackdelay = 3.0f;

    Vector3 objpos;
    Vector3 objpos2;
    Vector3 Goalpos;
    //public Cube m_Enemy;
    // Start is called before the first frame update
    //애니메이션 이벤트 추가할것
    void Start()
    {
        this.transform.position = GameObject.Find("Start").GetComponent<Transform>().position;
        objpos = GameObject.Find("End").GetComponent<Transform>().position;
        objpos2 = GameObject.Find("Plane (80)").GetComponent<Transform>().position;

        //Vector3 DESTPOS = m_Navi.destination;
        m_Navi = GetComponent<NavMeshAgent>();
        m_STATE = STATE.CREATE;
        ChangeSTATE(STATE.TARGET1);
        m_Path = new NavMeshPath();

        //   m_Monsterinfo = GetComponent<MonsterStat>();
    }

    // Update is called once per frame
    void Update()
    {
        //   RaycastHit hit;
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    if (Physics.Raycast(ray, out hit))
        //        m_Navi.SetDestination(hit.point);
        //}
        StateProcess();

    }




    void ChangeSTATE(STATE s)
    {
        if (m_STATE == s) return;
        m_STATE = s;
        switch (m_STATE)
        {
            case STATE.TARGET1:

                m_Navi.SetDestination(objpos);



                break;
            case STATE.TAGET2:
                //m_Navi.SetDestination(obj2.transform.position);
                m_Navi.SetDestination(objpos2);

                break;

            case STATE.ATTACK:
                m_Enemy = GameObject.FindObjectOfType<Obstacles>();

                break;

        }
    }
    void StateProcess()
    {

        switch (m_STATE)
        {
            case STATE.TARGET1:
                //   NavMesh.CalculatePath(transform.position, obj.transform.position,NavMesh.AllAreas ,m_Path);
                //  if (path == true)
                float T = Vector3.Distance(this.transform.position, objpos);
                float s = Vector3.Distance(this.transform.position, m_Navi.destination);
                if (!m_Navi.pathPending) //계산완료 후 이동
                {
                    if (m_Navi.remainingDistance <= m_Navi.stoppingDistance)
                    {
                        if (!m_Navi.hasPath || m_Navi.velocity.sqrMagnitude == 0.0f)
                        {
                            //상태를 멈춤상태로 만들거나 2번째 상태로 만드는게 나을듯. ㅇㅇ 맞어
                            ChangeSTATE(STATE.TAGET2);
                        }
                    }
                    else if (T - s > 1.5f)
                    {
                        ChangeSTATE(STATE.ATTACK);
                    }

                }





                break;


            case STATE.TAGET2:
                float T2 = Vector3.Distance(this.transform.position, objpos);
                float s2 = Vector3.Distance(this.transform.position, m_Navi.destination);
                if (!m_Navi.pathPending) //계산완료 후 이동
                {
                    if (m_Navi.remainingDistance <= m_Navi.stoppingDistance)
                    {
                        if (!m_Navi.hasPath || m_Navi.velocity.sqrMagnitude == 0.0f)
                        {
                            //상태를 멈춤상태로 만들거나 2번째 상태로 만드는게 나을듯. ㅇㅇ 맞어
                            ChangeSTATE(STATE.TAGET2);
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

                    if (dist > 3.5f)
                    {
                        m_Navi.SetDestination(m_Enemy.transform.position);
                    }
                    else if (dist < 3.5f)
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
                        attackdelay = 2.0f;
                        Onattack();
                        if (m_Enemy == null)
                        {
                            ChangeSTATE(STATE.TAGET2);
                        }
                    }
                    attackdelay -= Time.smoothDeltaTime;
                
               
                    break;

        }

    }
    //충돌하면 경로 재계산하고 ATTACK로 바뀌고 적군 제거하고 다시 이동.


    private void OnCollisionStay(Collision collision)


    {


        //   Debug.Log("충돌 중!");

    }


    void Onattack(Obstacles enemy)
    {
        //아 이런. 이걸 
        m_Enemy.OnDamage(m_Monsterinfo.MonsterAttack);
        //    //큐프
        Debug.Log("공격3");
        // //   Debug.Log(m_Monsterinfo.MonsterAttack);
    }



    protected void OnBattle(Obstacles enemy)
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
        m_Enemy = enemy.gameObject.GetComponentInChildren<Obstacles>();
        if (m_Enemy == null) return;
        ChangeSTATE(STATE.ATTACK);
        Debug.Log("공격2");
        //
    }

    void Onattack()
    {
        //아 이런. 이걸 
        if (m_Enemy != null)
        {
            m_Enemy.OnDamage(m_Monsterinfo.MonsterAttack);

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
}
