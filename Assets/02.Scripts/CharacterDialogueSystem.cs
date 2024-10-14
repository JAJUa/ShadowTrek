using DG.Tweening;
using Febucci.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;
using UnityEngine.UI;
using VInspector.Libs;
using VInspector;

public class CharacterDialogueSystem : MonoBehaviour
{
    [Serializable]
    public struct DialogueData
    {
        public LocalizedString[] localizeName;
        public int[] characterIndex; //어떤 캐릭터가 대화를 하는지 인덱스


    }
    [SerializeField] DialogueData[] dialogueData;

    private bool isDialogueIn;


    Coroutine textCloseCor;
    [SerializeField] Canvas[] dialogueCanvas;
    [SerializeField] GameObject[] character;
     List<TypewriterByCharacter> m_dialogueTypeWriter = new List<TypewriterByCharacter>();
     List<LocalizeStringEvent> m_localizeStringEvent = new List<LocalizeStringEvent>(); // 로컬라이즈가 들어가 있는 텍스트메쉬프로
     List<Image> m_dialogueImage = new List<Image>();
     List<TextMeshProUGUI> m_dialogueText = new List<TextMeshProUGUI>();
   
    [SerializeField] int dialogueIndex = 0;  //현재 몇번째 대화인지
    [SerializeField] int entireIndex = 0; // 한 대화 전체 인덱스
    Tween[] dialogueFadeTween = new Tween[2];
    Tween[] dialogueTextTween = new Tween[2];

    private void Awake()
    {
        foreach(Canvas canvas in dialogueCanvas)
        {
            Image image = canvas.transform.GetChild(0).GetComponentInChildren<Image>();
            m_dialogueImage.Add(image);
            m_dialogueTypeWriter.Add(image.GetComponentInChildren<TypewriterByCharacter>());
            m_localizeStringEvent.Add(image.GetComponentInChildren<LocalizeStringEvent>());
            m_dialogueText.Add( image.GetComponentInChildren<TextMeshProUGUI>());
            //image.gameObject.SetActive(false);
        }
    }

    [Button]
    public void StartDialogue()
    {
       
        isDialogueIn = true;
        int index = dialogueData[entireIndex].characterIndex[dialogueIndex];
     
        m_dialogueImage[index].DOFade(1, 0.3f);
        m_dialogueText[index].DOFade(1, 0.5f);
        
     
        m_localizeStringEvent[dialogueData[entireIndex].characterIndex[dialogueIndex]].StringReference = dialogueData[entireIndex].localizeName[dialogueIndex];
        m_dialogueTypeWriter[index].ShowText(dialogueData[entireIndex].localizeName[dialogueIndex].GetLocalizedString());
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && isDialogueIn)
        {
            NextDialogue();
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < character.Length; i++)
        {
            m_dialogueImage[i].transform.position = character[i].transform.position + new Vector3(0, 16, 0);

            m_dialogueImage[i].transform.rotation = Camera.main.transform.rotation;
        }
    }

    public void NextDialogue()
    {
        ++dialogueIndex;
        if (dialogueIndex >= dialogueData[entireIndex].localizeName.Length) { DialogueFinish(); return; }
        if (dialogueData[entireIndex].characterIndex[dialogueIndex] != dialogueData[entireIndex].characterIndex[dialogueIndex - 1]) //현재 텍스트 캐릭터와 다음 텍스트 캐릭터가 다를 때
        { CloseDialogue(); DOVirtual.DelayedCall(0.1f, () => StartDialogue()); }
        else StartDialogue();
    }

    void CloseDialogue()
    {
        Debug.Log("CLoseDialogue");
        foreach (Image image in m_dialogueImage) image.DOFade(0, 0.3f);
        foreach (TextMeshProUGUI text in m_dialogueText) text.DOFade(0, 0.1f);
    }
    public void DialogueFinish()
    {
        isDialogueIn = false;
        CloseDialogue();
        dialogueIndex = 0;
        entireIndex++;

    }




}
