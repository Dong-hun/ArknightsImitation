using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{//유니티 헤즈페스.등 명령어
 //일단 이 스킨으로 만들어 보기.
 //몬스터 이동: 시작하면 자동으로 경로로 이동.
 // Start is called before the first frame update

    Main m_Main;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MakingD();


    }
    void MakingD()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject obj = Instantiate(Resources.Load("Cube")) as GameObject;

        }

    }

}
