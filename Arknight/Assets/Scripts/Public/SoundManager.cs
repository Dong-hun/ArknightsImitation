using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    // 오디오 소스
    public AudioSource BGMSource;

    // 오디오 클립
    public AudioClip[] BGMList;

    // 배경음 슬라이더
    public Slider volum;

    // 현재 배경음량
    private float backgroundVolum = 1.0f;

    private void Awake()
    {
        // 씬이 전환되도 파괴되지 않게 설정
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        // 오디오 클립에 노래 담아줌
        BGMList = new AudioClip[2];
        BGMList[0] = Resources.Load("BGM/StartSceneBGM") as AudioClip;
        BGMList[1] = Resources.Load("BGM/MainSceneBGM") as AudioClip;

        // 오디오 소스 컴포넌트 가져오고 처음 노래는 0번클립으로 설정하고 시작
        BGMSource = GetComponent<AudioSource>();
        BGMSource.clip = BGMList[0];
        BGMSource.Play();

        // 플레이어 프리팹으로 볼륨크기 가져옴
        backgroundVolum = PlayerPrefs.GetFloat("backgroundVolum", 1f);

        // 슬라이더에 볼륨크기 저장
        volum.value = backgroundVolum;

        // 저장된 볼륨 크기만큼 볼륨을 설정
        BGMSource.volume = volum.value;
    }
    private void Update()
    {
        // 음악 설정
        SetBackgroundSound();
    }

    public void SetBackgroundSound()
    {
        // 현재 씬이 메인씬이라면
        if(SceneManager.GetActiveScene().name == "Main Scene")
        {
            // 볼륨이 없다면
            if(volum == null)
            {
                // 비활성화된 슬라이더를 찾아서 넣어줌
                volum = GameObject.Find("Panel")
                    .transform.Find("Button").
                    transform.Find("Hide").
                    transform.Find("Totalbar").GetComponent<Slider>();
                volum.value = backgroundVolum;
            }
            
        }

        // 오디오 소스의 볼륨 크기를 슬라이더 볼륨 크기만큼 설정
        BGMSource.volume = volum.value;

        // 플레이어 프리팹에 저장할 값에도 넣어준 뒤 저장
        backgroundVolum = volum.value;
        PlayerPrefs.SetFloat("backgroundVolum", backgroundVolum);
    }

    // 노래 변경
    public void PlayBackgroundSound(AudioClip clip, AudioSource audioPlayer)
    {
        // 전의 오디오 가져옴
        AudioClip prevclip = audioPlayer.clip;

        // 전의 오디오랑 바꿔틀려는 오디오가 같지 않다면
        if (prevclip.name != clip.name)
        {
            // 오디오 세팅
            audioPlayer.Stop();
            audioPlayer.clip = clip;
            audioPlayer.loop = true;
            audioPlayer.time = 0;
            audioPlayer.Play();
        }
        // 같다면 리턴
        else
            return;

    }

}
