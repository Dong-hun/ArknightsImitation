using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DelVoid();
public delegate void DelAttack(GameObject enemy);
public delegate void DelDelete(GameObject obj);
public delegate IEnumerator DelCor(float timer);

public class TowerManager : MonoBehaviour
{
    /* 타워 관리 스크립트 */

    //==================== 상속해서 쓸것 (추상클래스 -> 일반클래스 전환)
    public enum STATE                // 타워 상태
    {
        IDLE, BATTLE, SKILL, DEATH               // 대기, 전투, 사망
    }
    public STATE m_State;            // 상태 받는 변수
    public float m_MaxHp;            // 최대 체력
    public float m_CurrentHp;        // 체력
    public float m_MaxMp;            // 최대 마력
    public float m_CurrentMp;        // 마력   
    public int m_TileX;              // 타워 X좌표
    public int m_TileY;              // 타워 Y좌표
    public float m_Damage;           // 공격력
    public float m_AttackDelay;      // 공격 딜레이
    public float m_AttackDist;       // 사거리

    public Animator m_Anim;             // 애니메이터 (protected로 상속중이여서 인스팩터창 링크불가능 
                                        // 이름으로 호출 or public으로 바꿔서 링크걸기)

    protected NodeManager m_NodeManager;
    protected BuildManager m_BuildManager;

    protected List<Enemy> m_EnemyList;  // 적 리스트
    public GameObject m_Target;      // 현재 타겟
    public GameObject Target
    {
        get
        {
            return m_Target;
        }
    }



    //protected DelVoid m_Attack;       // 공격 담는 딜리게이트 (딜리게이트로 한번 해볼까해서 넣어봤어여)

    // Start is called before the first frame update
    protected void Start()
    {
        // 매니저 가져오기
        m_NodeManager = GameObject.Find("NodeList").GetComponent<NodeManager>();
        m_BuildManager = GameObject.Find("BuildManager").GetComponent<BuildManager>();

        // 사망 코루틴 딜리게이트 추가
        this.GetComponentInChildren<TowerAnimationEvent>().m_Death = new DelCor(Disappear);

        // 초기화
        m_Target = null;
        m_State = STATE.IDLE;
    }

    // 타워 세팅
    protected void Init(int maxhp = 50, int maxmp = 0, int dmg = 0, float dist = 1.0f, float delay = 0.0f)
    {
        m_State = STATE.IDLE;
        m_MaxHp = maxhp;
        m_MaxMp = maxmp;
        m_CurrentHp = m_MaxHp;
        m_CurrentMp = 0;
        m_Damage = dmg;
        m_AttackDist = dist;
        m_AttackDelay = delay;
    }

    // 상태 변경시 한버 호출될 함수
    protected virtual void ChangeState(STATE s)
    {

    }

    // 프레임 마다 업데이트 할 함수
    protected virtual void StateProcess()
    {

    }

    // 공격
    protected virtual void Attack()
    {

    }

    protected virtual void Death()
    {

    }


    public void UpdateHp(float dmg)
    {
        m_CurrentHp += dmg;
    }

    // 죽으면 사라지는 코루틴
    protected IEnumerator Disappear(float dist)
    {
        // 죽은 애니메이션이 끝나고 나서 1초뒤에 실행
        yield return new WaitForSeconds(1.0f);

        // 설정 거리보다 클때까지 반복문 돌아서
        while (dist > 0.0f)
        {
            // delta에 보정치 * 스무스델타타임으로 더해줌
            float delta = 0.7f * Time.smoothDeltaTime;

            // 위에서 보정된 delta값만큼 아래로 내려감
            this.transform.Translate(Vector3.down * delta);

            // 내려간 만큼 설정 거리에서 빼줌
            dist -= delta;

            // yield return으로 돌아감
            yield return null;
        }

        // 반복문을 빠져나오면 타워리스트 안에있는 자기 자신과
        // 타워리스트 안에 있는 힐타워의 어라운드리스트안의 자기 자신을 제거해줘야됨

        // 타워 리스트의 개수 만큼 반복문 진행
        for (int i = 0; i < m_BuildManager.m_TowerList.Count; ++i)
        {
            // 해당 원소의 레이어가 "HealTower"면
            if (m_BuildManager.m_TowerList[i].layer == LayerMask.NameToLayer("HealTower"))
            {
                // 길어진 수식을 짧게하기 위해 해당 원소를 지역변수로 담아줌
                HealTower healTower = m_BuildManager.m_TowerList[i].GetComponent<HealTower>();

                // 해당 원소의 주변 타워 리스트의 개수 만큼 반복문 진행
                for (int j = 0; j < healTower.m_AroundTowerList.Count; ++j)
                {
                    // 해당 오브젝트가 검사하는 힐 타워 리스트안에 들어가 있으면
                    if (this.gameObject == healTower.m_AroundTowerList[j])
                    {
                        // 그 리스트 안의 원소 제거 후 break로 빠져나옴
                        healTower.m_AroundTowerList.Remove(healTower.m_AroundTowerList[j]);
                        break;
                    }
                }
            }

            // 길어진 수식을 짧게하기 위해 해당 원소를 지역변수로 담아줌
            GameObject removeTower = m_BuildManager.m_TowerList[i];

            // 리스트 안의 원소와 지우려는 오브젝트가 같다면
            if (removeTower == this.gameObject)
            {
                // 해당 원소를 지워주고 반복문 빠져나옴
                m_BuildManager.m_TowerList.Remove(removeTower);
                break;
            }
        }

        // 해당 타워의 타일 좌표를 가져옴
        int TileX = m_TileX;
        int TileY = m_TileY;

        // 가져온 좌표의 타일정보를 NONE으로 바꿔줌
        m_NodeManager.m_TileState[TileY, TileX] = NodeManager.TILEINFO.NONE;

        m_NodeManager.GetNode(TileX, TileY).GetComponent<MeshRenderer>().material = Resources.Load("Tower/Material/Grass") as Material;


        // 위의 작업이 끝나면 해당 오브젝트 삭제
        Destroy(this.gameObject);

        yield return null;
    }

    // 스킬 사용
    public virtual IEnumerator ActiveSkill(float timer)
    {
        yield return null;
    }
}
