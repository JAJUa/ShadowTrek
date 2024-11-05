using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Localization.Platform.Android;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using VInspector;

public class Player : Character
{
    [SerializeField] private bool isReplay; //목표 지점에 도착하면 다시 돌아갈 것인가
    [SerializeField] private Transform bwShaderSphere;
    [SerializeField] private bool seraInv;  //무적 기능 (테스트용)
    [SerializeField] private PlayableDirector endPlayableDirector;
    [SerializeField] private int shadowIndex = 3;


    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    public override void Start()
    {     
        animator = transform.GetChild(0).GetComponent<Animator>();
        base.Start();        
        pointInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
    }

    // Update is called once per frame
    void Update()
    {
        CharacterMove();
    }
    

    public void EnterReplayMode()
    {
        StopCoroutine(moveCoroutine);
        InGameManager.Inst.inRelpayMode = true;
        InGameManager.Inst.PapaActive(true);
        InGameManager.Inst.inRelpayMode = true;
        RePlay.Inst.RePlayMode(gameObject, animator, pointInTime);
        animator.SetBool("isWalk", false);

        playerInLight();

        InGameManager.Inst.CameraPosReset();
        InGameUIManager.Inst.SpriteChange(false);
        if(!InGameManager.Inst.isAnswering)
            InGameUIManager.Inst.StayBtnActive(true);

        if (InGameManager.Inst.isAnswering)
            AnswerManager.Inst.ChangeChracter(false);
        return;
    }

    public override void InLight()
    {
    
        Tile tile = TileFinding.GetOneTile(transform.position);
        tile.character = this;
        Debug.Log("판정");
        if (!tile.isLight && !seraInv)
        {
            shadowIndex--;
            if(shadowIndex == 1)
            {
                bwShaderSphere.DOScale(300, 1f);
            }
            if(shadowIndex == 0)
            {
                CharacterDead();
                return;
            }
        }
        else
        {
            playerInLight();
            
        }

        if (tile.isEndTile)
        {
            if (isReplay && !InGameManager.Inst.inRelpayMode)
            {
                InGameManager.Inst.StopMoving();
                EnterReplayMode();
            }
            else if (InGameManager.Inst.inRelpayMode || !isReplay)
            {
                InGameManager.Inst.StopMoving();
                InGameManager.Inst.moveBlock = true;
                endPlayableDirector.Play();
            }
           
        }
    }

    [Button]
    public void playerInLight() //플레이어 빛에 닿게
    {
        isLight = false;
        bwShaderSphere.DOScale(0, 0.5f);
        shadowIndex = 3;
    }


}

