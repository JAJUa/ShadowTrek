using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommandBtn : MonoBehaviour,IPointerClickHandler
{
    private EditorController editorController;
    [SerializeField] private EditorController.CommandState commandState;
    public void Init(EditorController _editor)
    {
        editorController = _editor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       editorController.ChangeState(commandState);
    }
}
