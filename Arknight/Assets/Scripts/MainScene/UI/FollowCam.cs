using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowCam : MonoBehaviour
{
    public LayerMask m_CastLayer;
    public Transform m_RotBasePoint = null;
    public float collisionOffset = 1.0f;    // 가상의 구를 만들기 위한 반지름

    public float m_OriginDist = 0.0f;       // 원래 플레이어 - 카메라 간의 거리
    public float m_TargetDist = 0.0f;       // 보정된 거리float m_RotSpeed = 180.0f;
    public float m_CameraDist = 0.0f;       // 보정이 끝난 플레이어 - 카메라 간의 거리

    public float m_RotSpeed = 180.0f;       // 회전 속도(초당 180도로 돌린다고 생각하면 될듯?)
    public float m_ZoomSpeed = 300.0f;      // 줌 속도
    public float m_LerpSpeed = 10.0f;       // 보간 속도

    //public Vector3 m_Offset;                // 보정값
    public Vector3 m_Rotation;              // 자신회전값

    public Vector2 m_ZoomRange;             // 줌 제한 값
    public Vector2 m_RotXRange;             // 회전 제한 값

    // Start is called before the first frame update
    void Start()
    {
        m_OriginDist = m_TargetDist = m_CameraDist = Vector3.Distance(m_RotBasePoint.position, transform.position);
        //m_Offset = new Vector3(0, 1.5f, 0);
        m_RotXRange = new Vector2(15, 80);
        m_ZoomRange = new Vector2(20, 50);

        m_Rotation = transform.rotation.eulerAngles;
    }



    // Update is called once per frame
    void Update()
    {
        // 우클릭시 회전
        if(Input.GetMouseButton(1))
        {
            m_Rotation.x -= Input.GetAxis("Mouse Y") * m_RotSpeed * Time.deltaTime;             // 상하 회전
            //m_Rotation.y += Input.GetAxis("Mouse X") * m_RotSpeed * Time.deltaTime;

            // 회전 제한을 둠
            m_Rotation.x = Mathf.Clamp(m_Rotation.x, m_RotXRange.x, m_RotXRange.y);
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            m_TargetDist -= Input.GetAxis("Mouse ScrollWheel") * m_ZoomSpeed * Time.deltaTime;  // 마우스 휠로 확대 축소가 됨
            m_TargetDist = Mathf.Clamp(m_TargetDist, m_ZoomRange.x, m_ZoomRange.y);             // 줌 제한을 둠
            m_OriginDist = m_TargetDist;                                                        // 원래거리에 현재 거리 대입
        }

        // 실제 카메라 위치를 보정된 위치로 보간시켜줌 
        m_CameraDist = Mathf.Lerp(m_CameraDist, m_TargetDist, Time.deltaTime * m_LerpSpeed);

        // 카메라를 바라보는 오브젝트에 위치 시킴 (Offset만큼 더해져서 보는 시점에 오프셋만큼 변경해줌)
        transform.position = m_RotBasePoint.position;

        // 회전값을 적용함
        transform.rotation = Quaternion.Euler(m_Rotation);

        // 카메라 Foward방향의 반대방향으로 원하는 거리만큼 이동시킴 (+= 해줘야 위치로 이동됨)
        transform.position += -transform.forward * m_CameraDist;
    }
}
