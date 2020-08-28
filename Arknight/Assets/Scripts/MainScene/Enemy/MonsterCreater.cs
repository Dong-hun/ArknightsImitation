﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCreater : MonoBehaviour
{
    public bool GameOver = false;
    public int MaxMonster = 3;
    public int TotalMonster = 0;
    public GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Monster());
        StartCoroutine(Boss());
    }

    // Update is called once per frame
    void Update()
    {
        MakingD();
    }


    IEnumerator Monster()
    {//while 문 밖에다가 설정할 것.
        int MonsterCount = 0;

        while (!GameOver)
        {

            if (MonsterCount<MaxMonster){
                yield return new WaitForSeconds(2.0f);

                GameObject obj = Instantiate(Resources.Load("Enemy/Slime")) as GameObject;
                obj.transform.SetParent(this.transform);
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

                GameObject obj = Instantiate(Resources.Load("Enemy/3DBOSS")) as GameObject;
                obj.transform.SetParent(this.transform);
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