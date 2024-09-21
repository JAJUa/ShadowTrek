using DG.Tweening;
using Febucci.UI;
using Febucci.UI.Actions;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VInspector;

public class StartBookManager : MonoBehaviour
{
    [Tab("디버깅")]
    [SerializeField] private int bookPageIndex = 0,maxPageIndex;
    [SerializeField] AutoFlip autoFlip;
    [SerializeField] Book bookScript;
    [SerializeField] Button sceneStartBtn;

    [Tab ("내용")]
    [TextArea][SerializeField] private string[] chapterSceneName;
    [TextArea][SerializeField] private string[] chapterText;
    [TextArea][SerializeField] private string[] bgmText;
    [TextArea][SerializeField] private string[] bgmInforText;
    [TextArea][SerializeField] private string[] chapterStory;

    [Tab("TypeWriterAnim")]
    [SerializeField] TypewriterByCharacter chapterSceneName_TA;
    [SerializeField] TypewriterByCharacter chapterText_TA;
    [SerializeField] TypewriterByCharacter bgmText_TA;
    [SerializeField] TypewriterByCharacter bgmInforText_TA;
    [SerializeField] TypewriterByCharacter chapterStory_TA;

    private void Start()
    {
        sceneStartBtn.enabled = true;
        SettingBook(bookPageIndex);
    }

    public void ShowTextAnimations()
    {
        sceneStartBtn.enabled = true;
        chapterSceneName_TA.ShowText(chapterSceneName[bookPageIndex].ToString());
        chapterText_TA.ShowText(chapterText[bookPageIndex].ToString());
        bgmText_TA.ShowText(bgmText[bookPageIndex].ToString());
        bgmInforText_TA.ShowText(bgmInforText[bookPageIndex].ToString());
        chapterStory_TA.ShowText(chapterStory[bookPageIndex].ToString());
    }

    public void EnterScene()
    {
        FadeInFadeOut.Inst.NextScene(2 + bookPageIndex);
    }

    public void HideText(bool isNext)
    {
        sceneStartBtn.enabled = false;
        if (isNext)
        {
            if (bookPageIndex + 1 >= maxPageIndex) return;
        }
        else if (bookPageIndex - 1 < 0) return;
      
        chapterSceneName_TA.StopShowingText();
        chapterText_TA.StopShowingText();
        bgmText_TA.StopShowingText();
        bgmInforText_TA.StopShowingText();
        chapterStory_TA.StopShowingText();

        chapterSceneName_TA.StartDisappearingText();
        chapterText_TA.StartDisappearingText();
        bgmText_TA.StartDisappearingText();
        bgmInforText_TA.StartDisappearingText();
        chapterStory_TA.StartDisappearingText();


      
        bookPageIndex = isNext ? ++bookPageIndex : --bookPageIndex;

       


        if (isNext) DOVirtual.DelayedCall(0.6f, () => autoFlip.FlipRightPage());
        else DOVirtual.DelayedCall(0.6f, () => autoFlip.FlipLeftPage());

        DOVirtual.DelayedCall(1.6f, () => ShowTextAnimations());
    }

    public void SettingBook(int bookPage)
    {
        bookPageIndex = bookPage;

        bookScript.currentPage = (bookPage + 1) * 2;
        ShowTextAnimations();
    }


}
