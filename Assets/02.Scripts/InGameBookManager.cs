using DG.Tweening;
using Febucci.UI;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using VInspector;

public class InGameBookManager : MonoBehaviour
{
    [Tab("Debugging")]
    [SerializeField] private int bookPageIndex = 0;
    [SerializeField] private int bookPageMaxIndex = 1;
    [SerializeField] private AutoFlip autoFlip;
    [SerializeField] private Book bookScript;

    [Tab("Modify")]
    [TextArea] [SerializeField] private string[] clipNameText;
    [TextArea] [SerializeField] private string[] infoText;

    [SerializeField] private PlayerIcon[] selectIcon;
    [SerializeField] private Image playerImage;
    [SerializeField] private Image papaImage;
    [SerializeField] private Image vidImage;
    [SerializeField] private Image vidmaskImage;
    [SerializeField] private RawImage vidraw;
    [SerializeField] private Image nextBtn, preBtn;
    [SerializeField] private VideoClip[] videos;

    [Tab("TypeWriterAnim")]
    [SerializeField] private TypewriterByCharacter clipNameText_TA;
    [SerializeField] private TypewriterByCharacter infoText_TA;
    [SerializeField] private TypewriterByCharacter manualText_TA;
    [SerializeField] private TypewriterByCharacter manualInfoText_TA;
    [SerializeField] private TypewriterByCharacter selectInfoText_TA;

    [Flags] private enum PlayerIcon
    {
        None = 0,
        papa = 1 << 0,
        player = 1 << 1,
    }

    private void Start()
    {
        SettingBook(bookPageIndex);
    }

    public void NextPage()
    {
        // Start text animation
        ShowTextAnimations();

        // Change video player
        UpdateVideoPlayer();

        // fade in animation
        StartFadeInAnimations();
    }

    public void CloseText(bool isNext)
    {
        if (bookPageMaxIndex <= bookPageIndex && isNext) return;
        if (bookPageIndex <= 0 && !isNext) return;

        // Stop text animation
        HideTextAnimations();

        // fade out animation
        StartFadeOutAnimations();

        // Update page index
        UpdatePageIndex(isNext);

        // Page Delay
        HandlePageFlip(isNext);
    }

    private void ShowTextAnimations()
    {
        clipNameText_TA.ShowText(clipNameText[bookPageIndex]);
        infoText_TA.ShowText(infoText[bookPageIndex]);
        manualText_TA.StartShowingText();
        manualInfoText_TA.StartShowingText();
        selectInfoText_TA.StartShowingText();
    }

    private void HideTextAnimations()
    {
        clipNameText_TA.StopShowingText();
        infoText_TA.StopShowingText();
        manualText_TA.StopShowingText();
        manualInfoText_TA.StopShowingText();
        selectInfoText_TA.StopShowingText();

        clipNameText_TA.StartDisappearingText();
        infoText_TA.StartDisappearingText();
        manualText_TA.StartDisappearingText();
        manualInfoText_TA.StartDisappearingText();
        selectInfoText_TA.StartDisappearingText();
    }

    private void UpdateVideoPlayer()
    {
        vidraw.GetComponent<VideoPlayer>().clip = videos[bookPageIndex];
    }

    private void StartFadeInAnimations()
    {
        vidImage.DOFade(1f, 1f);
        vidmaskImage.DOFade(1f, 1f);
        vidraw.DOColor(new Color(vidraw.color.r, vidraw.color.g, vidraw.color.b, 1f), 1f);

        StartFadeIcon();
    }

    private void StartFadeIcon()
    {
        PlayerIcon currentIcons = selectIcon[bookPageIndex];

        bool hasPapa = (currentIcons & PlayerIcon.papa) == PlayerIcon.papa;
        bool hasPlayer = (currentIcons & PlayerIcon.player) == PlayerIcon.player;

        if (hasPapa && hasPlayer)
        {
            papaImage.DOFade(1f, 1f);
            playerImage.DOFade(1f, 1f);
        }
        else if (hasPapa)
        {
            papaImage.DOFade(1f, 1f);
            playerImage.DOFade(0.4f, 1f);
        }
        else if (hasPlayer)
        {
            papaImage.DOFade(0.4f, 1f);
            playerImage.DOFade(1f, 1f);
        }
    }

    private void StartFadeOutAnimations()
    {
        vidImage.DOFade(0f, 0.8f);
        vidmaskImage.DOFade(0f, 0.8f);
        vidraw.DOColor(new Color(vidraw.color.r, vidraw.color.g, vidraw.color.b, 0f), 0.8f);
        playerImage.DOFade(0f, 0.8f);
        papaImage.DOFade(0f, 0.8f);
    }

    private void UpdatePageIndex(bool isNext)
    {
        bookPageIndex = isNext
            ? Mathf.Min(bookPageIndex + 1, clipNameText.Length - 1)
            : Mathf.Max(bookPageIndex - 1, 0);

        //책 넘기는 버튼 인덱스 넘어가면 사라지게
        float nextFade = bookPageIndex < bookPageMaxIndex ? 1f : 0f;
        float preFade = bookPageIndex >0? 1f : 0f;
        //  nextBtn.DOFade(nextFade, 0.5f);
        //  preBtn.DOFade(preFade, 0.5f);
        nextBtn.enabled = bookPageIndex < bookPageMaxIndex;
        preBtn.enabled = bookPageIndex > 0;
    }

    private void HandlePageFlip(bool isNext)
    {
        float delay = .8f;
        DOVirtual.DelayedCall(delay, () =>
        {
            if (isNext)
                autoFlip.FlipRightPage();
            else
                autoFlip.FlipLeftPage();
        });

        DOVirtual.DelayedCall(delay + 1f, () => NextPage());
    }

    public void SettingBook(int bookPage)
    {
        bookPageIndex = bookPage;

        bookScript.currentPage = (bookPage + 1) * 2;
        StartFadeIcon();
        ShowTextAnimations();
    }
}
