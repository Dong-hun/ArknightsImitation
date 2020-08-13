using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyManager

    /* 생성할 적 클래스 */
{
    // Start is called before the first frame update
    void Start()
    {
        m_State = STATE.MOVE;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
