using Abu;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VInspector;

public class TutoFadeManager : MonoBehaviour
{
    [SerializeField] GameObject tutorialFade;
    [SerializeField] TutorialFadeImage tutorialFadeImage;
    [SerializeField] TextMeshProUGUI tutoText;
    // Start is called before the first frame update
    void Start()
    {
        tutorialFadeImage = tutorialFade.GetComponent<TutorialFadeImage>();
    }

    [Button]
    public void Fade()
    {
        //첫 실행시에만 실행되게
        tutorialFade.gameObject.SetActive(true);
        DOTween.To(() => tutorialFadeImage.Smoothness, x => tutorialFadeImage.Smoothness = x, 0.012f, 0.5f)
            .OnComplete(()=> { tutoText.gameObject.SetActive(true);tutoText.transform.DOScale(1, 0.5f); }); ;
    }

    [Button]
    public void FadeOut()
    {
       
        tutoText.transform.DOScale(0, 0.5f).OnComplete(() =>
        {
            tutoText.gameObject.SetActive(false);
            DOTween.To(() => tutorialFadeImage.Smoothness, x => tutorialFadeImage.Smoothness = x, 1f, 1).OnComplete(()=>tutorialFade.gameObject.SetActive(false));
        });
        //끝나고 세이브 후 다시는 실행 안되게
          
    }
}
