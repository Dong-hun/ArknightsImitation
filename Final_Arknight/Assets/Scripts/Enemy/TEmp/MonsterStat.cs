using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : MonoBehaviour
{
    public int MonsterAttack=70;
    public int BossAttack = 10;
    public float AttackDelay = 2.0f;
    public int MaxHp = 233;
    public int CurrentHP = 0;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHP = MaxHp;

    }
    public bool UpdateHP(int v)
    {
        CurrentHP += v;
        if (CurrentHP > MaxHp) CurrentHP = MaxHp;
        if (CurrentHP <= 0)
        {
            //사망
            CurrentHP = 0;
            return false;
        }
        //참이면 앞쪽 거짓이면 뒤쪽이 계산된다.
        return true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
