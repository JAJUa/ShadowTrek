using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInFadeOut : Singleton<FadeInFadeOut>
{
   
    [SerializeField] private Image fadeImage;
    [SerializeField] private string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        fadeImage.enabled = true;
        FadeIn(true, 1);
    }

    public void FadeIn()
    {
        fadeImage.enabled = true;
        fadeImage.DOFade(1, 0.15f);
    }

    public void FadeIn(bool useDotween,float _time = 0.25f)
    {
        if(useDotween)
            fadeImage.DOFade(0, _time);
        else
        {
            Color c = fadeImage.color;
            c.a = 0;
            fadeImage.color = c;
        }
    }
    
    public void FadeOut()
    {
        fadeImage.DOFade(0, 0.15f).OnComplete(() =>
        {
            fadeImage.enabled = false;
        });
    }
    
    public void FadeOut(bool useDotween,float _time = 0.25f)
    {
        if(useDotween)
            fadeImage.DOFade(1, _time);
        else
        {
            Color c = fadeImage.color;
            c.a = 1;
            fadeImage.color = c;
        }
    }
    
    public void FadeOut(bool useDotween,float _time ,int _sceneIndex = 0)
    {
        if(useDotween)
            fadeImage.DOFade(1, _time).OnComplete(()=>SceneManager.LoadScene(_sceneIndex));
        else
        {
            Color c = fadeImage.color;
            c.a = 1;
            fadeImage.color = c;
            SceneManager.LoadScene(_sceneIndex);
        }
    }
}
