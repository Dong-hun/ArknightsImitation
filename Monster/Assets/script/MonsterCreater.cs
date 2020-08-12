using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCreater : MonoBehaviour
{
    public bool GameOver = false;
    public int MaxMonster = 3;
    public int TotalMonster = 0;
    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = Instantiate(Resources.Load("Temp Target")) as GameObject;
        GameObject obj2 = Instantiate(Resources.Load("Temp Target2")) as GameObject;

        StartCoroutine(Monster());
        StartCoroutine(Boss());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator Monster()
    {//while 문 밖에다가 설정할 것.
        int MonsterCount = 0;

        while (!GameOver)
        {

            if (MonsterCount<MaxMonster){
                yield return new WaitForSeconds(2.0f);

                GameObject obj = Instantiate(Resources.Load("Temp Monster(Moving)")) as GameObject;
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
            if (TotalMonster == 1)
            {
                yield return new WaitForSeconds(3.0f);

                GameObject obj = Instantiate(Resources.Load("3DBOSS")) as GameObject;
            }
            else
            {
                yield return null;

            }
        }
    }


}
