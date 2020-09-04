using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MonsterStat : MonoBehaviour
{
    public float MonsterAttack = 1.0f;
    public float BossAttack = 2.0f;
    public float AttackDelay = 2.0f;
    public float MaxHp = 233.0f;
    public float CurrentHP = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHP = MaxHp;

    }
    public bool UpdateHP(float v)
    {
        CurrentHP += v;
        if (CurrentHP > MaxHp) CurrentHP = MaxHp;
        if (CurrentHP <= 0.0f)
        {
            //사망
            CurrentHP = 0.0f;
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
