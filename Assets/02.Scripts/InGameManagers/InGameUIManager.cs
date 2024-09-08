using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using TMPro.EditorUtilities;
using Febucci.UI;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Inst;
    public Image modeChangeImage;
    public Sprite normalModeSprite, shadowModeSprite;
    public GameObject optionCanvas,inGameCanvas,helpBookCanvas;
    public Animator menuAnim;
    [SerializeField] TypewriterByCharacter titleText;
    [SerializeField] string titleName;
    [SerializeField] bool startTitleAnim;
    public bool openUI;



    private void Awake()
    {
        Inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (startTitleAnim) ShowTitleText(0.2f);
    }

 

    // Update is called once per frame
    void Update()
    {
       
       
    }

    public void OpenOption(bool isOpen)
    {
        optionCanvas.SetActive(isOpen);
        openUI= isOpen;
    }

    public void OpenBook()
    {
        openUI = !openUI;
        InGameManager.Inst.moveBlock = openUI;
        helpBookCanvas.SetActive(openUI);

    }

    public void ShowTitleText(float delay = 0)
    {
        DOVirtual.DelayedCall(delay, () =>
        {
            titleText.ShowText(titleName);
            DOVirtual.DelayedCall(1.5f, () => titleText.StartDisappearingText());
        });
       

    }



  

    public void ModeSpriteChange(bool isNormal) 
    {
        if (isNormal)
        {
            modeChangeImage.sprite = normalModeSprite;
        }
        else
        {
            modeChangeImage.sprite = shadowModeSprite;
        }
    }

    public void MenuFade()
    {
        Debug.Log("123");
        if(menuAnim.GetBool("MenuFade") == false)
            menuAnim.SetBool("MenuFade", true);
        else
            menuAnim.SetBool("MenuFade", false);
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
}
