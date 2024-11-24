using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class AnswerManager : MonoBehaviour
{

    public static AnswerManager Inst;
    [SerializeField] List<Collider>  allTiles = new List<Collider>();
    [SerializeField] Transform tilesParent;
    [SerializeField] int tileIndex=1;
    [SerializeField] GameObject arrow;
    [SerializeField] List<Transform> lineTrans = new List<Transform>(); 
    [SerializeField]LineRenderer lineRenderer;
    [SerializeField]Transform player, papa,curCharacter;

    [SerializeField ]bool isInteract = false;

    [Serializable]
    public struct AnswerData
    {
        public enum AnswerType { tileTurn, interaction,stay };
        public AnswerType answerType;
        public Collider tile;
        public Dialouge dialogue;

    }

    [Foldout("SeraAnswerData")]
    public AnswerData[] sera_answerData;
    [Foldout("PapaAnswerData")]
    public AnswerData[] papa_answerData;


    private void Awake()
    {
        Inst = this;

   

    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitUntil(()=>InGameManager.Inst.player || InGameManager.Inst.papa);
        
        player = InGameManager.Inst.player.transform;
        papa = InGameManager.Inst.papa.transform;
        curCharacter = player;
        tileIndex = 1;
       
        for(int i = 0; i < tilesParent.childCount; i++)
        {
            Collider col = tilesParent.GetChild(i).GetComponentInChildren<Collider>();
            allTiles.Add(col);
        }
    }

    [Button]
    public void SeraTile()
    {
        VolumeManager.Inst.ChangeGlobalVolume(true);
        if (tileIndex >= sera_answerData.Length) return;
        TileColDisable(sera_answerData[tileIndex].tile, sera_answerData[tileIndex-1].tile);
    }

    private void Update()
    {
        if (InGameManager.Inst.isAnswering)
        {
            LineRenderer();
            if (!InGameManager.Inst.inRelpayMode)
            {
                if (sera_answerData[tileIndex-1].answerType == AnswerData.AnswerType.tileTurn)
                {
                    if (Vector3.Distance(player.position, sera_answerData[tileIndex - 1].tile.transform.position) < 2f && tileIndex <= sera_answerData.Length - 1)
                    {
                        player.position = new Vector3(sera_answerData[tileIndex - 1].tile.transform.position.x, player.position.y, sera_answerData[tileIndex - 1].tile.transform.position.z);
                        SeraTile();
                    }
                }
              
            }
            else
            {
                switch (papa_answerData[tileIndex - 1].answerType)
                {
                    case AnswerData.AnswerType.tileTurn:
                        if (Vector3.Distance(papa.position, papa_answerData[tileIndex - 1].tile.transform.position) < 2f && tileIndex <= papa_answerData.Length - 1)
                        {
                            papa.position = new Vector3(papa_answerData[tileIndex - 1].tile.transform.position.x, papa.position.y, papa_answerData[tileIndex - 1].tile.transform.position.z);
                            PapaTile();
                        }
                        break;
                    case AnswerData.AnswerType.interaction:
                        if (!isInteract)
                        {
                            isInteract = true;
                            Debug.Log(tileIndex - 1);
                            papa_answerData[tileIndex - 1].dialogue.AnswerDialogue();
                        }
                        break;
                    case AnswerData.AnswerType.stay:
                        if (!isInteract)
                        {
                            isInteract = true;
                            InGameUIManager.Inst.StayBtnActive(true);
                        }
                        break;
                }

               
              
            }
        }
        

    }

    public void ChangeChracter(bool isSera)
    {
        curCharacter = isSera ? player : papa;
        tileIndex= 1;
    }
    public void PapaTile()
    {
        if (tileIndex >= papa_answerData.Length) return;
        isInteract = false;
        TileColDisable(papa_answerData[tileIndex].tile, papa_answerData[tileIndex - 1].tile);
     
        
    }

    public void TileColDisable(Collider targetCol,Collider preCol)
    {  
        lineTrans.Clear();
        lineTrans.Add(targetCol.transform);
        lineTrans.Add(preCol.transform);
        InGameManager.Inst.isAnswering = true;
        
        foreach (Collider col in allTiles)
        {
            col.tag = "Untagged";
        }
        targetCol.tag ="MoveTile";
        preCol.tag = "MoveTile";
        Debug.Log("두번");
        tileIndex++;
    }
    public void LineRenderer()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, new Vector3(curCharacter.transform.position.x, 2.7f, curCharacter.transform.position.z));
        lineRenderer.SetPosition(1, new Vector3(lineTrans[0].position.x, 2.7f, lineTrans[0].position.z));
   
    }



}
