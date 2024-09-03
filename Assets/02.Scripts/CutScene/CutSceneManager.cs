using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System;
using VInspector;
using Febucci.UI;

public class CutSceneManager : MonoBehaviour
{
    public static CutSceneManager Inst;

    [SerializeField] PlayableDirector playableDirector;
    public enum CutSceneType { startCutScene, middleCutScene }
    [SerializeField] CutSceneType cutSceneType;
    [SerializeField] bool typingEffect;
    [SerializeField] RectTransform up, down;
    [SerializeField] GameObject[] character;
    [SerializeField] bool isDialogue;

    [Foldout("Center")]
    [Header("Center Text")]
    [TextArea] [SerializeField] string[] centerDetail;
    [SerializeField] TextMeshProUGUI centerText;
    [SerializeField] float centerTextCloseCool;

    [Foldout("Dialogue")]
    [Header("Dialogue Text")]
    [TextArea][SerializeField] string[] dialogueDetail;
    [SerializeField] int dialogueIndex = 0;
    [SerializeField] Image[] dialogueImage;
    [SerializeField] TypewriterByCharacter[] dialogueText;
    [SerializeField] float dialogueTextCloseCool;
    [SerializeField] Tween[] dialogueTextTween = new Tween[2];

    [Foldout("Bottom")]
    [Header ("Bottom Text")]
    [TextArea][SerializeField] string[] text;
    [SerializeField] Image bottomTextBackground;
    [SerializeField] float bottomTextCloseCool;
    [SerializeField] TextMeshProUGUI cutSceneBottomText;
    [SerializeField] float typingCool;
    [SerializeField] AudioClip[] ttsAudio;

    Coroutine textCloseCor;

    

   

    private void Awake()
    {
        Inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (cutSceneType == CutSceneType.startCutScene) StartCutScene();

    }

    public void Texting(int index)
    {
        if(textCloseCor != null) StopCoroutine(textCloseCor);


        if (typingEffect)
        {
            StartCoroutine(TypingEffect(index));
        }
        else
        {
           // dialogueTextTween.Kill();
            bottomTextBackground.DOFade(0, 0f);
            cutSceneBottomText.DOFade(0, 0f);
            bottomTextBackground.gameObject.SetActive(true);
            bottomTextBackground.DOFade(1, 0.4f);
            cutSceneBottomText.DOFade(1, 0.4f);
            cutSceneBottomText.text = text[index];
            if(index<ttsAudio.Length)
                AudioManager.Inst.DialogueAudio(ttsAudio[index]);
            textCloseCor = StartCoroutine(TextClose(bottomTextCloseCool,cutSceneBottomText));
         //   dialogueTextTween =  DOVirtual.DelayedCall(bottomTextCloseCool + 0.2f, () => bottomTextBackground.gameObject.SetActive(false));

        }
       

    }

    public void DialogueImage(int dialogueIndex)
    {
        if (dialogueTextTween != null)
        {
            dialogueTextTween[dialogueIndex].Kill();
        }
        dialogueImage[dialogueIndex].gameObject.SetActive(true);
        
        dialogueImage[dialogueIndex].DOFade(1, 0.5f);
        dialogueText[dialogueIndex].ShowText(dialogueDetail[this.dialogueIndex]);

        DOVirtual.DelayedCall(dialogueTextCloseCool + 0.2f, () => dialogueText[dialogueIndex].StartDisappearingText());
        dialogueTextTween[dialogueIndex] = DOVirtual.DelayedCall(dialogueTextCloseCool + 1f, () => {
            dialogueImage[dialogueIndex].DOFade(0, 0.5f).OnComplete(()=>dialogueImage[dialogueIndex].gameObject.SetActive(false)) ;      
            });
        this.dialogueIndex++;
    }



    public void CenterText(int index)
    {
        centerText.gameObject.SetActive(true);
        centerText.DOFade(1, 0.01f);
        centerText.text = centerDetail[index];
        StartCoroutine(TextClose(centerTextCloseCool,centerText));
        DOVirtual.DelayedCall(centerTextCloseCool + 0.2f,()=>centerText.gameObject.SetActive(false));
    }

    IEnumerator TypingEffect(int index)
    {
        cutSceneBottomText.text = string.Empty;
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < text[index].Length; i++)
        {
          
            stringBuilder.Append(text[index][i]);
            cutSceneBottomText.text = stringBuilder.ToString();
            yield return new WaitForSeconds(typingCool);
        }
    }

    IEnumerator TextClose(float cool,TextMeshProUGUI text)
    {
        yield return new WaitForSeconds(cool);
        text.DOFade(0, 0.2f).OnComplete(() => text.text = string.Empty);

     
    }

    private void LateUpdate()
    {
        if (isDialogue)
        {
            for(int i = 0; i<character.Length; i++)
            {
                dialogueImage[i].transform.position = character[i].transform.position + new Vector3(0, 15, 0);
                dialogueImage[i].transform.rotation = Camera.main.transform.rotation;
            }
         
        }

    }

    public void StartCutScene()
    {
        playableDirector.Play();
    }

    public void CutSceneIn(bool cutSceneIn)
    {
        if(cutSceneIn)
        {
            InGameManager.Inst.moveBlock = true;
            up.DOAnchorPosY(-50, 0.7f);
            down.DOAnchorPosY(50, 0.7f);
        }
        else
        {
            InGameManager.Inst.moveBlock = false;
            up.DOAnchorPosY(50, 0.7f);
            down.DOAnchorPosY(-50, 0.7f);
        }
    }

}
