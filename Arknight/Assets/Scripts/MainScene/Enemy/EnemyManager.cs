using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    /* 적 전체 관리 클래스 */

    public enum STATE                // 적 상태
    {                                   
        IDLE, MOVE, BATTLE, DIE         // 대기, 움직임, 전투, 사망(필요시 추가삭제 하시면 됩니당)
    }
    public STATE m_State;            // 상태값 저장 변수
    public NavMeshAgent m_Navi;                // NavMeshAgent 컴포넌트 접근용 변수
    public Vector3 m_StartPos;       // 시작지
    Vector3 _dest;                      // 목적지
    public Vector3 m_Dest
    {
        get
        {
            return m_Dest;
        }
        set
        {
            m_Dest = value;
        }
    }

    


    // Start is called before the first frame update
    protected void Start()
    {
        m_Navi.speed = 10.0f;           // 이동 속도
        m_Navi.angularSpeed = 360.0f;   // 회전 속도 (디그리)
        m_Navi.stoppingDistance = 0.5f; // 제동 거리 (이 거리부터 제동 시작)
        m_Navi.autoBraking = true;      // 도착 직전 감속을 true로 설정
                                        // -> (false로 하면 도착해서 감속하기 때문에 속도를 주체못하고 도착지보다 더 벗어남)
        
        m_Navi.SetDestination(m_Dest);  // 네비게이션에서도 도착지 설정
        m_State = STATE.IDLE;           // 상태를 움직임으로 설정해서 바로 이동하게 함
    }

    // Update is called once per frame
    protected void Update()
    {
        StateProcess();
    }

    // State 변경시 한번만 호출되는 함수
    protected void ChangeState(STATE s)
    {
        if (m_State == s) return;       // 같은 상태이면 리턴
        m_State = s;                    // 아니라면 상태 변경

        switch(m_State)                 // 상태에 따라서 호출
        {
            case STATE.IDLE:
                // 대기상태로 전환시에 한번 호출할 변수 or 함수 작성
                m_Dest = transform.position;    // 도착지를 현재 위치로 설정 (임시)
                break;
            case STATE.MOVE:
                m_Navi.SetDestination(m_Dest);
                // 움직임상태로 전환시에 한번 호출할 변수 or 함수 작성
                break;
            case STATE.BATTLE:
                // 전투상태로 전환시에 한번 호출할 변수 or 함수 작성
                break;
            case STATE.DIE:
                // 사망상태로 전환시에 한번 호출할 변수 or 함수 작성
                break;
        }
    }

    // 상태에 따라서 계속 업데이트 해줄 함수
    protected void StateProcess()
    {
        // 상태에 따라서 호출
        switch (m_State)
        {
            case STATE.IDLE:
                // 대기상태로 전환시에 업데이트할 변수 or 함수 작성
                break;
            case STATE.MOVE:
                // 움직임상태로 전환시에 업데이트할 변수 or 함수 작성
                break;
            case STATE.BATTLE:
                // 전투상태로 전환시에 업데이트할 변수 or 함수 작성
                break;
            case STATE.DIE:
                // 사망상태로 전환시에 업데이트할 변수 or 함수 작성
                break;
        }
    }

    // 도착지 체크 함수
    public void CheckDestination()
    {
        float originDist = Vector3.Distance(m_Dest, transform.position);            // 목적지 까지의 원래 거리
        float navDist = Vector3.Distance(m_Navi.destination, transform.position);   // nav.destination까지의 거리
        float distOffset = 0.5f;    // 거리 계산용 보정값

        // nav.destination의 거리에 보정치만큼 계산해서 원래 거리보다 작으면
        if (navDist - distOffset < originDist && originDist < navDist + distOffset)
        {
            ChangeState(STATE.IDLE);
        }
    }
}
