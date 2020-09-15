using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicsource;
    public AudioSource btnsource;

    public Slider volum;
    private float backgroundVolum = 1.0f;

    private void Start()
    {
        backgroundVolum = PlayerPrefs.GetFloat("backgroundVolum", 1f);
        volum.value = backgroundVolum;
        musicsource.volume = volum.value;
    }
    private void Update()
    {
        SetBackgroundSound();
    }

    public void SetBackgroundSound()
    {
        musicsource.volume = volum.value;

        backgroundVolum = volum.value;
        PlayerPrefs.SetFloat("backgroundVolum", backgroundVolum);
    }
    //public void SetMusicVolume(float volume)
    //{
    //    musicsource.volume = volume;                   
    //}
    //
    //public void SetButtonVolume(float volume)
    //{
    //    btnsource.volume = volume;
    //}
    //
    //public void OnSfx()
    //{
    //    btnsource.Play(); 
    //}
}
