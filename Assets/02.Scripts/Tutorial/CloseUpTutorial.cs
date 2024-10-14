using Abu;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public static class CloseUpTutorial
{
    public static void CloseUp(GimicTutorial gimicTutorial, Image image,TextMeshProUGUI text = null)
    {
        image.gameObject.SetActive(true);
        image.GetComponent<TutorialHighlight>().enabled = true;
        image.GetComponent<TutorialButtonInteract>().TutorialStart(gimicTutorial);
        gimicTutorial.tutorialFade.gameObject.SetActive(true);
        DOTween.To(() => gimicTutorial.tutorialFade.Smoothness, x => gimicTutorial.tutorialFade.Smoothness = x, 0.025f, 1f)
            .OnComplete(() => { 
                if(text)
                {
                    text.gameObject.SetActive(true);
                    text.rectTransform.DOScale(Vector3.one, 0.5f);
                }
            });
    }

    public static void CloseDown(TutorialFadeImage tutorialFade, Image image,bool isDisposable = false,TextMeshProUGUI text = null)
    {
        if (text != null) { text.rectTransform.DOScale(Vector3.zero, 0.25f).OnComplete(()=>text.gameObject.SetActive(false)); }
        DOTween.To(() => tutorialFade.Smoothness, x => tutorialFade.Smoothness = x, 1f, 1f)
            .OnComplete(() =>
            {
                image.gameObject.SetActive(!isDisposable);
                tutorialFade.gameObject.SetActive(false); 
                image.GetComponent<TutorialHighlight>().enabled = false;   
                TutorialManager.Inst.FinshTutorial();
            });
       
    }
}
