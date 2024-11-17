using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : illuminant
{
    protected override void Awake()
    {
        base.Awake();
    }


    public override void ResetLight()
    {
        TargetTileLighting(false,false);
    }

    void Start()
    {
        illuminantType = IlluminantType.onAction;
        GetTargetTileVector(15f);
        TargetTileLighting(false,false);
    }
}