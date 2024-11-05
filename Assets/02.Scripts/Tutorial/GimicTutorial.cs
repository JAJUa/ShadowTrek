using Abu;
using DG.Tweening;
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
    [Tooltip("클릭할 이미지가 터치 후 사라질 것인가")]
    public bool isDisposable;
    // Start is called before the first frame update
    public override IEnumerator Excute()
    {
        yield return base.Excute();
        image.DOFade(0, 0);
        text.rectTransform.DOScale(Vector3.zero, 0);
       
        CloseUpTutorial.CloseUp(this,image,text);

    }

    public void FinishGimic()
    {
        if(specialEvent!= null)
        {
            specialEvent.Invoke();
        }
        CloseUpTutorial.CloseDown(this,image,text);
      
    }

}
