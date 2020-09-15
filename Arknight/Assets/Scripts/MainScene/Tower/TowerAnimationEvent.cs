using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAnimationEvent : MonoBehaviour
{
    // 딜리게이트
    public DelAttack m_Attack;
    public DelCor m_Death;
    public DelCor m_BasicTowerSkill;
    public DelCor m_HealTowerSkill;
    public DelAttack m_Recovery;


    // 공격
    public void OnAttack()
    {
        // 해당 오브젝트의 레이어가 BasicTower면
        if (this.transform.parent.gameObject.layer == LayerMask.NameToLayer("BasicTower"))
        {
            if (this.GetComponentInParent<BasicTower>().Target != null)
            {
                // 공격 함수 실행
                m_Attack?.Invoke(this.GetComponentInParent<BasicTower>().Target);

                // BasicTower의 Effect효과 설정
                GameObject effect = Instantiate(Resources.Load("Tower/Basic/Effect/Hit")) as GameObject;
                effect.transform.position = this.GetComponentInParent<BasicTower>().Target.transform.position;
                effect.transform.localScale *= 3f;
            }
        }
        // 해당 오브젝트의 레이어가 HealTower면
        else if (this.transform.parent.gameObject.layer == LayerMask.NameToLayer("HealTower"))
        {
            // HealTower의 Target이 null이 아니면
            if (this.GetComponentInParent<HealTower>().Target != null)
            {
                // 회복 함수 실행
                m_Recovery?.Invoke(this.GetComponentInParent<HealTower>().Target);

                // HealTower의 Effect 효과 설정
                GameObject effect = Instantiate(Resources.Load("Tower/Heal/Effect/Recovery")) as GameObject;
                Vector3 pos = this.GetComponentInParent<HealTower>().Target.transform.position;
                pos.y = 1.0f;
                effect.transform.position = pos;
                effect.transform.localScale *= 5.0f;
            }
        }
    }

    public void OnSkill()
    {
        if (this.transform.parent.gameObject.layer == LayerMask.NameToLayer("BasicTower"))
        {
            StartCoroutine(m_BasicTowerSkill?.Invoke(10.0f));
            //StopCoroutine(this.GetComponentInParent<BasicTower>().ActiveSkill());

            // Effect 추가할것
            GameObject effect = Instantiate(Resources.Load("Tower/Basic/Effect/SpeedUp")) as GameObject;
            effect.transform.position = this.transform.position;
            effect.transform.localScale *= 3.0f;
        }
        else if (this.transform.parent.gameObject.layer == LayerMask.NameToLayer("HealTower"))
        {
            StartCoroutine(m_HealTowerSkill?.Invoke(10.0f));

            // Effect 추가할것
            GameObject effect = Instantiate(Resources.Load("Tower/Heal/Effect/DamageUp")) as GameObject;
            Vector3 pos = this.GetComponentInParent<HealTower>().Target.transform.position;
            pos.y = 0.5f;
            effect.transform.position = pos;
            effect.transform.localScale *= 5.0f;
        }

    }

    // 사망

    public void OnDeath()
    {
        // 해당 딜리게이트가 있으면 실행
        StartCoroutine(m_Death?.Invoke(3.0f));
    }
}
