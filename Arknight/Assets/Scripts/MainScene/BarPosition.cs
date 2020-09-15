using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarPosition : MonoBehaviour
{
    public Camera my_camera;

    void Start()
    {
        my_camera = Camera.allCameras[0];
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + my_camera.transform.rotation * Vector3.back, my_camera.transform.rotation * Vector3.up);
        
    }
}
