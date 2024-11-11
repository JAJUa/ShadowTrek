using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : illuminant
{
    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        GetTargetTileVector(15f);
        TargetTileLighting(false,false);
    }
}