using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    int[] num = new int[3];
    public int Overnum;
    


    // Start is called before the first frame update
    void Start()
    {
        num[0] = 1;
        num[1] = 1;
        num[2] = 1;
        num[3] = 1;

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
