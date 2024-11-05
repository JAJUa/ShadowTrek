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
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

public class CutSceneManager : MonoBehaviour
{

    [SerializeField] PlayableDirector playableDirector;
    public enum CutSceneType { startCutScene, middleCutScene }
    [SerializeField] CutSceneType cutSceneType;
    [SerializeField] RectTransform up, down;
    [SerializeField] GameObject[] character;
    [SerializeField] bool isDialogue;

    [SerializeField] LocalizedString[] localizeName;
    [SerializeField] LocalizeStringEvent[] localizeStringEvent;

  

    [Foldout("Dialogue")]
    [Header("Dialogue Text")]
    [TextArea][SerializeField] string[] dialogueDetail;
    [SerializeField] int dialogueIndex = 0;
    [SerializeField] Image[] dialogueImage;
    [SerializeField] TypewriterByCharacter[] dialogueTypeWriter;
    [SerializeField] TextMeshProUGUI[] dialogueText;
    [SerializeField] float dialogueTextCloseCool;
     Tween[] dialogueFadeTween = new Tween[2];
     Tween[] dialogueTextTween = new Tween[2];
    Coroutine textCloseCor;


    // Start is called before the first frame update
    void Start()
    {
        if (cutSceneType == CutSceneType.startCutScene) StartCutScene();
        else playableDirector.stopped += OnTimelineStopped;
    }
    
    void OnTimelineStopped(PlayableDirector director)
    {
        Debug.Log("타임라인 재생이 끝났습니다.");
        
        TutorialManager.Inst.FinshTutorial();
        transform.gameObject.SetActive(false);
    }

    public void DialogueImage(int dialogueIndex)
    {
        if (dialogueFadeTween != null) dialogueFadeTween[dialogueIndex].Kill();
        if(dialogueTextTween != null) dialogueTextTween[dialogueIndex].Kill();
        dialogueImage[dialogueIndex].gameObject.SetActive(true);
        
        dialogueImage[dialogueIndex].DOFade(1, 0.3f);
        dialogueText[dialogueIndex].DOFade(1, 0.5f);
        localizeStringEvent[dialogueIndex].StringReference = localizeName[this.dialogueIndex];
        dialogueTypeWriter[dialogueIndex].ShowText(localizeName[this.dialogueIndex].GetLocalizedString());
     

        dialogueTextTween[dialogueIndex] =  DOVirtual.DelayedCall(dialogueTextCloseCool , () => dialogueTypeWriter[dialogueIndex].StartDisappearingText());
        dialogueFadeTween[dialogueIndex] = DOVirtual.DelayedCall(dialogueTextCloseCool+1.5f , () => {
            dialogueImage[dialogueIndex].DOFade(0, 0.5f) ;      
            });
        this.dialogueIndex++;
    }
    

    
    


    public void CloseText(int dialogueIndex)
    {
        dialogueImage[dialogueIndex].DOFade(0, 0.5f);
        dialogueText[dialogueIndex].DOFade(0, 0.3f);
    }

    private void LateUpdate()
    {
        if (isDialogue)
        {
            for(int i = 0; i<character.Length; i++)
            {
                dialogueImage[i].transform.position = character[i].transform.position + new Vector3(0, 16, 0);
                
                dialogueImage[i].transform.rotation = Camera.main.transform.rotation;
            }
         
        }

    }

    public void StopCutScene()
    {
        InGameManager.Inst.isCutsceneIn = false;
        if (up)
        {
            up.DOAnchorPosY(50, 0.7f);
            down.DOAnchorPosY(-50, 0.7f);
        }
       
        playableDirector.Stop();
      
    }

    public void StartCutScene() => playableDirector.Play();

    public void CutSceneIn(bool cutSceneIn)
    {
        if(cutSceneIn)
        {
            InGameManager.Inst.isCutsceneIn= true;
            MoveBlock(true);
            up.DOAnchorPosY(-50, 0.7f);
            down.DOAnchorPosY(50, 0.7f);
        }
        else
        {
            foreach(Image image in dialogueImage)
            {
                image.DOFade(0,0.5f);
            }

            foreach(TextMeshProUGUI text in dialogueText)
            {
                text.DOFade(0, 0.3f);
            }

            InGameManager.Inst.isCutsceneIn =false;
            MoveBlock(false);
            up.DOAnchorPosY(50, 0.7f);
            down.DOAnchorPosY(-50, 0.7f);
            
        }
    }
    public void MoveBlock(bool isBlock)
    {
        InGameManager.Inst.moveBlock = isBlock;
    }

    public void GoTitle()
    {

        FadeInFadeOut.Inst.NextScene(0);
    }

}
