using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanton : illuminant
{


    public override void LightOn()
    {
        illuminantType = IlluminantType.always;
        GetTargetTileVector(7.5f);
        TargetTileLighting(true,false);
    }


    public override void AllWaysLighting()
    {
        base.AllWaysLighting();
        List<Tile> lightTiles =  TileFinding.GetTiles(targetTileVector);
        foreach (var tile in lightTiles) tile.GetLight(true);
    }



}
