using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Inst;

    public Tutorial[] tutorials;
    public Transform[] tiles;
    [SerializeField] int tutorialNumber;
    [SerializeField] Image dialogueImage;
    [SerializeField] TextMeshProUGUI dialogue;

    private void Awake()
    {
        Inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform tile in tiles)
        {
            tile.gameObject.SetActive(false);
        }
       
        TutorialPlay();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [Button]
    public void FinshTutorial()
    {
        if(tutorialNumber < tutorials.Length)
        {
            if (tutorials[tutorialNumber].textExplain) tutorials[tutorialNumber].TutorialDialogueText(false);
            tutorialNumber++;
            TutorialPlay();
        }
        

    }

    public void TutorialPlay()
    {
        ResetSetting();
      
        if (tutorialNumber < tutorials.Length)
        {
            tutorials[tutorialNumber].gameObject.SetActive(true);
           StartCoroutine( tutorials[tutorialNumber].Excute());
        }
        

    }

    public void ResetSetting()
    {
        dialogue.DOFade(0, 0);
        dialogueImage.DOFade(0,0);
        foreach (Transform tile in tiles) tile.gameObject.SetActive(false);
    }
}
