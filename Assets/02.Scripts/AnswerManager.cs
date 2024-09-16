using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerManager : MonoBehaviour
{
    [SerializeField] List<Collider>  seraCorrectTiles = new List<Collider>(),papaCorrectTiles = new List<Collider>(),allTiles = new List<Collider>();
    [SerializeField] Transform tilesParent;
    [SerializeField] int tileIndex;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < tilesParent.childCount; i++)
        {
            allTiles.Add(tilesParent.GetChild(i).GetComponent<Collider>());
        }
    }

    public void SeraTile()
    {
        if (tileIndex >= seraCorrectTiles.Count) return;
        TileColDisable();
        seraCorrectTiles[tileIndex].enabled= true;
        tileIndex++;
    }

    public void TileColDisable()
    {
        foreach(Collider col in allTiles)
        {
            col.enabled = false;
        }
    }

   
}
