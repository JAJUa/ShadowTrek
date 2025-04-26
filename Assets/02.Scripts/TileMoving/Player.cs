using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
//using UnityEditor.Localization.Platform.Android;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using VInspector;

public class Player : Character
{
    public List<PointInTime> pointInTime;
    [SerializeField] private Transform bwShaderSphere;
    [SerializeField] private bool seraInv;  //무적 기능 (테스트용)
    [SerializeField] private int shadowIndex = 3;

    [SerializeField] private List<Vector3Int> path;

    [HideInInspector]public RePlay replay;

    protected override void Awake()
    {
        replay = GetComponent<RePlay>();
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {     
        animator = transform.GetChild(0).GetComponent<Animator>();
        
        base.Start();

    }

    [Button]
    public override void CharacterMove()
    {
        path = MapDataManager.Inst.Data.mapData[MapDataManager.Inst.testMapIndex].seraPath;
        if (pointInTime.Count == 0)
        {
            var newPath = new List<Vector3Int>(path);
            newPath.Reverse();
            foreach (var pos in newPath)
            {
                pointInTime.Add(new PointInTime(pos,Quaternion.identity));
            }
            pointInTime.RemoveAt(0);
        }

           
        var _path = pathFind.ReturnNodePath(path);
        moveCoroutine =  StartCoroutine(pathFindAI.MoveAlongPath(_path)); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            CharacterMove();
        }
    }


    public override void EnterReplayMode()
    {
        ResetCharacter();
       
    }

    public override void InLight()
    {
    
        Tile tile = TileFinding.GetOneTile(transform.position);
        if(tile==null)
            Debug.LogError("타일이 없음");
        tile.character = this;
        Debug.Log("판정");
        if (InGameManager.Inst.CurState() == GameState.ShadowTurn)
        {
            if (!tile.isLight && !seraInv)
            {
                SetShadowIndex(--shadowIndex);
            }
            else
            {
                playerInLight();
            }
            
            ReplayMode(tile);
        }
        else 
            UnReplayMode(tile);
       
     
    }

    protected override void ReplayMode(Tile _tile)
    {
        if (_tile.isEndTile)
        {
            InGameManager.Inst.StopMoving();
            InGameManager.Inst.moveBlock = true;
            ++MapDataManager.Inst.testMapIndex;
            DOVirtual.DelayedCall(0.75f, () =>  //임시
            {
                FadeInFadeOut.Inst.FadeIn();
                MapDataManager.Inst.NextMap();
            });
        }
    }

    protected override void UnReplayMode(Tile _tile)
    {
        Debug.Log("끝에 도착");
        if (_tile.isEndTile)
        {
            InGameManager.Inst.ChangeState(GameState.ShadowTurn);
            seraInv = false;
        }
    }

    private void SetShadowIndex(int _shadowIndex)
    {
        shadowIndex = _shadowIndex;
        
        if(shadowIndex == 1)
        {
            bwShaderSphere.DOScale(300, 0.5f);
        }
        if(shadowIndex == 0)
        {
            CharacterDead();
        } 
    }


    [Button]
    public void playerInLight() //플레이어 빛에 닿게
    {
        isLight = false;
        bwShaderSphere.DOScale(0, 0.5f);
        SetShadowIndex(3);
    }

    public override void ResetCharacter()
    {
        base.ResetCharacter();
        replay.ResetReplayLine();
        replay.Init(pointInTime);
        playerInLight();
    }
}

