using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    //public Node m_Node; 다른스크립트 함수호출방법(1)
    
    public static BuildManager instance; //다른 스크립트에서 BuildManager스크립트 사용가능
    public ButtonUI m_Button;

    public GameObject SelectNode;//선택한 노드에
    public GameObject Tower1; //타워를 만든다
    public GameObject Tower2; //타워를 만든다
    public GameObject Tower3; //타워를 만든다

    

    //타워를 만드는 함수
    public void Start()
    {
        instance = this;//instance 변수안에 BuildManager 스크립트를 넣어준다.
        
        //m_Node.OnMouseUp();다른스크립트 함수호출방법(1)
    }
    public void BuildToTower1()
    {
        
        Instantiate(Tower1, SelectNode.transform.position, Quaternion.identity); //(생성할 오브젝트,생성될 위치,생성될 각도)
        m_Button.BuildOffButton();
        
        //ChangeTower.instance.Click1();

    }

    public void BuildToTower2()
    {
        
        Instantiate(Tower2, SelectNode.transform.position, Quaternion.identity); //(생성할 오브젝트,생성될 위치,생성될 각도)
        m_Button.BuildOffButton();


        //ChangeTower.instance.Click2();

    }

    public void BuildToTower3()
    {
        
        Instantiate(Tower3, SelectNode.transform.position, Quaternion.identity); //(생성할 오브젝트,생성될 위치,생성될 각도)
        m_Button.BuildOffButton();



        //ChangeTower.instance.Click3();

    }



}

