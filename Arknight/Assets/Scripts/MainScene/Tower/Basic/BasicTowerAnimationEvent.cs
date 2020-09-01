using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTowerAnimationEvent : MonoBehaviour
{
    // Start is called before the first frame update

    // 공격함수 담는 딜리게이트
    public DelAttack m_Attack;

    // 공격
    public void OnAttack()
    {
        // 해당 딜리게이트가 있으면 실행
        m_Attack?.Invoke(this.GetComponentInParent<BasicTower>().m_Target);
    }
}
