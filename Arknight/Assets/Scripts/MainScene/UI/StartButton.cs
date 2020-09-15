using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public GameObject m_Option;
    public GameObject m_OptionButton;
    public GameObject m_OptionClick;

    bool IsPause;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene()
    {

    }

    bool bPaused;
    //private void OnApplicationPause(bool pause)
    //{
    //    if(pause)
    //    {
    //        bPaused = true;
    //        Debug.Log("일시정지");
    //    }
    //    else
    //    {
    //        if(bPaused)
    //        {
    //            bPaused = false;
    //            Debug.Log("일시정지 상태 해제");
    //        } 
    //    }
    //}
    public void OnOption()
    {
        //m_OptionButton.SetActive(false);
        m_Option.SetActive(true); // 클락하면 뜨고
        /*일시정지 활성화*/
        if (IsPause == false)
        {
            Time.timeScale = 0;
            IsPause = true;
            return;
        }
    }   

    public void OnClick()
    {
        m_Option.SetActive(false); //클릭하면 사라짐

        /*일시정지 비활성화*/
        if (IsPause == true)
        {
            Time.timeScale = 1;
            IsPause = false;
            return;
        }
    }

}
