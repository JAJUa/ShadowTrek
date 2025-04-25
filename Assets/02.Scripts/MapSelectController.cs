using System;
using DG.Tweening;
using Febucci.UI;
using Febucci.UI.Actions;
using System.Collections;
using System.Collections.Generic;
using MapDataSheet;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VInspector;

public class MapSelectController : MonoBehaviour
{
    [System.Serializable]
    public class ChapterInfo
    {
         public int id;
         public List<string> infos = new List<string>();

        public ChapterInfo( MapInfoData _data)
        {
            id = _data.id;
            infos.AddRange(new[] { _data.chapterName, _data.chapterNum,"bgm" ,_data.bgmInfo, _data.story });
        }
    }
    
    [Tab("디버깅")]
    public List<ChapterInfo> chapterInfos = new List<ChapterInfo>();
    [SerializeField] private int bookPageIndex = 0,maxPageIndex;
    [SerializeField] AutoFlip autoFlip;
    [SerializeField] Book bookScript;
    [SerializeField] Button sceneStartBtn;
    [SerializeField] Button nextBtn, prevBtn;
    [SerializeField] Image bookHiglight;

    [FormerlySerializedAs("chapterSceneName")]
    

    [Tab("TypeWriterAnim")]
    [SerializeField] List<TypewriterByCharacter> typeWriters = new List<TypewriterByCharacter>();

    private void Awake()
    {
        MapInfoData.Load();
    }

    private void Start()
    {
        foreach (var _mapInfo in MapInfoData.MapInfoDataList)
        {
            chapterInfos.Add(new ChapterInfo( _mapInfo));
        }
        sceneStartBtn.enabled = true;
        SettingBook(bookPageIndex);
        nextBtn.onClick.AddListener(() => HideText(true));
        prevBtn.onClick.AddListener(() => HideText(false));
    }

    public void ShowTextAnimations()
    {
        sceneStartBtn.enabled = true;
        bookHiglight.enabled = true;
        var _info = chapterInfos[bookPageIndex];
        for (int i = 0; i < typeWriters.Count; i++)
        {
            typeWriters[i].ShowText(_info.infos[i]);
        }

        if (bookPageIndex + 1 >= maxPageIndex) nextBtn.gameObject.SetActive(false);
        else nextBtn.gameObject.SetActive(true);
        if (bookPageIndex - 1 < 0) prevBtn.gameObject.SetActive(false);
        else prevBtn.gameObject.SetActive(true);

        prevBtn.interactable = true;
        nextBtn.interactable = true;
    }

    public void EnterScene()
    {
        Debug.Log("Enter Scene ");
        FadeInFadeOut.Inst.FadeOut(true,1,2);
    }

    public void HideText(bool isNext)
    {
        sceneStartBtn.enabled = false;
        bookHiglight.enabled = false;
        prevBtn.interactable = false;
        nextBtn.interactable = false;

        if (isNext)
        {
            if (bookPageIndex + 1 >= maxPageIndex) return;
        }
        else if (bookPageIndex - 1 < 0) return;

        foreach (var _typeWriter in typeWriters)
        {
            _typeWriter.StopShowingText();
            _typeWriter.StartDisappearingText();
        }
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
