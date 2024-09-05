using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Inst;
    public Image shadowGauge,modeChangeImage;
    public Sprite normalModeSprite, shadowModeSprite;
    public GameObject optionCanvas,inGameCanvas,helpBookCanvas;
    public Animator menuAnim;
    public bool openUI;



    private void Awake()
    {
        Inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
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
        InGameManager.Inst.moveBlock = true;
        helpBookCanvas.SetActive(true);

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
