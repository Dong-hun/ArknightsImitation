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
        
        BuildTowerBtn1.gameObject.SetActive(true);
        BuildTowerBtn2.gameObject.SetActive(true);
        BuildTowerBtn3.gameObject.SetActive(true);

    }

    //타워 업그레이드, 삭제 UI On (활성화)
    public void TowerOnBtn()
    {
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
