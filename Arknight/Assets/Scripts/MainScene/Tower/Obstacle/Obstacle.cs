using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : TowerManager
{
    public MonsterStat Monsterstatinfo;
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
        Monsterstatinfo.BossAttack = dmg;

        if (!Monsterstatinfo.UpdateHP(-dmg))
        {

            Destroy(this.gameObject);
            Debug.Log("사망");
        }
    }
}
