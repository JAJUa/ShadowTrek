using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class Tutorial : MonoBehaviour
{
    [SerializeField] Transform triggerObj;
    [SerializeField] Transform[] activeTiles;
    [SerializeField] Transform player;
    public bool textExplain,moveTutoCutScene,replayPlayerPosChange;
    public enum Character {Daughter,Papa}
    public Character character;
    public enum TutorialType { MoveTutorial, textTutorial, cutSceneClickTuto, ClickObj,TutorialAnimation}
    public TutorialType tutorialType;
    [SerializeField] Animator tutorialAnimator;
    [SerializeField] int tutoIndex;
    [SerializeField] bool isTutoAnimInt;
    [SerializeField] string triggerName;
    [ShowIf("textExplain")]
    [SerializeField] TextMeshProUGUI dialogue;
    [ShowIf("textExplain")]
    [SerializeField] Image dialogueImage;
    [ShowIf("textExplain")]
    [TextArea]
    [SerializeField] string[] explainText;

    [ShowIf("textExplain")]
    [SerializeField] bool isContinuous;
    [ShowIf("isContinuous")]
    [SerializeField] Transform textTriggerObj;
    [ShowIf("isContinuous")]
    [SerializeField] int textIndex = 0;

    [ShowIf("moveTutoCutScene")]
    [SerializeField] CutSceneManager cutSceneManager;

   



    bool tutoFnish;

  

    [ShowIf("textTutorial")]
    [SerializeField] string[] tutorialText;

    private void Start()
    {
       if(player== null)
        {
            switch (character)
            {
                case Character.Daughter:
                    player = GameObject.FindGameObjectWithTag("PlayerControl").transform;
                    break;
                case Character.Papa:
                    player = GameObject.FindGameObjectWithTag("Papa").transform;
                    break;
            }
        }
     

        if (replayPlayerPosChange)
        {
            if (player.TryGetComponent(out Player playerSc))
            {
                int index = playerSc.pointInTime.Count-1;
                playerSc.pointInTime.RemoveAt(index);
                playerSc.pointInTime.RemoveAt(index-1);
            }
        }
    }

    public void Excute()
    {
        foreach(Transform tile in activeTiles) tile.gameObject.SetActive(true);
        if (triggerObj != null) triggerObj.gameObject.SetActive(true);
        if (textExplain) TutorialDialogueText(true);
        if (isTutoAnimInt)
        {
            if (tutorialType == TutorialType.TutorialAnimation) DOVirtual.DelayedCall(0.5f, () =>
            { tutorialAnimator.SetInteger(triggerName,tutoIndex); InGameManager.Inst.moveBlock = true; }); ;
        }
        else
        {
            if (tutorialType == TutorialType.TutorialAnimation) DOVirtual.DelayedCall(0.5f, () =>
            { tutorialAnimator.SetTrigger(triggerName); InGameManager.Inst.moveBlock = true; }); ;
        }
     

    }

    public void TutorialDialogueText(bool isOpen)
    {

        if (isOpen)
        {
            dialogueImage.gameObject.SetActive(true);
            dialogue.text = "";
            dialogue.text = explainText[textIndex];

        }
        float endValue = isOpen ? 1f : 0f;
        dialogueImage.DOFade(endValue,0.5f);
        dialogue.DOFade(endValue, 0.5f);
        textIndex++;
        if (!isOpen)
        {      
            DOVirtual.DelayedCall(0.5f, () =>
            {
                dialogue.text = "";
                dialogueImage.gameObject.SetActive(false);
            });
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(!tutoFnish) 
        {
            if (triggerObj != null)
            {
                if (Vector3.Distance(player.position, triggerObj.position) < 2f)
                {
                  
                    tutoFnish = true;
                    player.position = triggerObj.position;  
                    if (moveTutoCutScene)
                    {
                        InGameManager.Inst.StopMoving();

                        
                        cutSceneManager.StartCutScene();
                        
                    }
                    else
                    {
                        TutorialManager.Inst.FinshTutorial();
                    }
                    
                }
            }

        }

        if (isContinuous)
        {
            if (Vector3.Distance(player.position, textTriggerObj.position) < 1.4f)
            {
                isContinuous = false;
                TutorialDialogueText(true);
            }
        }

    }

    [Button]
    public void DebugDist()
    {
        Debug.Log(Vector3.Distance(player.position, triggerObj.position));
    }
}
