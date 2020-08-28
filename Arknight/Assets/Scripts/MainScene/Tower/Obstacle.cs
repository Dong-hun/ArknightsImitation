using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : TowerManager
{
    public MonsterStat CUBESTAT;
    public MonsterStat m_Obinfo;
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

    private void Start()
    {

        base.Init();
    }

    protected override void ChangeState(STATE s)
    {
        if (m_State == s) return;
        m_State = s;

        switch(m_State)
        {
            case STATE.IDLE:
                break;
            case STATE.DIE:
                break;
        }
    }

    protected override void StateProcess()
    {
        switch (m_State)
        {
            case STATE.IDLE:
                break;
            case STATE.DIE:
                break;
        }
    }

    public void OnDamage(int dmg)
    {


        Debug.Log("인식");
        CUBESTAT.BossAttack = dmg;

        if (!m_Obinfo.UpdateHP(-dmg))
        {

            Destroy(this.gameObject);
            Debug.Log("사망");
        }

        //        if (this.gameObject != null)

        //   CUBESTAT.OnDamage(dmg);



    }
}
