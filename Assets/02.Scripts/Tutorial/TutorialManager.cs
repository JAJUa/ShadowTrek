using Abu;
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

    [SerializeField]List<Tutorial> tutorials = new List<Tutorial>();
    [SerializeField] private Transform tileParent,tutorialParent; 
    [HideInInspector]public List<Transform> tiles = new List<Transform>();
    [SerializeField] int tutorialNumber;

    private void OnValidate()
    {
      
    }
    private void Awake()
    {
        Inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < tutorialParent.childCount; i++)
        {
            tutorials.Add(tutorialParent.GetChild(i).GetComponent<Tutorial>());
            tutorialParent.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < tileParent.childCount; i++)
        {
            tiles.Add(tileParent.GetChild((i)));
        }

        foreach (Transform tile in tiles)
        {
            tile.gameObject.SetActive(false);
        }
       
        TutorialPlay();
        
    }

    
    [Button]
    public void FinshTutorial()
    {
        tutorials[tutorialNumber].gameObject.SetActive(false);
        tutorialNumber++;
        if(tutorialNumber < tutorials.Count)
        {
            TutorialPlay();
        }
        else
        {
            TileEnable(true);
        }
        

    }
    void TutorialPlay()
    {
        ResetSetting();
      
        if (tutorialNumber < tutorials.Count)
        {
            tutorials[tutorialNumber].gameObject.SetActive(true);
           StartCoroutine( tutorials[tutorialNumber].Excute());
        }
        

    }
    void ResetSetting()
    {
        TileEnable(false);
    }

    void TileEnable(bool enable)
    {
        if (enable) Debug.Log("전체");
        foreach (Transform tile in tiles) tile.gameObject.SetActive(enable);
    }
}
