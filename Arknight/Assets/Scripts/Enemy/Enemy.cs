using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyManager

    /* 생성할 적 클래스 */
{

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        //m_StartPos = GameObject.Find("Plane").transform.position;
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
