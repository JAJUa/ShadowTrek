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
        pathFindAI.Init(moveSpeed,this,pointInTime,role);
    }

    public virtual void CharacterMove()
    {
        
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
            InGameManager.Inst.AllRestart();
        else InGameManager.Inst.ReplayModeRestart();
    }

    protected virtual void UnReplayMode(Tile _tile)
    {
        
    }
    protected virtual void ReplayMode(Tile _tile)
    {
        
    }

    public virtual void ResetCharacter()
    {
        DOVirtual.DelayedCall(0.1f, () => transform.position = startPos);
        DOVirtual.DelayedCall(0.1f, () => transform.rotation = startRot);
        Tile tile = TileFinding.GetOneTile(startPos);
        
        pathFindAI.StopMoveCor();
        tile.character = this;
    }


}
