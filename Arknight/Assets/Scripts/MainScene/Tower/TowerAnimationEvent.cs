using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAnimationEvent : MonoBehaviour
{
    // 공격함수 담는 딜리게이트
    public DelAttack m_Attack;
    public DelRecovery m_Recovery;
    public DelDeath m_Death;

    // 공격
    public void OnAttack()
    {
        if (this.transform.parent.gameObject.layer == LayerMask.NameToLayer("BasicTower"))
        {
            // 해당 딜리게이트가 있으면 실행
            m_Attack?.Invoke(this.GetComponentInParent<BasicTower>().Target);
        }
        else if (this.transform.parent.gameObject.layer == LayerMask.NameToLayer("HealTower"))
        {
            // 해당 딜리게이트가 있으면 실행
            StartCoroutine(m_Recovery?.Invoke());
        }
    }

    // 사망

    public void OnDeath()
    {
        // 해당 딜리게이트가 있으면 실행
        StartCoroutine(m_Death?.Invoke(3.0f));
    }
}
