using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractTutorial : Tutorial
{
   
    [SerializeField] Transform dialougeObj;
    Dialouge dialogue;

    private void Awake()
    {
        dialogue = dialougeObj.GetComponentInChildren<Dialouge>();
    }

    public override IEnumerator Excute()
    {
        yield return base.Excute();
        dialogue.isTutorial = true;
    }


    public void ExcuteSpecialEvent()
    {
        if (specialEvent != null)
            specialEvent.Invoke();
    }

  
}
