using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampRotDialogue : Dialouge
{ 
    [SerializeField] TurnLight m_lamp;
    public override void Interact()
    {
        m_lamp.TurnReverse();
        base.Interact();
    }
}
