using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat
{
    protected int m_HP = 0;         // 체력
    protected int m_Attack = 0;     // 공격력

    virtual protected void SetStat(int hp, int attack)
    {
        m_HP = hp;
        m_Attack = attack;
    }
}
