using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class TutorialBook : MonoBehaviour
{
    [SerializeField] private int bookPageIndex = 0;
    [Tab ("TypeWriter")]
    [SerializeField] TypewriterByCharacter clipNameTxt,clipInforTxt;
    [Tab("³»¿ë")]
    [SerializeField] string[] clipName, usedInfor;

    public void NextPage()
    {
        clipNameTxt.ShowText(clipName[bookPageIndex]);
        clipInforTxt.ShowText(usedInfor[bookPageIndex]);
    }

}
