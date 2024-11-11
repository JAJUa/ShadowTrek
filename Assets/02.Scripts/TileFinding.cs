using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileFinding 
{
    public static  List<Tile> GetTiles(List<Vector3> lightTiles) //여러위치 타일 반환
    {
        List<Tile> returnTiles = new List<Tile>();
        foreach (var position in lightTiles)
        {
            Vector2Int pos = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
            if (TileManager.Inst.GetMapTiles().TryGetValue(pos, out var tile))
            {
                returnTiles.Add(tile);
            }
        }
        return returnTiles;
    }
    
    public static  Tile GetOneTile(Vector3 position) //특정 위치의 타일 반환
    {
        Vector2 pos = new Vector2Int((int)position.x, (int)position.z);
        if (TileManager.Inst.GetMapTiles().TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }
    
    public static List<Vector3> TargetingTiles(Transform transform,float offset) //자신 기준으로 예상 타일 범위 넣기
    {
        List<Vector3> targetTileVector = new List<Vector3>();
        targetTileVector.Add(new Vector3(transform.position.x + offset,0,transform.position.z + offset));
        targetTileVector.Add(new Vector3(transform.position.x - offset,0,transform.position.z -offset));
        targetTileVector.Add(new Vector3(transform.position.x - offset,0,transform.position.z + offset));
        targetTileVector.Add(new Vector3(transform.position.x + offset,0,transform.position.z - offset));
        targetTileVector.Add(new Vector3(transform.position.x ,0,transform.position.z + offset));
        targetTileVector.Add(new Vector3(transform.position.x ,0,transform.position.z -offset));
        targetTileVector.Add(new Vector3(transform.position.x - offset,0,transform.position.z));
        targetTileVector.Add(new Vector3(transform.position.x + offset,0,transform.position.z));

        return targetTileVector;
    }
}
