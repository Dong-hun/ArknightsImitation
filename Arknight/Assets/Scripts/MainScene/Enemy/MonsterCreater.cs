using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCreater : MonoBehaviour
{
    public bool GameOver = false;
    public int MaxMonster = 3;
    public int TotalMonster = 0;
     GameObject obj;
    public List<Enemy> m_EnemyList;
    int MonsterCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Monster());
        StartCoroutine(Boss());
        StartCoroutine(Monster2());

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator Monster()
    {//while 문 밖에다가 설정할 것.

        while (!GameOver)
        {

            if (MonsterCount<MaxMonster){
                yield return new WaitForSeconds(2.0f);

                GameObject obj = Instantiate(Resources.Load("Enemy/Slime")) as GameObject;
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
    IEnumerator Monster2()
    {//while 문 밖에다가 설정할 것.

        while (!GameOver)
        {

            if (MonsterCount < MaxMonster)
            {
                yield return new WaitForSeconds(3.0f);

                GameObject obj = Instantiate(Resources.Load("Enemy/TURTLES")) as GameObject;
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


}
