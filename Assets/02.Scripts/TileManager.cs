using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  class TileManager:MonoBehaviour
{
    public static TileManager Inst;
    private  Dictionary<Vector2, Tile> mapTiles = new Dictionary<Vector2, Tile>();
    
    private  void Awake()
    {
        Inst = this;
        GameObject[] tileObj = GameObject.FindGameObjectsWithTag("MoveTile");
        foreach(var tile in tileObj)
        {
            if (tile.TryGetComponent(out Tile tileCs))
            {
                Transform tileTrans = tile.transform;
                mapTiles.Add(new Vector2((int)tileTrans.position.x,(int)tileTrans.position.z),tileCs);
            }
        }
    
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
            tile.SetLight();
        }
    }

}

