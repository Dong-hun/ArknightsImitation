using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowCam : MonoBehaviour
{
    public LayerMask m_CastLayer;
    public Transform m_RotBasePoint = null;

    public float m_OriginDist = 0.0f;       // 원래 플레이어 - 카메라 간의 거리
    public float m_TargetDist = 0.0f;       // 보정된 거리float m_RotSpeed = 180.0f;
    public float m_CameraDist = 0.0f;       // 보정이 끝난 플레이어 - 카메라 간의 거리

    //float collisionOffset = 1.0f;    // 가상의 구를 만들기 위한 반지름 (카메라의 부드러운 충돌용 오프셋)
    float m_RotSpeed = 180.0f;       // 회전 속도(초당 180도로 돌린다고 생각하면 될듯?)
    float m_ZoomSpeed = 300.0f;      // 줌 속도
    float m_LerpSpeed = 10.0f;       // 보간 속도

    Vector3 m_Rotation;              // 자신회전값

    Vector2 m_ZoomRange;             // 줌 제한 값
    Vector2 m_RotXRange;             // 회전 제한 값

    // Start is called before the first frame update
    void Start()
    {
        m_OriginDist = m_TargetDist = m_CameraDist = Vector3.Distance(m_RotBasePoint.position, transform.position);

        m_RotXRange = new Vector2(15, 80);              // 상하 회전값 한계치
        m_ZoomRange = new Vector2(1, 50);               // 줌 한계치

        m_Rotation = transform.rotation.eulerAngles;    // 시작시 앵글값 저장 (오일러 앵글)
    }

    // Update is called once per frame
    void Update()
    {
        // 우클릭시 회전
        if(Input.GetMouseButton(1))
        {
            m_Rotation.x -= Input.GetAxis("Mouse Y") * m_RotSpeed * Time.deltaTime;             // 상하 회전
            //m_Rotation.y += Input.GetAxis("Mouse X") * m_RotSpeed * Time.deltaTime;           // 좌우 회전

            // 회전 제한을 둠
            m_Rotation.x = Mathf.Clamp(m_Rotation.x, m_RotXRange.x, m_RotXRange.y);
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            m_TargetDist -= Input.GetAxis("Mouse ScrollWheel") * m_ZoomSpeed * Time.deltaTime;  // 마우스 휠로 확대 축소가 됨
            m_TargetDist = Mathf.Clamp(m_TargetDist, m_ZoomRange.x, m_ZoomRange.y);             // 줌 제한을 둠
            m_OriginDist = m_TargetDist;                                                        // 원래거리에 현재 거리 대입
        }

        //Ray ray = new Ray();                    // 레이 생성
        //ray.origin = m_RotBasePoint.position;   // 바라보는 타겟에서 시작해서
        //ray.direction = -transform.forward;     // 뒤 방향으로 발사
        //
        //RaycastHit hit;
        //
        //// 가상의 구의 반지름만큼 더해줘서 애매한 위치까지도 피킹이 일어나게 함
        //if (Physics.Raycast(ray, out hit, m_CameraDist + collisionOffset, m_CastLayer))
        //{
        //    // 피킹된 거리에서 반대 방향으로 구의 반지름 만큼 위치를 더해줌
        //    //transform.position = hit.point - ray.direction * collisionOffset;
        //
        //    Vector3 pos = hit.point - ray.direction * collisionOffset;      // 충돌지점 저장
        //    m_TargetDist = Vector3.Distance(pos, m_RotBasePoint.position);  // 충돌한 지점과 바라보는 물체의 거리를 구함
        //}
        //// 충돌이 되지 않았으면 보정거리를 원래 거리로 바꿔줌
        //else
        //    m_TargetDist = m_OriginDist;

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
