using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class AnswerManager : MonoBehaviour
{

    public static AnswerManager Inst;
    [SerializeField] List<Collider>  seraCorrectTiles = new List<Collider>(),papaCorrectTiles = new List<Collider>(),allTiles = new List<Collider>();
    [SerializeField] Transform tilesParent;
    [SerializeField] int tileIndex=1;
    [SerializeField] GameObject arrow;
    [SerializeField] List<Transform> lineTrans = new List<Transform>(); 
    [SerializeField]LineRenderer lineRenderer;
    Transform player, papa,curCharacter;

    [Serializable]
    public struct AnswerData
   {
        public enum  AnswerType{ tileTurn,interaction };
        public AnswerType answerType;
        public Collider tile;
        public GameObject obj;

    }

    public AnswerData[] answerData;


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
        if (tileIndex >= seraCorrectTiles.Count) return;
        TileColDisable(seraCorrectTiles[tileIndex], seraCorrectTiles[tileIndex-1]);
    }

    private void Update()
    {
        if (InGameManager.Inst.isAnswering)
        {
            LineRenderer();
            if (!InGameManager.Inst.inRelpayMode)
            {
                if (Vector3.Distance(player.position, seraCorrectTiles[tileIndex-1].transform.position) < 2f &&  tileIndex <= seraCorrectTiles.Count-1)
                {
                    player.position = new Vector3(seraCorrectTiles[tileIndex-1].transform.position.x, player.position.y, seraCorrectTiles[tileIndex-1].transform.position.z);
                    SeraTile();
                }
            }
            else
            {
                if (Vector3.Distance(papa.position, papaCorrectTiles[tileIndex - 1].transform.position) < 2f && tileIndex <= papaCorrectTiles.Count - 1)
                {
                    papa.position = new Vector3(papaCorrectTiles[tileIndex - 1].transform.position.x, papa.position.y, papaCorrectTiles[tileIndex - 1].transform.position.z);
                    PapaTile();
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
        if (tileIndex >= papaCorrectTiles.Count) return;
        TileColDisable(papaCorrectTiles[tileIndex], papaCorrectTiles[tileIndex - 1]);
        
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
