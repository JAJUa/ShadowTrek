using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class q : MonoBehaviour
{
    public Vector3 qq;
    public bool a;

    private void Start()
    {
        if(a)
            transform.rotation = Quaternion.AngleAxis(180,Vector3.up);
        else
        {
            transform.rotation = Quaternion.AngleAxis(90,Vector3.forward) * Quaternion.AngleAxis(90,Vector3.right)*Quaternion.AngleAxis(90,Vector3.up);
        }
    }
}
