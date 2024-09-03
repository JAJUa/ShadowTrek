using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Localization.Platform.Android;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class Player : Character
{
    [SerializeField] GameObject shadowPapa;
    [SerializeField] Transform endObj;
    [SerializeField] bool isReplay; //목표 지점에 도착하면 다시 돌아갈 것인가
    [SerializeField] Transform bwShaderSphere;
    [SerializeField] PlayableDirector playableDirector;
    public int shadowIndex = 3;


    private void Awake()
    {
        startPos = transform.position;
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


    public void CheckReplay()
    {
        if (isReplay && !InGameManager.Inst.inRelpayMode)
        {
            if (Vector3.Distance(transform.position,endObj.position)<2f)
            {
               EnterReplayMode();
            }
        }
        else if (InGameManager.Inst.inRelpayMode || !isReplay)
        {
            if (Vector3.Distance(transform.position, endObj.position) <2f)
            {
                playableDirector.Play();
            }
           
        }


    }

    public void EnterReplayMode()
    {
        StopCoroutine(moveCoroutine);
        shadowPapa.SetActive(true);
        InGameManager.Inst.inRelpayMode = true;
        RePlay.Inst.isReplayMode= true;
        RePlay.Inst.RePlayMode(gameObject, animator, pointInTime);
        animator.SetBool("isWalk", false);

        InGameManager.Inst.CameraPosReset();
        return;
    }

    public override void InLight()
    {
        Debug.Log(isLight);
        if (!isLight)
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
    }

    public void playerInLight()
    {
        isLight = false;
        bwShaderSphere.DOScale(0, 0.5f);
        shadowIndex = 3;
    }
}

