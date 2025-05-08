using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  class TileManager:Singleton<TileManager>
{
    public  Dictionary<Vector2, Tile> mapTiles = new Dictionary<Vector2, Tile>();
    
    private  void Awake()
    {
        GameObject[] tileObj = GameObject.FindGameObjectsWithTag("MoveTile");
        if(tileObj.Length ==0)Debug.Log("타일 감지 못함");
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

    }

    private void Start()
    {
        HideAllTiles();
    }

    public void ResetTileCharacter()
    {
        foreach (var tile in mapTiles.Values)
        {
            tile.character = null;
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

    public void HideAllTiles()
    {
        foreach (var tile in mapTiles.Values)
        {
            tile.gameObject.SetActive(false);
        }
    }

    public void ShowAllTiles()
    {
        foreach (var tile in mapTiles.Values)
        {
            tile.gameObject.SetActive(true);
        }
    }
    
    public void ShowTiles(List<Vector3> tiles)
    {
        foreach (var pos in tiles)
        {
            Vector2 newPos = new Vector2(pos.x, pos.z);
            if (mapTiles[newPos] != null)
            {
                mapTiles[newPos].gameObject.SetActive(true);
            }
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

