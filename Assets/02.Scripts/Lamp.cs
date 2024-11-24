using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : illuminant
{
    [SerializeField]private bool isLight;
    protected override void Awake()
    {
        base.Awake();
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

    void Start()
    {
        
        illuminantType = IlluminantType.onAction;
        GetTargetTileVector(15f);
        TargetTileLighting(false,false);
    }

    public override void AllWaysLighting()
    {
        if(isLight)TargetTileLighting(true,false);
        else TargetTileLighting(false,false);
    }
}