using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public MonsterCreater m_MonsterCreater;
    public TextMeshPro m_MonsterCount;

    // Start is called before the first frame update
    void Start()
    {
        m_MonsterCreater = GameObject.Find("MonsterCreater").GetComponent<MonsterCreater>();
        //StartCoroutine(ShowMonsterCount());
    }

    // Update is called once per frame
    void Update()
    {
        //m_MonsterCount.text = m_MonsterCreater.m_EnemyList.Count.ToString() + " / 300";
    }

    IEnumerator ShowMonsterCount()
    {
        while(true)
        {
            m_MonsterCount.text = m_MonsterCreater.m_EnemyList.Count.ToString() + " / 300";
            yield return null;
        }
    }
}
