using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonUI : MonoBehaviour
{
    //hirec에 존재
    public Button BuildTowerBasic;
    public Button BuildTowerHeal;
    public Button BuildTowerObs;
    public Button BuildTowerDagger;
    public Button TowerUpgrade;
    public Button TowerDelete;

    public NodeManager m_NodeMng;

    public Vector3 TempPos;


    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    //타워 생성 UI Off (비활성화)
    public void BuildOffButton()
    {
        BuildTowerBasic.gameObject.SetActive(false);
        BuildTowerHeal.gameObject.SetActive(false);
        BuildTowerObs.gameObject.SetActive(false);
        BuildTowerDagger.gameObject.SetActive(false);
    }

    //타워 생성 UI On (활성화)
    public void BuildOnButton()
    {
        //TempPos는 월드포지션
        //UI전용 카메라의 마우스클릭 position의 좌표를 받아옴
        TempPos = Camera.allCameras[1].ScreenToWorldPoint(Input.mousePosition);
        TempPos.z = 0.0f; //이z값이 왜 효력이 없는지 물어보기
        //TempPos.x = 0.0f;
        //TempPos.y = 0.0f;
        BuildTowerBasic.gameObject.transform.position = TempPos;
        //lpos는 로컬 포지션
         Vector3 lpos = BuildTowerBasic.gameObject.transform.localPosition;
         lpos.z = 0;
        //오른쪽으로 40, 아래로 30이동
        lpos.x += 75;
        lpos.y -= 50;
        BuildTowerBasic.gameObject.transform.localPosition = lpos;

        BuildTowerHeal.gameObject.transform.position = TempPos;
        lpos.x += 75;
        BuildTowerHeal.transform.localPosition = lpos;

        BuildTowerObs.gameObject.transform.position = TempPos;
        lpos.x += 75;
        BuildTowerObs.transform.localPosition = lpos;

        BuildTowerDagger.gameObject.transform.position = TempPos;
        lpos.x += 75;
        BuildTowerDagger.transform.localPosition = lpos;

        BuildTowerBasic.gameObject.SetActive(true);
        BuildTowerHeal.gameObject.SetActive(true);
        BuildTowerObs.gameObject.SetActive(true);
        BuildTowerDagger.gameObject.SetActive(true);

    }

    //타워 업그레이드, 삭제 UI On (활성화)
    public void TowerOnBtn()
    {
        ////TempPos는 월드포지션
        //TempPos = Camera.allCameras[1].ScreenToWorldPoint(Input.mousePosition);
        //TempPos.z = 0.0f;
        //TowerUpgrade.transform.position = TempPos;
        //Vector3 lpos = TowerUpgrade.transform.localPosition;
        //lpos.x += 100;
        //lpos.y -= 30;
        //TowerUpgrade.transform.localPosition = lpos;
        //
        //TowerDelete.transform.position = TempPos;
        //lpos.x += 100;
        //TowerDelete.transform.localPosition = lpos;

        TowerUpgrade.gameObject.SetActive(true);
        TowerDelete.gameObject.SetActive(true);
    }

    //타워 업그레이드, 삭제 UI Off (비활성화)
    public void TowerOffBtn()
    {
        TowerUpgrade.gameObject.SetActive(false);
        TowerDelete.gameObject.SetActive(false);
    }

}
