using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cancel : MonoBehaviour
{
    public void PlayBtn()
    {
        SceneManager.LoadScene("StartScene");
    }
}
