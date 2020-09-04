using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonUI : MonoBehaviour
{
    //hirec에 존재
    public Button BuildTowerBtn1;
    public Button BuildTowerBtn2;
    public Button BuildTowerBtn3;
    public Button TowerUpgrade;
    public Button TowerDelete;

    public Vector3 TempPos;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    //타워 생성 UI Off (비활성화)
    public void BuildOffButton()
    {
        BuildTowerBtn1.gameObject.SetActive(false);
        BuildTowerBtn2.gameObject.SetActive(false);
        BuildTowerBtn3.gameObject.SetActive(false);
    }

    //타워 생성 UI On (활성화)
    public void BuildOnButton()
    {
        //TempPos는 월드포지션
        //UI전용 카메라의 마우스클릭 position의 좌표를 받아옴
        TempPos = Camera.allCameras[1].ScreenToWorldPoint(Input.mousePosition);
        TempPos.z = 0.0f;
        BuildTowerBtn1.gameObject.transform.position = TempPos;
        //lpos는 로컬 포지션
        Vector3 lpos = BuildTowerBtn1.gameObject.transform.localPosition;
        //오른쪽으로 40, 아래로 30이동
        lpos.x += 80;
        lpos.y -= 30;
        BuildTowerBtn1.gameObject.transform.localPosition = lpos;


        BuildTowerBtn2.gameObject.transform.position = TempPos;
        lpos.x += 40;
        BuildTowerBtn2.transform.localPosition = lpos;


        BuildTowerBtn3.gameObject.transform.position = TempPos;
        lpos.x += 40;
        BuildTowerBtn3.transform.localPosition = lpos;

        BuildTowerBtn1.gameObject.SetActive(true);
        BuildTowerBtn2.gameObject.SetActive(true);
        BuildTowerBtn3.gameObject.SetActive(true);

    }

    //타워 업그레이드, 삭제 UI On (활성화)
    public void TowerOnBtn()
    {

        //TempPos는 월드포지션
        TempPos = Camera.allCameras[1].ScreenToWorldPoint(Input.mousePosition);
        TempPos.z = 0.0f;
        TowerUpgrade.transform.position = TempPos;
        Vector3 lpos = TowerUpgrade.transform.localPosition;
        lpos.x += 80;
        lpos.y -= 30;
        TowerUpgrade.transform.localPosition = lpos;

        TowerDelete.transform.position = TempPos;
        lpos.x += 60f;
        TowerDelete.transform.localPosition = lpos;

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
