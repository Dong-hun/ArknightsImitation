using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Moving : MonoBehaviour
{//만들어야 하는것
 //1. 몬스터가 자동으로 목적지로 이동한다.
 //2. 몬스터가 이동하다가 장애물을 만나면 어떠한 이동 패턴을 보인다.
 //3.  haspath,pathPending 이용하고
 //4. 상태기계 상태로 전환해
 // 5.적 생성... (코루틴으로 생성..시간 몇초마다..) 몇 마리가 죽었으면 나온다. ㅇㅋ.
 
 //6. 보스 패턴 (1. 5초동안 기모아서 한방(무조건한방에 다죽임),2.체력이 낮고 빠른 보스3.체력이 5배이고 3바퀴 더돌고 들어가는 보스4.광역공격)
 //7. 장애물 부시기  경로..재계산 
 //8.레인지 시스템
   public enum STATE
    {
        TARGET1,TAGET2,ATTACK,DEAD,GOAL

    }
    public NavMeshAgent m_Navi;
    public GameObject obj;
    public GameObject obj2;
    public GameObject Goal;
    public STATE m_STATE;
    // Start is called before the first frame update
    void Start()
    {
        m_Navi = GetComponent<NavMeshAgent>();
        m_STATE = STATE.TARGET1;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
                m_Navi.SetDestination(hit.point);
        }
        StateProcess();
    }

    
    

    void ChangeSTATE(STATE s)
    {
        if (m_STATE == s) return;
        m_STATE = s;
        switch (m_STATE)
        {
            case STATE.TARGET1:

                m_Navi.SetDestination(obj.transform.position);



                break;
            case STATE.TAGET2:
                m_Navi.SetDestination(obj2.transform.position);

                break;

            case STATE.ATTACK:
                


                break;

        }
    }
    void StateProcess()
    {

        switch (m_STATE)
        {
            case STATE.TARGET1:
                m_Navi.SetDestination(obj.transform.position);

                if (!m_Navi.pathPending)
                {

                    if (m_Navi.remainingDistance <= m_Navi.stoppingDistance)
                    {

                        // if (!m_Navi.hasPath || m_Navi.velocity.sqrMagnitude == 0.0f)
                        if (!m_Navi.hasPath || m_Navi.velocity.sqrMagnitude == 0.0f)
                        {
                            // m_Navi.SetDestination(obj2.transform.position);
                            //상태를 멈춤상태로 만들거나 2번째 상태로 만드는게 나을듯. ㅇㅇ 맞어
                            ChangeSTATE(STATE.TAGET2);
                        }
                    }
                    

                }

                break;


            case STATE.TAGET2:
               
                    break;

        }

    }
    //충돌하면 경로 재계산하고 ATTACK로 바뀌고 적군 제거하고 다시 이동.


    private void OnCollisionStay(Collision collision)

    {


     //   Debug.Log("충돌 중!");

    }




}