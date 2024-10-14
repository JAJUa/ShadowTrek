using System.Collections;
using System.Collections.Generic;
using Abu;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialButtonInteract : MonoBehaviour,IPointerClickHandler
{
    private bool tutorialClick;
    private GimicTutorial tutorial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TutorialStart(GimicTutorial tutorial)
    {
        tutorialClick = true;
        this.tutorial = tutorial;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if(tutorialClick) tutorial.FinishGimic();
        tutorialClick = false;
    }
}
