using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public void PlayBtn()
    {
        Destroy(GameObject.Find("SoundManager").gameObject);
        SceneManager.LoadScene("Start Scene");
    }
}
