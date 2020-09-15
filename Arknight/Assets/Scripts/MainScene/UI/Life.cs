using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    int[] num = new int[2];
    public int Overnum;

    List<Sprite> m_LifeImage;
    

    // Start is called before the first frame update
    void Start()
    {
        num[0] = 1;
        num[1] = 1;
        num[2] = 1;
        num[3] = 1;

        int a = Screen.width;
        int b = Screen.height;

        // 이미지 3개 띄우고
        // 목숨이 깎일때마다 뒤에서 삭제


        print(num[3]);
    }

    public void LM ()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
