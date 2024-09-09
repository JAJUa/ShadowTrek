using DG.Tweening;
using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class TutorialBook : MonoBehaviour
{
    [SerializeField] private int bookPageIndex = 0;
    [SerializeField] AutoFlip autoFlip;
    [Tab ("TypeWriter")]
    [SerializeField] TypewriterByCharacter clipNameTxt,clipInforTxt;
    [Tab("내용")]
    [SerializeField][TextArea] string[] clipName, usedInfor;

    public void NextPage()
    {
        clipNameTxt.ShowText(clipName[bookPageIndex].ToString());
        clipInforTxt.ShowText(usedInfor[bookPageIndex].ToString());
    }

    public void CloseText(bool isRight)
    {
        if (isRight)
        {
            if (bookPageIndex + 1 >= clipName.Length) return;
        }
        else
        {
            if (bookPageIndex - 1 < 0) return;
        }
        clipInforTxt.StopShowingText();
        clipNameTxt.StopShowingText();

        clipNameTxt.StartDisappearingText();
        clipInforTxt.StartDisappearingText();

        bookPageIndex = isRight ? ++bookPageIndex : --bookPageIndex;

        if (isRight) DOVirtual.DelayedCall(0.8f, () => autoFlip.FlipRightPage());
        else DOVirtual.DelayedCall(0.8f, () => autoFlip.FlipLeftPage());

        DOVirtual.DelayedCall(1.6f, () => NextPage());

    }

}
