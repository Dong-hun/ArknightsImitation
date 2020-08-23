 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SceneLoad : MonoBehaviour
{
    public Slider progressbar;
    public Text loadtext;
    private void Start()
    {
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync("Main Scene");
        operation.allowSceneActivation = false; //  로딩이 끝나면 멈추게함
        while (!operation.isDone)  //로딩이 끝나서 is done이 true 가 될때까지 반복함
        {
            yield return null;
            if(progressbar.value < 1f) // 움직이는 발판으로 사용했던 MoveTowards를 이용해서 밸류값 증가.
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
            }
            else //value가 1과 같거나 커지면 text를 변경해줌
            {
                loadtext.text = "Press Spacebar";
            }
            if (Input.GetKeyDown(KeyCode.Space) && progressbar.value >= 1f && operation.progress >=0.9f)
            {
                operation.allowSceneActivation=true;
            }
        }
    }
    
}
