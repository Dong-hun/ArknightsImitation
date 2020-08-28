using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 싱글톤 템플릿화 */
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour // MonoBehaviour객체만 상속받을 수 있음 (접근 한정자)
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<T>();

                if(instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).ToString();    // 스크립트 이름이 게임 오브젝트 이름이 됨
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }
}
