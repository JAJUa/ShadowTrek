using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : illuminant
{
    [SerializeField]private bool isLight;
    
    [SerializeField] private Material lightMaterial,offMaterial;
    private MeshRenderer renderer;

    private void Awake()
    {
        renderer = transform.GetComponent<MeshRenderer>();
    }

    void Start()
    {
        LightOff();
    }

    public override void LightOn()
    {
        illuminantType = IlluminantType.onAction;
        GetTargetTileVector(15f);
        TargetTileLighting(false,false);
        Material[] mats = renderer.materials;
        mats[2] = lightMaterial;
        renderer.materials = mats;
    }

    public override void LightOff()
    {
        base.LightOff();
        Material[] mats = renderer.materials;
        mats[2] = offMaterial;
        renderer.materials = mats;
    }

    public override void ResetLight()
    {
        TargetTileLighting(false,false);
    }

    public override void TargetTileLighting(bool isLight = true, bool action = true)
    {
        this.isLight = isLight;
        base.TargetTileLighting(isLight, action);
    
    }

    public override void AllWaysLighting()
    {
        if(isLight)TargetTileLighting(true,false);
        else TargetTileLighting(false,false);
    }
}