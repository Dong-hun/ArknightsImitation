using System.Collections;
using System.Collections.Generic;
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
         CREATE, TARGET1, TAGET2, ATTACK, BATTLE, DEAD, GOAL
    }

    
    


    public NavMeshAgent m_Navi;
    public STATE m_STATE;
    public NavMeshPath m_Path;
    public MonsterStat m_Monsterinfo;
    public Obstacle m_Enemy;
    float attackdelay = 3.0f;
<<<<<<< HEAD
    MonsterCreater Monc; //몬스터 리스트 받아오기용 변수

=======
  // public MonsterCreater MonsterCreater; //몬스터 리스트 받아오기용 변수
>>>>>>> 68a1f7ca25405b833c3c8f54ffe943407112cad0
    public BuildManager m_Buildmanager;

    Vector3 objpos; //Plane80
    Vector3 objpos2; //End노드
    Vector3 Goalpos; 

    /* 생성할 적 클래스 */
    // Start is called before the first frame update
    void Start()
    {

        this.transform.position = GameObject.Find("Start").GetComponent<Transform>().position;

        objpos = GameObject.Find("Plane (80)").GetComponent<Transform>().position;
       //첫째 목표지
        objpos2 = GameObject.Find("End").GetComponent<Transform>().position;
       //둘째 목표지
        // Goalpos = GameObject.Find("End").GetComponent<Transform>().position;
        m_STATE = STATE.CREATE;
        //시작하자마자 상태 Target1으로 변경
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
        if (this.m_Monsterinfo.CurrentHP == 0)
        {
            ChangeSTATE(STATE.DEAD);
        }
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
                
                //Plane80을 도착지로
                m_Navi.SetDestination(objpos);


                break;

            case STATE.TAGET2:
                //m_Navi.SetDestination(obj2.transform.position);
                m_Navi.SetDestination(objpos2);

                break;
                
                
            case STATE.ATTACK:

                //하이에라키 에서 Obstacle장애물들을 enemyList에 넣는다.

                //가장 가까운 장애물 인식후 일정 가장 가까운 적을 공격 대상으로 인식

                Obstacle[] enemyList = GameObject.FindObjectsOfType<Obstacle>();

                //해당 장애물과 자기의 가장 짧은 거리
                float tempDist = 999f; //999f는 뭘의미????????????????
                int sel = -1; //[-1]은 없으니까 stateprocess에서null로 만들기 위해서 sel=-1로 해준것?????????????

                //enemyLisht갯수만큼 반복문(0개면 실행X)
                for (int i = 0; i < enemyList.Length; ++i)
                {
                    //dist =장애물위치-적위치
                    float dist = Vector3.Distance(enemyList[i].transform.position, this.transform.position);
                    
                    //tempDist보다 더 짧은거리(dist)가 있다
                    if (dist < tempDist)
                    {
                        
                        tempDist = dist;
                        sel = i;
                    }
                }

                m_Enemy = enemyList[sel]; //[-1]은 없으니까 stateprocess에서null로 만들기 위해서 sel=-1로 해준것?


                break;
            case STATE.GOAL:
            //    m_Navi.SetDestination(Goalpos);

                break;
            case STATE.DEAD:
                death();

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
                
                //T= 적위치-Plane80사이의 거리
                float T = Vector3.Distance(this.transform.position, objpos);
                //s= 적위치-장애물사이의 거리
                float s = Vector3.Distance(this.transform.position, m_Navi.destination);


                if (!m_Navi.pathPending) //계산완료 후 이동
                {
                    //Target1거리 <= 적이 멈춘거리 (장애물이 없다) 
                    if (m_Navi.remainingDistance <= m_Navi.stoppingDistance)
                    {
                        if (!m_Navi.hasPath || m_Navi.velocity.sqrMagnitude == 0.0f)
                        {
                            //상태를 멈춤상태로 만들거나 2번째 상태로 만드는게 나을듯. ㅇㅇ 맞어
                            ChangeSTATE(STATE.TAGET2);
                        }
                    }

                    //전체거리- 장애물까지거리= 남은거리>1.5f (장애물이 있고, 가야할길이 남았다.)
                    else if (T - s > 1.5f)
                    {
                        ChangeSTATE(STATE.ATTACK);
                    }
                    
                }
                                                             
                break;


            case STATE.TAGET2:
                //Target1과 동일
                float T2 = Vector3.Distance(this.transform.position, objpos2);
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

                //장애물이 있을때

                //적군이 존재하고 일정거리 이상이면 근처로 이동, 일정거리 이하면 공격

                if (m_Enemy != null)
                {
                    //dist= 적위치-장애물위치 
                    float dist = Vector3.Distance(this.transform.position, m_Enemy.transform.position);

                    //적-장애물이 조금 멀면
                    if (dist > 5.5f)
                    {
                        //네비 목적지= 장애물위치
                        m_Navi.SetDestination(m_Enemy.transform.position);
                    }
                    //적-장애물이 가까우면
                    else if (dist < 5.5f)
                    {
                        //공격
                        ChangeSTATE(STATE.BATTLE);
                    }
                    // Debug.Log(dist);
                }
               
                //장에믈이없으면
                else if (m_Enemy == null)
                {
                    ChangeSTATE(STATE.TAGET2);
                }
                break;

            //??????????????????
            case STATE.BATTLE:


                
                //delay깍아주고 Mathf.Epsilon(0보다작은수)

                //일정 공격속도 로 공격

                if (attackdelay <= Mathf.Epsilon)
                {
                    attackdelay = 2.0f;
                    Onattack();
                    
                    //죽였다 싶으면 다시 target2로 이동
                
                    if (m_Enemy == null)
                    {
                        ChangeSTATE(STATE.TAGET2);
                    }
                }
                attackdelay -= Time.smoothDeltaTime;


                break;
            case STATE.GOAL:

               
                break;
            case STATE.DEAD:

                break;
        }

    }
    //충돌하면 경로 재계산하고 ATTACK로 바뀌고 적군 제거하고 다시 이동.


    

    private void OnCollisionStay(Collision collision)


    {


        //   Debug.Log("충돌 중!");

    }


   //  void Onattack(Obstacle enemy)
   // {
   //     //아 이런. 이걸 
   //     m_Enemy.OnDamage(m_Monsterinfo.MonsterAttack);
   //     //    //큐프
   //     Debug.Log("공격3");
   //     // //   Debug.Log(m_Monsterinfo.MonsterAttack);
   // }



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
            m_Enemy.OnDamage(m_Monsterinfo.MonsterAttack);

            //큐프
            Debug.Log("공격3");
        }//   Debug.Log(m_Monsterinfo.MonsterAttack);
    }
    public void OnDamage(float dmg)
    {

        if (!m_Monsterinfo.UpdateHP(-dmg))
        {

            Debug.Log("사망");
        }
    }

   //몬스터 삭제 함수, 리스트도 같이 지운다.
    void death()
    {
        //for (int i = 0; i< MonsterCreater.m_EnemyList.Count; i++)
        //{ 
        //    if (this.gameObject == MonsterCreater.m_EnemyList[i])
        //    {
        //        MonsterCreater.m_EnemyList.Remove(MonsterCreater.m_EnemyList[i]);
        //    }

            Destroy(this.gameObject);

        

    }

    //void Death()
    //{
    //    for (int i = 0; i < MonsterCreater.m_EnemyList.Count; ++i)
    //    {            
    //        if (this.gameObject == MonsterCreater.m_EnemyList[i])
    //        {
    //            MonsterCreater.m_EnemyList.Remove(MonsterCreater.m_EnemyList[i]);
    //        }
    //    }

    //    Destroy(this.gameObject);
    //}


}
