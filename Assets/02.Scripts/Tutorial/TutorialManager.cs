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

    public Tutorial[] tutorials;
    [SerializeField] private Transform tileParent; 
    [HideInInspector]public List<Transform> tiles = new List<Transform>();
    [SerializeField] int tutorialNumber;


    private void Awake()
    {
        Inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
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
        tutorialNumber++;
        if(tutorialNumber < tutorials.Length)
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
      
        if (tutorialNumber < tutorials.Length)
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
        foreach (Transform tile in tiles) tile.gameObject.SetActive(enable);
    }
}
