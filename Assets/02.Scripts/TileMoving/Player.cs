using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Localization.Platform.Android;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using VInspector;

public class Player : Character
{
    [SerializeField] private bool isReplay; //목표 지점에 도착하면 다시 돌아갈 것인가
    [SerializeField] private Transform bwShaderSphere;
    [SerializeField] private bool seraInv;  //무적 기능 (테스트용)
    [SerializeField] private int shadowIndex = 3;

    [SerializeField] private List<Vector3Int> path;

    private RePlay replay;

    protected override void Awake()
    {
        replay = GetComponent<RePlay>();
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {     
        animator = transform.GetChild(0).GetComponent<Animator>();
             
        pointInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
        base.Start();
        move();

    }

    [Button]
    public void move()
    {
        path = MapDataManager.Inst.Data.mapData[MapDataManager.Inst.testMapIndex].seraPath;
        if(path.Count>0)
            CharacterMove();
        var _path = pathFind.ReturnNodePath(path);
        moveCoroutine =  StartCoroutine(pathFindAI.MoveAlongPath(_path)); 
    }

  
    public override void CharacterMove()
    {
      
    }


    public override void EnterReplayMode()
    {
        ResetCharacter();
        replay.Init(pointInTime);
    }

    public override void InLight()
    {
    
        Tile tile = TileFinding.GetOneTile(transform.position);
        if(tile==null)
            Debug.LogError("타일이 없음");
        tile.character = this;
        Debug.Log("판정");
        if (!tile.isLight && !seraInv)
        {
           
            SetShadowIndex( --shadowIndex);
         
        }
        else
        {
            playerInLight();
            
        }
        if (InGameManager.Inst.inRelpayMode)
            ReplayMode(tile);
        else 
            UnReplayMode(tile);
       
     
    }

    protected override void ReplayMode(Tile _tile)
    {
        if (_tile.isEndTile)
        {
            InGameManager.Inst.StopMoving();
            InGameManager.Inst.moveBlock = true;
            // tile.endCutScene.StartCutScene();
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
        if (_tile.isEndTile)
        {
            InGameManager.Inst.StopMoving();
            InGameManager.Inst.EnterReplayMode();
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
            return;
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
        playerInLight();
        
    }
}

