using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public ButtonUI m_Button;
    public Color StartColor;
    public Color SelectColor;
    public Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();//mesh Renderer 컴포넌트를 담는다.
        StartColor = rend.material.color;
    }

    //마우스 클릭하고 뗏을때 발동(Box collider가 있어야 발동)
    private void OnMouseUp()
    {
        rend.material.color=SelectColor;
        BuildManager.instance.SelectNode = gameObject;//this.gameObject 나 자신(Node)가 선택된 것이기 때문에
        m_Button.BuildOnButton();//노드 클릭했을때 ui뜨게 만들기
    }

    // Update is called once per frame
   
}
