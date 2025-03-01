using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using Febucci.UI;
using VInspector;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Inst;

    [Tab("Setting")]
    public GameObject seraSprite, papaSprite;
    public GameObject helpBookCanvas, cutSceneSpeed, menuBtn;
    public Button stayBtn;
    public bool openUI, ableMenuBtn;
    [SerializeField] Color stayBtnDefault_Color, stayBtnAns_Color;

    [Tab("Menu")]
    public Animator menuAnim;
    public Animator optionAnim;
    [SerializeField] GameObject[] OptionChoiceText;
    private int OptionChoiceNum;

    [Tab("Title")]
    [SerializeField] TypewriterByCharacter titleText;
    [SerializeField] Image titleTextBox;
    [SerializeField] string titleName;
    [SerializeField] LocalizedString title_LS;
    [SerializeField] LocalizeStringEvent title_SE;
    [SerializeField] bool startTitleAnim;
    public bool titleTexting;



    private void Awake()
    {
        //Dont Create 2 GameManager
        if (Inst != null && Inst != this)
        {
            Destroy(InGameUIManager.Inst);
            return;
        }
        else
        {
            Inst = this;
        }
        ableMenuBtn = true;
        if (stayBtn != null)
            stayBtnDefault_Color = stayBtn.GetComponent<Image>().color;
        titleTexting = true;
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitUntil(()=>InGameManager.Inst);
        if (startTitleAnim)
        {
            ShowTitleText(0.2f);
        }
        stayBtn.onClick.AddListener(OnStayBtn);
    }

    public void OpenOption(bool isOpen)
    {
        openUI = isOpen;
    }

    public void OpenBook()
    {
        openUI = !openUI;
        InGameManager.Inst.moveBlock = openUI;
        helpBookCanvas.SetActive(openUI);
        if (openUI == false) MenuFade();
    }

    public void CutSceneSpeedUp(bool active)
    {
        cutSceneSpeed.SetActive(active);
    }

    public void ShowTitleText(float delay = 0)
    {
        titleTexting = true;
        InGameManager.Inst.moveBlock = true;
        titleTextBox.DOFade(0.5f, 0.5f);
        DOVirtual.DelayedCall(delay, () =>
        {
            title_SE.StringReference = title_LS;
            titleText.ShowText(title_LS.GetLocalizedString());
            DOVirtual.DelayedCall(2f, () => {
                titleText.StartDisappearingText();
                DOVirtual.DelayedCall(1f, () => { titleText.GetComponent<TextMeshProUGUI>().enabled = false;
                    titleTextBox.DOFade(0, 0.5f); InGameManager.Inst.moveBlock = false; titleTexting = false; });
            });
        });


    }


    public void SpriteChange(bool isSera)
    {
        seraSprite.SetActive(isSera);
        papaSprite.SetActive(!isSera);
    }

    public void StayBtnActive(bool active)
    {
        if (stayBtn != null)
        {
            stayBtn.GetComponent<Image>().color = InGameManager.Inst.isAnswering ? stayBtnAns_Color : stayBtnDefault_Color;
            stayBtn.interactable = active;
            float alpha = active ? 1 : 0;
            stayBtn.gameObject.GetComponent<Image>().DOFade(alpha, 0.3f).OnComplete(() => stayBtn.gameObject.SetActive(active));
        }

    }

    public void OnStayBtn()
    {
        stayBtn.interactable = false;
        stayBtn.gameObject.GetComponent<Image>().DOFade(0.2f, 0);
        InGameManager.Inst.OnlyPlayerReplay(true, false);

        if (!InGameManager.Inst.isAnswering)
        {
            DOVirtual.DelayedCall(0.35f, () =>
            {
                stayBtn.gameObject.GetComponent<Image>().DOFade(1f, 0.5f).OnComplete(() =>
                {
                    stayBtn.interactable = true;
                });
            });
        }

    }

    public void MenuFade()
    {
        if (ableMenuBtn && !titleTexting)
        {
            if (menuAnim.GetBool("MenuFade") == false)
                menuAnim.SetBool("MenuFade", true);
            else
                menuAnim.SetBool("MenuFade", false);

            if (openUI == true) { helpBookCanvas.SetActive(false); openUI = false; InGameManager.Inst.moveBlock = false; }
        }

    }

    public void MenuBtnActive(bool active)
    {
     
        menuBtn.SetActive(active);
        Debug.Log(active);
    }

    public void Hint()
    {
        InGameManager.Inst.HintReset();
    }

    public void AbleMenuBtn(bool able)
    {
        ableMenuBtn = able;
    }

    public void Restart()
    {
        FadeInFadeOut.Inst.FadeIn();

        DOVirtual.DelayedCall(0.5f, () =>
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        });
    }

    public void OptionFade(bool fade)
    {
        if (fade) InGameManager.Inst.moveBlock = true;
        optionAnim.SetBool("OptionFade", fade);
        if (!fade)
        {
            InGameManager.Inst.moveBlock = false;
        }
    }

    public void OptionChoiceFade(bool fade)
    {
        optionAnim.SetBool("OptionChoice", fade);
    }

    public void OptionChoiceBtn(int value)
    {
        switch (value)
        {
            case 1:
                OptionChoiceText[1].SetActive(false);
                OptionChoiceText[0].SetActive(true);
                OptionChoiceNum = 1;
                break;
            case 2:
                OptionChoiceText[0].SetActive(false);
                OptionChoiceText[1].SetActive(true);
                OptionChoiceNum = 2;
                break;
            case 3:
                if (OptionChoiceNum == 1)
                {
                    FadeInFadeOut.Inst.FadeIn();

                    DOVirtual.DelayedCall(1f, () =>
                    {
                    #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
                    #else
                            Application.Quit();
                    #endif
                    });
                }
                else if (OptionChoiceNum == 2)
                {
                    FadeInFadeOut.Inst.NextScene(1);
                }
                break;
            default:
                break;
        }
    }
}
