using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAnimationEvent : MonoBehaviour
{
    // 공격함수 담는 딜리게이트
    public DelAttack m_Attack;
    public DelDeath m_Death;
    public DelAttack m_Recovery;

    public GameObject BasicTowerAttackEffect;           // 기본타워 기본공격 이펙트
    public GameObject BasicTowerSkillEffect;            // 기본타워 스킬공격 이펙트

    public GameObject HealTowerAttackEffect;            // 힐타워 기본공격 이펙트
    public GameObject HealTowerSkillEffect;             // 힐타워 스킬공격 이펙트

    // 공격
    public void OnAttack()
    {
        if (this.transform.parent.gameObject.layer == LayerMask.NameToLayer("BasicTower"))
        {
            // 해당 딜리게이트가 있으면 실행
            m_Attack?.Invoke(this.GetComponentInParent<BasicTower>().Target);

            GameObject effect = Instantiate(BasicTowerAttackEffect);
            effect.transform.position = this.GetComponentInParent<BasicTower>().Target.transform.position;
            effect.transform.localScale *= 3f;
        }
        else if (this.transform.parent.gameObject.layer == LayerMask.NameToLayer("HealTower"))
        {
            m_Recovery?.Invoke(this.GetComponentInParent<HealTower>().Target);

            GameObject effect = Instantiate(HealTowerAttackEffect);
            effect.transform.position = this.GetComponentInParent<HealTower>().Target.transform.position;
            effect.transform.localScale *= 3f;
        }
    }

    // 사망

    public void OnDeath()
    {
        // 해당 딜리게이트가 있으면 실행
        StartCoroutine(m_Death?.Invoke(3.0f));
    }
}
