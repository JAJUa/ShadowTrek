using Abu;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GimicTutorial : Tutorial
{
    public TutorialFadeImage tutorialFade;
    [SerializeField] Image image;

    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private bool isDisposable;
    // Start is called before the first frame update
    public override IEnumerator Excute()
    {
        yield return base.Excute();
        //yield return new WaitUntil(()=>InGameUIManager.Inst.titleTexting == false);
        CloseUpTutorial.CloseUp(this,image,text);

    }

    public void FinishGimic()
    {
        CloseUpTutorial.CloseDown(tutorialFade,image,isDisposable,text);
      
    }

}
