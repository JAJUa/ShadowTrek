using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VInspector.Libs;

public enum CharacterRole
{
    Sera,
    Papa
};
public class Character : MonoBehaviour
{
    public CharacterRole role; 
    protected PathFind pathFind;
    protected PathFindAI pathFindAI;
    public List<PointInTime> pointInTime;
    public Animator animator;
    public float moveSpeed;
    public CurCharacter curCharacter;
    public bool isLight = false;
    public Coroutine moveCoroutine;
    public Vector3 startPos;
    public Quaternion startRot;
    public LineRenderer lineRenderer;


    protected virtual void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        pathFindAI = GetComponent<PathFindAI>();
        if (animator != null) animator.GetComponent<Animator>();
        pointInTime = new List<PointInTime>();
    }
    
    protected virtual void Start()
    {
        pathFind = PathFind.Inst;
        startPos = transform.position;
        startRot = transform.rotation;
        pathFindAI.Init(moveSpeed,this,pointInTime);
    }

    public virtual void CharacterMove()
    {
        if (Input.GetMouseButtonDown(0) && IsCharacterTurn() && !InGameManager.Inst.moveBlock)
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
                        Tile tile = TileFinding.GetOneTile( Vector3Int.RoundToInt(transform.position));
                        tile.character = null;
                        Vector3 tilePosition = hit.collider.transform.position;
                        Vector3Int _startPos = Vector3Int.RoundToInt(transform.position);
                        Vector3Int _targetPos = Vector3Int.RoundToInt(tilePosition);
                        InGameFXManager.Inst.TileClickParticle(tilePosition);
                        if(AudioManager.Inst != null)
                             AudioManager.Inst.AudioEffectPlay(2);
                        var finalNodeList =  pathFind.PathFinding(_startPos, _targetPos);
                        Debug.Log(finalNodeList.Count);
                        moveCoroutine =  StartCoroutine(pathFindAI.MoveAlongPath(finalNodeList)); 
                    }
                }
            }
            else
            {
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, results);

                foreach (RaycastResult result in results)
                {
                    Debug.Log("Hit " + result.gameObject.name);
                }
            }
        }
    }



    public virtual void InLight() { }

    public virtual void EnterReplayMode(){}

    public bool IsCharacterTurn()//현재 캐릭터의 턴인가
    {
        if (InGameManager.Inst.curCharacter == curCharacter)
        { return true; }
        else
        { return false; }
    } 

    public virtual void CharacterDead()
    {
        if (!InGameManager.Inst.papa)
            InGameManager.Inst.GameReStart();
        else InGameManager.Inst.ReplayModeRestart();
    }

    public virtual void ResetCharacter()
    {
        DOVirtual.DelayedCall(0.1f, () => transform.position = startPos);
        DOVirtual.DelayedCall(0.1f, () => transform.rotation = startRot);
        Tile tile = TileFinding.GetOneTile(startPos);
        tile.character = this;
    }


}
