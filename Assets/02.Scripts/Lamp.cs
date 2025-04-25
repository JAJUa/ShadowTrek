using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : illuminant
{
    [SerializeField]private bool isLight;
    

    void Start()
    {
        LightOff();
    }

    public override void LightOn()
    {
        illuminantType = IlluminantType.onAction;
        GetTargetTileVector(15f);
        TargetTileLighting(false,false);
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