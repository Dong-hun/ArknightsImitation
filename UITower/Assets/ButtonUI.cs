using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    public Button BuildToTowerButton1;
    public Button BuildToTowerButton2;
    public Button BuildToTowerButton3;
    public Button TowerUpgrade;
    public Button TowerDelete;

    void Start()
    {
        BuildToTowerButton1.gameObject.SetActive(false);
        BuildToTowerButton2.gameObject.SetActive(false);
        BuildToTowerButton3.gameObject.SetActive(false);
        TowerUpgrade.gameObject.SetActive(false);
        TowerDelete.gameObject.SetActive(false);
    }

    public void BuildOffButton()
    {
        BuildToTowerButton1.gameObject.SetActive(false);
        BuildToTowerButton2.gameObject.SetActive(false);
        BuildToTowerButton3.gameObject.SetActive(false);
    }

    public void BuildOnButton()
    {
        BuildToTowerButton1.gameObject.SetActive(true);
        BuildToTowerButton2.gameObject.SetActive(true);
        BuildToTowerButton3.gameObject.SetActive(true);
    }

}
