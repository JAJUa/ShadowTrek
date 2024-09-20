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
        public enum AnswerType { tileTurn, interaction };
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
        player = GameObject.FindGameObjectWithTag("PlayerControl").transform;
        papa = GameObject.FindGameObjectWithTag("Papa").transform;
        curCharacter = player;

    }
    // Start is called before the first frame update
    void Start()
    {
        tileIndex = 1;
       
        for(int i = 0; i < tilesParent.childCount; i++)
        {
            allTiles.Add(tilesParent.GetChild(i).GetComponent<Collider>());
        }
    }

    [Button]
    public void SeraTile()
    {
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
                if (papa_answerData[tileIndex - 1].answerType == AnswerData.AnswerType.tileTurn)
                {
                    if (Vector3.Distance(papa.position, papa_answerData[tileIndex - 1].tile.transform.position) < 2f && tileIndex <= papa_answerData.Length - 1)
                    {
                        papa.position = new Vector3(papa_answerData[tileIndex - 1].tile.transform.position.x, papa.position.y, papa_answerData[tileIndex - 1].tile.transform.position.z);
                        PapaTile();
                    }
                }
                else
                {
                    if (!isInteract)
                    {
                        isInteract = true;
                        Debug.Log(tileIndex - 1);
                        papa_answerData[tileIndex - 1].dialogue.AnswerDialogue();
                    }
                    
                   
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
        TileColDisable(papa_answerData[tileIndex].tile, papa_answerData[tileIndex - 1].tile);
        isInteract = false;
        
    }

    public void TileColDisable(Collider targetCol,Collider preCol)
    {  
        lineTrans.Clear();
        lineTrans.Add(targetCol.transform);
        lineTrans.Add(preCol.transform);
        InGameManager.Inst.isAnswering = true;
        foreach (Collider col in allTiles)
        {
            col.enabled = false;
        }
        targetCol.enabled = true;
        preCol.enabled = true;
        tileIndex++;
    }
    public void LineRenderer()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, new Vector3(curCharacter.transform.position.x, 2.7f, curCharacter.transform.position.z));
        lineRenderer.SetPosition(1, new Vector3(lineTrans[0].position.x, 2.7f, lineTrans[0].position.z));
   
    }



}
