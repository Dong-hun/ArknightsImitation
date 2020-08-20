﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DelVoid();

public abstract class TowerManager : MonoBehaviour
{
    /* 타워 관리 스크립트 */

    //==================== 상속해서 쓸것 (추상클래스)
    protected enum STATE                // 타워 상태
    {                               
        IDLE, BATTLE, DIE               // 대기, 전투, 사망
    }
    protected STATE m_State;            // 상태 받는 변수
    protected int m_HP;                 // 체력
    protected int m_MP;                 // 마력
    protected float m_Dist;             // 사거리
    protected int m_TileX;              // 타워 X좌표
    protected int m_TileY;              // 타워 Y좌표

    protected List<Enemy> m_EnemyList;
    protected DelVoid m_Attack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // 타워 세팅
    protected void Init(int hp, int mp, float dist)
    {
        m_State = STATE.IDLE;
        m_HP = hp;
        m_MP = mp;
        m_Dist = dist;
    }

    // 상태 변경시 한버 호출될 함수
    protected abstract void ChangeState(STATE s);

    // 프레임 마다 업데이트 할 함수
    protected abstract void StateProcess();
    
    // 공격
    protected abstract void Attack();

    // 사망
    protected abstract void Die();

    // 적 추가
    protected abstract void AddEnemy(Enemy enemy);

    // 가장 가까운 적 받아오기
    protected abstract Enemy GetNearestEnemy();
    
    // 적 제거
    protected abstract void RemoveEnemy(Enemy enemy);
}