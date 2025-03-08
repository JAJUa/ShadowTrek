using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorController : MonoBehaviour
{
    public enum CommandState
    {
        Nothing,SeraPath,SeraPos,PapaPos
    }
    
    [SerializeField] private bool setEditor;
    [SerializeField] private List<Vector3Int> pathList;

    [SerializeField] private CommandState commandState;
    [SerializeField] private Transform btnParent;

    private void Awake()
    {
        foreach (Transform child in btnParent)
        {
            child.GetComponent<CommandBtn>().Init(this);
        }
    }

    private void Start()
    {
       if(setEditor)
           InGameManager.Inst.moveBlock = true;
    }

    private void Update()
    {
        if (!setEditor) return;
        ClickTile();
        Command();
       
    }

    private void ClickTile()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("MoveTile"))
                    {
                        InGameManager.Inst.moveBlock = true;
                        Vector3Int _targetPos = Vector3Int.RoundToInt(hit.collider.transform.position);
                        InGameFXManager.Inst.TileClickParticle(_targetPos);

                        switch (commandState)
                        {
                            case CommandState.Nothing:
                                break;
                            case CommandState.PapaPos:
                                SetCharacterPos(CharacterRole.Papa,_targetPos);
                                break;
                            case CommandState.SeraPos:
                                SetCharacterPos(CharacterRole.Sera,_targetPos);
                                break;
                            case CommandState.SeraPath:
                                SetPath(_targetPos);
                                break;
                        }
                      

                    }
                }
            }
        }
    }

    public void ChangeState(CommandState _commandState)
    {
        commandState = _commandState;
    }

    private void Command()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (pathList.Count > 0)
            {
                pathList.RemoveAt(pathList.Count-1);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.H)&&commandState == CommandState.SeraPath)
        {
            var _seraPath = MapDataManager.Inst.Data.mapData[MapDataManager.Inst.testMapIndex].seraPath;
           _seraPath.Clear();
            foreach (var v in pathList)
            {
                _seraPath.Add(v);
            }
            commandState = CommandState.Nothing;
            pathList.Clear();
        }
    }

    public void SetCharacterPos(CharacterRole role,Vector3Int _targetPos)
    {
        
        var mapdata = MapDataManager.Inst.Data.mapData[MapDataManager.Inst.testMapIndex];
        for (int i = 0; i < mapdata.spawnCharacters.Count; i++)
        {
            if (mapdata.spawnCharacters[i].characterRole == role)
            { 
                MapDataManager.Inst.Data.mapData[MapDataManager.Inst.testMapIndex].spawnCharacters[i].spawnPos = _targetPos;
            }
        }

        commandState = CommandState.Nothing;

    }

    private void SetPath(Vector3Int _targetPos)
    {
        pathList.Add(_targetPos);
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && pathList.Count>0)
        {
            Gizmos.color = Color.blue;
            foreach (var p in pathList)
            {
                Gizmos.DrawCube(p + new Vector3Int(0,2),Vector3.one);
            }
            
        }
    }
}
