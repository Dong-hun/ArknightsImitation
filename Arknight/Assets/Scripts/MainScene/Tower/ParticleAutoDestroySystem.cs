using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 파티클 자동 삭제 스크립트
public class ParticleAutoDestroySystem : MonoBehaviour
{
    // 파티클 가져올 변수
    private ParticleSystem ps;

    void Start()
    {
        // 파티클 가져옴
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // 파티클이 잇을때
        if(ps)
        {
            // 파티클이 죽었다면
            if(!ps.IsAlive())
            {
                // 없애줌
                Destroy(this.gameObject);
            }
        }
    }
}
