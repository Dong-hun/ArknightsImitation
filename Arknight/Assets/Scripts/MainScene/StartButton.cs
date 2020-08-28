using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public GameObject m_Option;
    public GameObject m_OptionButton;
    public GameObject m_OptionClick;
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

    public void OnOption()
    {
        //m_OptionButton.SetActive(false);
        m_Option.SetActive(true); // 클락하면 뜨고
    }   

    public void OnClick()
    {
        m_Option.SetActive(false); //클릭하면 사라짐
    }

}
