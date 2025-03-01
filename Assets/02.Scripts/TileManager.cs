using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  class TileManager:MonoBehaviour
{
    public static TileManager Inst;
    public  Dictionary<Vector2, Tile> mapTiles = new Dictionary<Vector2, Tile>();
    
    private  void Awake()
    {
        GameObject[] tileObj = GameObject.FindGameObjectsWithTag("MoveTile");
        if(tileObj.Length ==0)Debug.Log("타일 감지 못함");
        Debug.Log(tileObj.Length);
        foreach(var tile in tileObj)
        {
            if (tile.TryGetComponent(out Tile tileCs))
            {
                Transform tileTrans = tile.transform;
                Vector2 targetVector = new Vector2((int)tileTrans.position.x, (int)tileTrans.position.z);
             //   Debug.Log(targetVector);
                mapTiles.Add(targetVector,tileCs);
            }
        }
        if(Inst != null && Inst!=this)
            Debug.Log("타일매니저 있음");
        
        Inst = this;

    }



  

    public Dictionary<Vector2, Tile> GetMapTiles()=> new Dictionary<Vector2, Tile>(mapTiles);

    public void SetLightsTile() //타일 빛 적용
    {
        foreach (var tile in mapTiles.Values)
        {
            tile.SetLight();
        }
    }

    public void LightOffAllTiles()
    {
        foreach (var tile in mapTiles.Values)
        {
            tile.GetLight(false);
            tile.character = null;
            tile.SetLight();
        }
    }

}

