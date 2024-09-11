using DG.Tweening;
using Febucci.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using VInspector;

public class InGameBookManager : MonoBehaviour
{
    [Tab("Debugging")]
    [SerializeField] private int bookPageIndex = 0;
    [SerializeField] private AutoFlip autoFlip;

    [Tab("Modify")]
    [TextArea] [SerializeField] private string[] clipNameText;
    [TextArea] [SerializeField] private string[] infoText;

    [SerializeField] private PlayerIcon[] selectIcon;
    [SerializeField] private Image playerImage;
    [SerializeField] private Image papaImage;
    [SerializeField] private Image vidImage;
    [SerializeField] private Image vidmaskImage;
    [SerializeField] private RawImage vidraw;
    [SerializeField] private VideoClip[] videos;

    [Tab("TypeWriterAnim")]
    [SerializeField] private TypewriterByCharacter clipNameText_TA;
    [SerializeField] private TypewriterByCharacter infoText_TA;
    [SerializeField] private TypewriterByCharacter manualText_TA;
    [SerializeField] private TypewriterByCharacter manualInfoText_TA;
    [SerializeField] private TypewriterByCharacter selectInfoText_TA;
    [SerializeField] private TypewriterByCharacter manualTitleText_TA;

    private enum PlayerIcon { player, papa }

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
        manualTitleText_TA.StartShowingText();
    }

    private void HideTextAnimations()
    {
        clipNameText_TA.StopShowingText();
        infoText_TA.StopShowingText();
        manualText_TA.StopShowingText();
        manualInfoText_TA.StopShowingText();
        selectInfoText_TA.StopShowingText();
        manualTitleText_TA.StopShowingText();

        clipNameText_TA.StartDisappearingText();
        infoText_TA.StartDisappearingText();
        manualText_TA.StartDisappearingText();
        manualInfoText_TA.StartDisappearingText();
        selectInfoText_TA.StartDisappearingText();
        manualTitleText_TA.StartDisappearingText();
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

        switch (selectIcon[bookPageIndex])
        {
            case PlayerIcon.papa:
                papaImage.DOFade(1f, 1f);
                break;
            case PlayerIcon.player:
                playerImage.DOFade(1f, 1f);
                break;
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
    }

    private void HandlePageFlip(bool isNext)
    {
        float delay = 1.2f;
        DOVirtual.DelayedCall(delay, () =>
        {
            if (isNext)
                autoFlip.FlipRightPage();
            else
                autoFlip.FlipLeftPage();
        });

        DOVirtual.DelayedCall(delay + 1.5f, () => NextPage());
    }
}
