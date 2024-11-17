using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class LightShooterDialogue : Dialouge
{
    [SerializeField] private LightShooter lightShooter;
    // Start is called before the first frame update

    [Button]
    public override void Interact()
    {
        Debug.Log("인터렉트");
        lightShooter.ChangeDir();
        base.Interact();
    }
}
