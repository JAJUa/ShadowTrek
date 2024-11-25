using System;
using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VInspector;

public class Tutorial : MonoBehaviour
{
    [HideInInspector] public Transform player;
    [SerializeField] List<Transform> activeTiles = new List<Transform>();
    public bool haveEvent;
    public enum Character { Daughter, Papa }
    public Character character;
    [ShowIf("haveEvent")]public UnityEvent specialEvent;

    private void Awake()
    {
        

       
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => InGameManager.Inst);
        if (player == null)
        {
            switch (character)
            {
                case Character.Daughter:
                    player = InGameManager.Inst.player.transform;
                    break;
                case Character.Papa:
                    player =  InGameManager.Inst.papa.transform;
                    break;
            }
        }
    }

    public virtual IEnumerator Excute()
    {
        if (activeTiles.Count ==0)activeTiles = TutorialManager.Inst.tiles;
        foreach(Transform tile in activeTiles) tile.gameObject.SetActive(true);
        yield return new WaitUntil(()=>InGameUIManager.Inst.titleTexting == false);
       


    }

}
