using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class AnswerManager : MonoBehaviour
{
    [SerializeField] List<Collider>  seraCorrectTiles = new List<Collider>(),papaCorrectTiles = new List<Collider>(),allTiles = new List<Collider>();
    [SerializeField] Transform tilesParent;
    [SerializeField] int tileIndex=1;
    [SerializeField] GameObject arrow;
    [SerializeField] List<Transform> lineTrans = new List<Transform>(); 
    [SerializeField]LineRenderer lineRenderer;
    Transform player, papa;

    public bool isAnswer;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PlayerControl").transform;
        papa = GameObject.FindGameObjectWithTag("Papa").transform;

    }
    // Start is called before the first frame update
    void Start()
    {
       
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
        if (isAnswer)
        {
            LineRenderer();
            if (!InGameManager.Inst.inRelpayMode)
            {
              
                if (Vector3.Distance(player.position, seraCorrectTiles[tileIndex].transform.position) < 15.2f &&  tileIndex < seraCorrectTiles.Count-1)
                {
                    player.position = new Vector3(seraCorrectTiles[tileIndex].transform.position.x, player.position.y, seraCorrectTiles[tileIndex].transform.position.z);
                    SeraTile();
                }
            }
        }
        

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
        isAnswer = true;
        foreach (Collider col in allTiles)
        {
            col.enabled = false;
        }
        targetCol.enabled = true;
        preCol.enabled = true;
        //Instantiate(arrow, targetCol.transform.position + new Vector3(0, 10, 0), Quaternion.identity);
        tileIndex++;
    }
    public void LineRenderer()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, new Vector3(player.transform.position.x, 2.7f, player.transform.position.z));
        lineRenderer.SetPosition(1, new Vector3(lineTrans[0].position.x, 2.7f, lineTrans[0].position.z));
   
    }



}
