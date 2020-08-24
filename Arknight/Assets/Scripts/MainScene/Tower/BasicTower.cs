﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BasicTower : TowerManager

{

    /* 타워매니저를 상속받아서 만들어지는 각 타워들의 스크립트 */
    // 나중에 스크립트 이름을 타워이름으로 바꿀것 (현재 임시, 바꾸면 이 주석 지울것)
    // 기본 공격 하는 타워
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

    public void GetTileData()
    {
        TileX = 0;
        int test = TileY;
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Init(50, 50, 5.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    // 타일 위치 세팅
    public void SetTileNumber(int x, int y)
    {
        m_TileX = x;
        m_TileY = y;
    }

    // 상태 변경시 한번 호출될 함수
    protected override void ChangeState(STATE s)
    {
        if (m_State == s) return;
        m_State = s;

        switch (m_State)
        {
            case STATE.IDLE:
                //m_Anim.SetBool("Idle", true);
                break;
            case STATE.BATTLE:
                //m_Anim.SetBool("Attack", true);
                break;
            case STATE.DIE:
                //m_Anim.SetBool("Die", true);
                break;
        }
    }

    // 프레임 마다 업데이트 할 함수
    protected override void StateProcess()
    {
        switch (m_State)
        {
            case STATE.IDLE:
                break;
            case STATE.BATTLE:
                Attack();
                break;
            case STATE.DIE:
                break;
        }
    }

    // 대기
    protected override void Idle()
    {

    }

    // 회전
    void Rotation(Enemy enemy)
    {
        // 적과의 방향 구함
        Vector3 dir = enemy.transform.position -
            this.transform.position;

        // 해당 방향으로 회전
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
            Quaternion.LookRotation(dir), Time.smoothDeltaTime * 360.0f);
    }

    // 공격
    protected override void Attack()
    {
        // 가장 가까운 적 방향으로 회전
        Rotation(GetNearestEnemy());

        // 딜레이가 0이하가 된다면
        if (m_AttackDelay <= Mathf.Epsilon)
        {
            // 제일 가까운 적이 살아있다면
            if (GetNearestEnemy() != null)
            {
                // Attack 트리거 발동
                m_Anim.SetTrigger("Attack");

                // 다시 딜레이 설정
                m_AttackDelay = 3.0f;
            }
            // 적이 죽었다면
            else
            {
                // IDLE상태로 바꿈
                ChangeState(STATE.IDLE);
            }
        }

        // 딜레이 감소
        m_AttackDelay -= Time.deltaTime;
    }

    // 적에게 데미지 줌
    public void OnDamage(Enemy enemy)
    {
        // PlayAnimationEvent에서 공격 모션일때 이 함수 호출
    }

    // 사망
    protected override void Die()
    {

    }

    // 적 추가
    protected override void AddEnemy(Enemy enemy)
    {
        // 매개변수가 null이면 리턴
        if (enemy == null) return;

        // 리스트에 채워줌
        m_EnemyList.Add(enemy);

    }

    // 가장 가까운 적 받아오기
    protected override Enemy GetNearestEnemy()
    {
        // 리스트가 비었으면 리턴
        if (m_EnemyList.Count == 0) return null;

        // 첫 거리는 아주크게 설정
        float dist = 999f;

        // 가장 가까운 인덱스 저장용
        int sel = -1;

        // 있는 적 다 검사
        for (int i = 0; i < m_EnemyList.Count; ++i)
        {
            // 거리 계산
            float temp = Vector3.Distance(this.transform.position,
                m_EnemyList[i].transform.position);

            // 계산된 거리가 이전의 가장 가까운거리보다 작다면
            if (temp < dist)
            {
                // 계산된 거리를 가장 가까운 거리로 바꿔주고
                dist = temp;

                // 해당 인덱스 번호 저장
                sel = i;
            }
        }

        // 해당 인덱스 번호의 적 리턴
        return m_EnemyList[sel];
    }

    // 적 제거
    protected override void RemoveEnemy(Enemy enemy)
    {
        if (enemy == null) return;

        for (int i = 0; i < m_EnemyList.Count; ++i)
        {
            if (m_EnemyList[i].transform == enemy.transform)
            {
                m_EnemyList.Remove(m_EnemyList[i]);
            }
        }

        if (m_EnemyList.Count == 0)
            ChangeState(STATE.IDLE);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 사정거리에 들어온 적 추가
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            AddEnemy(other.gameObject.GetComponent<Enemy>());
            ChangeState(STATE.BATTLE);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // 적 방향으로 바라보기
        //if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        //{
        //    Vector3 dir = other.transform.position - transform.position;
        //    dir.Normalize();
        //
        //    this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
        //        Quaternion.LookRotation(dir), Time.smoothDeltaTime * 360.0f);
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        // 사정거리밖으로 나간 적 제거
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            RemoveEnemy(other.gameObject.GetComponent<Enemy>());
        }
    }
}