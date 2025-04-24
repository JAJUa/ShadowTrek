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
        LongFadeOut();
    }



    public void NextScene()
    {
        fadeImage.enabled = true;
        var sequence = DOTween.Sequence();

        sequence.Append(fadeImage.DOFade(1, 0.5f));
        sequence.AppendCallback(() => { SceneManager.LoadScene(sceneName); });
    }
    public void NextScene(int sceneIndex)
    {
        fadeImage.enabled = true;
        var sequence = DOTween.Sequence();

        sequence.Append(fadeImage.DOFade(1, 0.5f));
        sequence.AppendCallback(() => { SceneManager.LoadScene(sceneIndex); });
    }


    public void FadeIn()
    {
        fadeImage.enabled = true;
        fadeImage.DOFade(1, 0.15f);
    }

    public void FadeOut()
    {

        fadeImage.DOFade(0, 0.15f).OnComplete(() =>
        {
            fadeImage.enabled = false;
        });
    }

    public void LongFadeOut()
    {

        fadeImage.DOFade(0, 0.3f).OnComplete(() =>
        {
            Debug.Log("fadeOut");
            fadeImage.enabled = false;
        });
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
}
