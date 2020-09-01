using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Obstacle : TowerManager
{
    public MonsterStat Monsterstatinfo;

    [Header("Unity Stuff")]
    public Image HealthBar;
    
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

    new void Start()
    {
        base.Start();
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
            case STATE.DEATH:

                Death();
                break;
        }
    }
    public void OnDamage(float dmg)
    {
        Debug.Log("인식");
        Monsterstatinfo.BossAttack = dmg;

        Debug.Log(Monsterstatinfo.CurrentHP.ToString());
        Debug.Log(HealthBar.fillAmount.ToString());

       HealthBar.fillAmount = Monsterstatinfo.CurrentHP/Monsterstatinfo.MaxHp;

        if (!Monsterstatinfo.UpdateHP(-dmg))
        {
            

            //HealthBar.fillAmount = Monsterstatinfo.CurrentHP/Monsterstatinfo.MaxHp;
            ChangeState(STATE.DEATH);
        }
    }

    void Death()
    {
        for (int i = 0; i < m_BuildManager.m_ObstacleList.Count; ++i)
        {
            if(this.gameObject == m_BuildManager.m_ObstacleList[i])
            {
                m_BuildManager.m_ObstacleList.Remove(m_BuildManager.m_ObstacleList[i]);
            }
        }

        Destroy(this.gameObject);
    }
}
