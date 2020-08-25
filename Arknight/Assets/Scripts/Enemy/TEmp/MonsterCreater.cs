using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCreater : MonoBehaviour
{
    public bool GameOver = false;
    public int MaxMonster = 10;
    public int TotalMonster = 0;
    int MonsterCount = 0;
    public GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject obj = Instantiate(Resources.Load("Temp Target")) as GameObject;
        //GameObject obj2 = Instantiate(Resources.Load("Temp Target2")) as GameObject;

        StartCoroutine(Monster());
        StartCoroutine(Boss());
        StartCoroutine(Monster2());
    }

    // Update is called once per frame
    void Update()
    {
        MakingD();
    }


    IEnumerator Monster()
    {//while 문 밖에다가 설정할 것.

        while (!GameOver)
        {

            if (MonsterCount<MaxMonster){
                yield return new WaitForSeconds(2.0f);

                GameObject obj = Instantiate(Resources.Load("Enemy/Temp Monster(Moving)")) as GameObject;
               // GameObject obj2 = Instantiate(Resources.Load("Enemy/TURTLES")) as GameObject;
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
                yield return new WaitForSeconds(3.5f);

                GameObject obj2 = Instantiate(Resources.Load("Enemy/TURTLES")) as GameObject;
                MonsterCount++;
                TotalMonster++;
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
            if (TotalMonster == 4)
            {
                yield return new WaitForSeconds(3.0f);

                GameObject obj = Instantiate(Resources.Load("Enemy/3DBOSS")) as GameObject;
            }
            else
            {
                yield return null;

            }
        }
    }
    void MakingD()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            obj = Instantiate(Resources.Load("Enemy/Cube")) as GameObject;
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {

            Destroy(obj.gameObject);
        }
    }

}
