using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanton : illuminant
{

    // Start is called before the first frame update
    void Start()
    {
        illuminantType = IlluminantType.always;
        GetTargetTileVector(7.5f); //타일 받기
        //TargetTileLighting(true,false); //타일 빛 켜기
    }
    

    public override void AllWaysLighting()
    {
        base.AllWaysLighting();
        List<Tile> lightTiles =  TileFinding.GetTiles(targetTileVector);
        foreach (var tile in lightTiles) tile.GetLight(true);
    }



}
