using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class AlphaBlendControl : MonoBehaviour
{
    public Image background;
    public Image title;
    public Image fadeOut;

    public Button playButton;
    public Button optionButton;

    public TMP_Text playText;
    public TMP_Text optionText;

    private Color backgroundColor;
    private Color titleColor;
    private Color fadeOutColor;

    private Color playButtonColor;
    private Color optionButtonColor;

    private Color playTextColor;
    private Color optionTextColor;

    // Start is called before the first frame update
    void Start()
    {
        backgroundColor = background.color;
        titleColor      = title.color;
        fadeOutColor    = fadeOut.color;

        backgroundColor.a   = 0.0f;
        titleColor.a        = 0.0f;
        fadeOutColor.a      = 0.0f;

        background.color    = backgroundColor;
        title.color         = titleColor;
        fadeOut.color       = fadeOutColor;

        playButtonColor     = playButton.image.color;
        optionButtonColor   = optionButton.image.color;
        playTextColor       = playText.color;
        optionTextColor     = optionText.color;

        playButtonColor.a   = 0.0f;
        optionButtonColor.a = 0.0f;
        playTextColor.a     = 0.0f;
        optionTextColor.a   = 0.0f;

        playButton.image.color      = playButtonColor;
        optionButton.image.color    = optionButtonColor;
        playText.color              = playTextColor;
        optionText.color            = optionTextColor;

        playButton.interactable = false;
        optionButton.interactable = false;
        fadeOut.gameObject.SetActive(false);

        StartCoroutine(FadeIn());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FadeIn()
    {
        while(backgroundColor.a <= 1.0f)
        {
            backgroundColor.a += Time.deltaTime * 0.3f;
            background.color = backgroundColor;

            yield return null;
        }

        while (titleColor.a <= 1.0f)
        {
            titleColor.a += Time.deltaTime * 0.3f;
            title.color = titleColor;

            yield return null;
        }

        while (playTextColor.a <= 1.0f)
        {
            playButtonColor.a += Time.deltaTime * 0.5f;
            playTextColor.a += Time.deltaTime * 0.5f;
            playButton.image.color = playButtonColor;
            playText.color = playTextColor;

            optionButtonColor.a += Time.deltaTime * 0.5f;
            optionTextColor.a += Time.deltaTime * 0.5f;
            optionButton.image.color = optionButtonColor;
            optionText.color = optionTextColor;

            yield return null;
        }
        playButton.interactable = true;
        optionButton.interactable = true;
    }

    public void FadeOut()
    {
        fadeOut.gameObject.SetActive(true);
        StartCoroutine(OnPlayButton());
    }
    public IEnumerator OnPlayButton()
    {
        Color col = fadeOut.color;
        col.a = 0.0f;

        while (col.a <= 1.0f)
        {
            col = fadeOut.color;
            col.a += Time.deltaTime * 0.3f;
            fadeOut.color = col;

            yield return null;
        }

        SceneManager.LoadScene("Loading");
    }
}
