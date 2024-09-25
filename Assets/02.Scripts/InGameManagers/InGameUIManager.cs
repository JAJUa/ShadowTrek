using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using Febucci.UI;
using VInspector;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Inst;

    [Tab("Setting")]
    public GameObject seraSprite, papaSprite;
    public GameObject helpBookCanvas,cutSceneSpeed;
    public Button stayBtn;
    public bool openUI, ableMenuBtn;

    [Tab("Menu")]
    public Animator menuAnim;
    public Animator optionAnim;
    [SerializeField] GameObject[] OptionChoiceText;
    private int OptionChoiceNum;

    [Tab("Title")]
    [SerializeField] TypewriterByCharacter titleText;
    [SerializeField] Image titleTextBox;
    [SerializeField] string titleName;
    [SerializeField] bool startTitleAnim;
    public bool titleTexting;



    private void Awake()
    {
        Inst = this;
        ableMenuBtn = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (startTitleAnim)
        {
            ShowTitleText(0.2f);
        }
        stayBtn.onClick.AddListener(OnStayBtn);
    }

    public void OpenOption(bool isOpen)
    {
        openUI= isOpen;
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
            titleText.ShowText(titleName);
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

    public void StayBtnActive(bool isSera)
    {
        if (stayBtn != null)
            stayBtn.gameObject.SetActive(!isSera);
    }

    public void OnStayBtn()
    {
        stayBtn.interactable = false;
        stayBtn.gameObject.GetComponent<Image>().DOFade(0.2f, 0);
        InGameManager.Inst.OnlyPlayerReplay(true,false);

        DOVirtual.DelayedCall(0.35f, () =>
        {
            stayBtn.gameObject.GetComponent<Image>().DOFade(1f, 0.5f).OnComplete(() =>
            {
                stayBtn.interactable = true;
            });
        });
    }

    public void MenuFade()
    {
        if (ableMenuBtn)
        {
            if (menuAnim.GetBool("MenuFade") == false)
                menuAnim.SetBool("MenuFade", true);
            else
                menuAnim.SetBool("MenuFade", false);

            if (openUI == true) { helpBookCanvas.SetActive(false); openUI = false; InGameManager.Inst.moveBlock = false; }
        }
        
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
        optionAnim.SetBool("OptionFade", fade);
        if (!fade) SaveSystem.Inst.SaveData();
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
