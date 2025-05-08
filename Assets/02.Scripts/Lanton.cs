using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanton : illuminant
{
    private List<Tile> lightTiles = new List<Tile>();
    [SerializeField] private Material lightMaterial,offMaterial;
    private MeshRenderer renderer;

     void Awake()
    {
        renderer = transform.GetComponent<MeshRenderer>();
    }

    public override void LightOn()
    {
        illuminantType = IlluminantType.always;
        Material[] mats = renderer.materials;
        mats[1] = lightMaterial;
        renderer.materials = mats;
        GetTargetTileVector(7.5f);
        TargetTileLighting(true,false);
        lightTiles =TileFinding.GetTiles(targetTileVector);
        foreach (Tile tile in lightTiles)
        {
            tile.alWaysLighting = true;
        }
    }

    public override void LightOff()
    {
        base.LightOff();
        Material[] mats = renderer.materials;
        mats[1] = offMaterial;
        renderer.materials = mats;
    }
    


    public override void AllWaysLighting()
    {
        base.AllWaysLighting();
        List<Tile> lightTiles =  TileFinding.GetTiles(targetTileVector);
        foreach (var tile in lightTiles) tile.GetLight(true);
    }



}
