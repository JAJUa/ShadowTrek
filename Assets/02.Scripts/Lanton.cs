using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanton : illuminant
{

    // Start is called before the first frame update
    void Start()
    {
        GetTargetTileVector(7.5f);
        TargetTileLighting(true,false);
    }



}
