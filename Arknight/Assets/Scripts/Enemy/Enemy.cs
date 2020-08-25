using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyManager

    /* 생성할 적 클래스 */
{

    // Start is called before the first frame update
    new void Start()
    {
<<<<<<< HEAD
        base.Start();
        //m_StartPos = GameObject.Find("Plane").transform.position;
=======
        this.transform.position = GameObject.Find("Start").GetComponent<Transform>().position;
        objpos = GameObject.Find("End").GetComponent<Transform>().position;
        objpos2 = GameObject.Find("Plane (80)").GetComponent<Transform>().position;

        //Vector3 DESTPOS = m_Navi.destination;
        m_Navi = GetComponent<NavMeshAgent>();
        m_STATE = STATE.CREATE;
        ChangeSTATE(STATE.TARGET1);
        m_Path = new NavMeshPath();

        //   m_Monsterinfo = GetComponent<MonsterStat>();
>>>>>>> DongHun
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        //if (m_State == STATE.IDLE)
        //{
        //    m_Dest = GameObject.Find("Plane (89)").transform.position;
        //    m_Navi.SetDestination(m_Dest);
        //    ChangeState(STATE.MOVE);
        //}
    }
}
