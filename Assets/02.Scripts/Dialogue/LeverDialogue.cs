using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class LeverDialogue : Dialouge
{
    [SerializeField] Lever levera;
    // Start is called before the first frame update

    [Button]
    public override void Interact()
    {
        Debug.Log("인터렉트");
        levera.TurnLight();
        base.Interact();
    }
}
