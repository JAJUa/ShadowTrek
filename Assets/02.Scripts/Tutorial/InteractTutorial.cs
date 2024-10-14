using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractTutorial : Tutorial
{
    public UnityEvent specialEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ExcuteSpecialEvent()
    {
        if (specialEvent != null)
            specialEvent.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
