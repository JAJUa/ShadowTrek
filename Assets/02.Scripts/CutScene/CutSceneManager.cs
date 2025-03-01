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
using UnityEngine.Serialization;

public class CutSceneManager : MonoBehaviour
{
    
    
    [Serializable]
    public class DialogueCharacter
    {
        public Character character;
        public CharacterRole CharacterRole;
    }


    PlayableDirector playableDirector;
    public enum CutSceneType { startCutScene, middleCutScene }
    [SerializeField] CutSceneType cutSceneType;
    [SerializeField] RectTransform up, down;
    [SerializeField] List<DialogueCharacter> dialogueCharacter;
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
    IEnumerator Start()
    {
        yield return new WaitUntil(() => InGameManager.Inst.player);

        for (int i = 0; i < dialogueCharacter.Count; i++)
        {
            
            switch (dialogueCharacter[i].CharacterRole)
            {
                case CharacterRole.Sera:
                    dialogueCharacter[i].character = InGameManager.Inst.player;
                    break;
                case CharacterRole.Papa:
                    dialogueCharacter[i].character = InGameManager.Inst.papa;
                    break;
                
            }
        }
 
        
        playableDirector = GetComponent<PlayableDirector>();
        
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

    [Button]
    public void DialogueTexting() //클릭해서 넘어가는 다이얼로그
    {
        if (dialogueIndex >= localizeName.Length)
        {
            CloseText(0);
            return;
        }
        dialogueImage[0].DOFade(1, 0.3f);
        dialogueText[0].DOFade(1, 0.5f);
        localizeStringEvent[0].StringReference = localizeName[this.dialogueIndex];
        dialogueTypeWriter[0].ShowText(localizeName[this.dialogueIndex].GetLocalizedString());
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
            for(int i = 0; i<dialogueCharacter.Count; i++)
            {
                if(!dialogueCharacter[i].character) return;
                dialogueImage[i].transform.position = dialogueCharacter[i].character.transform.position + new Vector3(0, 16, 0);
                
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
