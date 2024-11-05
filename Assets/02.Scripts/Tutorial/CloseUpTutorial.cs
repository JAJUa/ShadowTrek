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
        InGameManager.Inst.moveBlock = true;
  
        image.gameObject.SetActive(true);
        image.GetComponent<TutorialHighlight>().enabled = true;
       
        gimicTutorial.tutorialFade.gameObject.SetActive(true);
        DOTween.To(() => gimicTutorial.tutorialFade.Smoothness, x => gimicTutorial.tutorialFade.Smoothness = x, 0.025f, 0.6f)
            .OnComplete(() => {
                image.GetComponent<TutorialButtonInteract>().TutorialStart(gimicTutorial);
                if (text)
                {
                   
                    text.gameObject.SetActive(true);
                    text.rectTransform.DOScale(Vector3.one, 0.5f);
                }
            });
    }

    public static void CloseDown(GimicTutorial gimicTutorial, Image image,TextMeshProUGUI text = null)
    {
        if (text != null) { text.rectTransform.DOScale(Vector3.zero, 0.25f).OnComplete(()=>text.gameObject.SetActive(false)); }
        DOTween.To(() => gimicTutorial.tutorialFade.Smoothness, x => gimicTutorial.tutorialFade.Smoothness = x, 1f, 0.6f)
            .OnComplete(() =>
            {
                
                gimicTutorial.tutorialFade.gameObject.SetActive(false); 
                image.gameObject.SetActive(!gimicTutorial.isDisposable);
                image.GetComponent<TutorialHighlight>().enabled = false;   
                TutorialManager.Inst.FinshTutorial();
                InGameManager.Inst.moveBlock = false;
            });
       
    }
}
