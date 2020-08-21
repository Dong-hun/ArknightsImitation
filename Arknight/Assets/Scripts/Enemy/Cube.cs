using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public int co;
    public MonsterStat CUBESTAT;
    public GameObject obj;
<<<<<<< HEAD:Monster/Assets/script/Cube.cs
    public Moving STAT;
    // Start is called before the first frame update
    void Start()
    {
         obj = GameObject.Find("Cube");
=======
    public EnemyManager STAT;
    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.Find("Cube");
>>>>>>> Beta:Arknight/Assets/Scripts/Enemy/Cube.cs
        CUBESTAT = GetComponent<MonsterStat>();
    }
    //해당 몬스터가 있는데 그 몬스터의 몬스터 스텟.

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)

    {

        //   Debug.Log("충돌 시작!");

    }



    // Collider 컴포넌트의 is Trigger가 false인 상태로 충돌중일 때
    //바닥은 제외해야 한다.
    private void OnCollisionStay(Collision collision)

    {


        // Debug.Log("충돌 중!");

    }



    // Collider 컴포넌트의 is Trigger가 false인 상태로 충돌이 끝났을 때

    private void OnCollisionExit(Collision collision)

    {
        // Debug.Log("충돌 끝!");

    }

<<<<<<< HEAD:Monster/Assets/script/Cube.cs
    //생성할 변수를 다루는 함수에 추가
    public void OnDamage(int dmg)
    {
        Debug.Log("인식");

        STAT.OnDamage(CUBESTAT.BossAttack);
    }
=======
>>>>>>> Beta:Arknight/Assets/Scripts/Enemy/Cube.cs
}

