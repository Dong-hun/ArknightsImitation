using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCreater : MonoBehaviour
{
    public bool GameOver = false;
    public int MaxMonster = 3;
    public int TotalMonster = 0;
    public GameObject obj;
    public List<Enemy> m_EnemyList;
   public int MonsterCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Monster());
        StartCoroutine(Monster2());
        StartCoroutine(Boss());
    }

    // Update is called once per frame
    void Update()
    {
        Seeknull();
    }


    IEnumerator Monster()
    {//while 문 밖에다가 설정할 것.

        while (!GameOver)
        {

            if (MonsterCount<MaxMonster){
                yield return new WaitForSeconds(2.0f);

                GameObject obj = Instantiate(Resources.Load("Enemy/Slime")) as GameObject;
                obj.transform.SetParent(this.transform);
                MonsterCount++;
                TotalMonster++;
                m_EnemyList.Add(obj.GetComponent<Enemy>());
            }
            else
            {
                yield return null;

            }
        }
    }
    IEnumerator Boss()
    {
        while (!GameOver)
        {
            if (TotalMonster == 1)
            {
                yield return new WaitForSeconds(3.0f);

                GameObject obj = Instantiate(Resources.Load("Enemy/3DBOSS")) as GameObject;
                obj.transform.SetParent(this.transform);
                m_EnemyList.Add(obj.GetComponent<Enemy>());
                MonsterCount++;
                TotalMonster++;

            }
            else
            {
                yield return null;

            }
        }
    }

    IEnumerator Monster2()
    {//while 문 밖에다가 설정할 것.

        while (!GameOver)
        {

            if (MonsterCount < MaxMonster)
            {
                yield return new WaitForSeconds(2.5f);

                GameObject obj = Instantiate(Resources.Load("Enemy/TURTLES")) as GameObject;
                obj.transform.SetParent(this.transform);
                m_EnemyList.Add(obj.GetComponent<Enemy>());
                MonsterCount++;
                TotalMonster++;
            }
            else
            {
                yield return null;

            }
        }
    }
    
    void Seeknull() //리스트 적군 없는 것 찾기
    {
        for (int i = 0; i < m_EnemyList.Count; i++)

        {
            if (m_EnemyList[i] == null)
            {
                m_EnemyList.Remove(m_EnemyList[i]); //리스트에서 삭제

                MonsterCount--; //몬스터 리젠을 위한 삭제
            }


        }

    }

}