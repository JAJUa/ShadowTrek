using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using VInspector;

public enum CurCharacter
{
    Player, Papa,Enemy,Pet
}

public class InGameManager : MonoBehaviour
{
    public static InGameManager Inst;

    [Tab("InGame")]
    public bool moveBlock = false;
    public bool isInteractionDetect;
    public bool inRelpayMode;
    public bool isAnswering;
    public CurCharacter curCharacter;
    public int curStageNum;
    public Player player;
    public ShadowModePapa papa;
    [SerializeField] bool onlyPlayer;
    [SerializeField] CutSceneManager endCutScene;
    AnswerManager answerManager;

    
    public bool changeRoom;
    public Transform cam;
    private Vector3 camPos, camRot;
    [HideInInspector] public bool isCutsceneIn;
    

   

  


    private void Awake()
    {
        //Dont Create 2 GameManager
        if (Inst != null && Inst != this)
        {
            Destroy(InGameManager.Inst);
            return;
        }
        else
        {
            Inst = this;
        }
    }

   

    private IEnumerator Start()
    {
        yield return new WaitUntil(()=>MapDataManager.Inst);
    
        answerManager = GetComponent<AnswerManager>();
        
        foreach (var spawnCharacter in MapDataManager.Inst.Data.mapData[MapDataManager.Inst.testMapIndex].spawnCharacters)
        {
            switch (spawnCharacter.characterRole)
            {
                case CharacterRole.Sera:
                    player = Instantiate(Resources.Load<Player>("Prefab/RealPlayer"),spawnCharacter.spawnPos,quaternion.identity);
                    break;
                case CharacterRole.Papa:
                    papa = Instantiate(Resources.Load<ShadowModePapa>("Prefab/ShadowPapa"),spawnCharacter.spawnPos,quaternion.identity);
                    break;
            }
        }
      
        
        curCharacter = CurCharacter.Player;
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        camPos = new Vector3(cam.transform.position.x,cam.transform.position.y, cam.transform.position.z);
        camRot = cam.transform.eulerAngles;

        if (papa)
            papa.gameObject.SetActive(false);
    }

    public void PapaActive(bool isActive) => papa.gameObject.SetActive(isActive);
      
    


    public void GameReStart()
    {
        moveBlock = true;
        FadeInFadeOut.Inst.FadeIn();
        int index = SceneManager.GetActiveScene().buildIndex;
        DOVirtual.DelayedCall(1.5f, () => SceneManager.LoadScene(index));
    }

    public void HintReset()
    {
        endCutScene.StopCutScene();
        StopAllCoroutines();
        moveBlock = true;
        FadeInFadeOut.Inst.FadeIn();

        CameraPosReset();
        StopMoving();
        curCharacter = CurCharacter.Player;
       
        DOVirtual.DelayedCall(0.8f, () => { papa.ResetCharacter();player.ResetCharacter(); inRelpayMode = false;  isAnswering = false; 
                                            player.pointInTime = new List<PointInTime>();
        });

        papa.gameObject.SetActive(false);
        player.playerInLight();
       // TileMoveScript.Inst.ResetLight();
        DOVirtual.DelayedCall(1.75f, () => { FadeInFadeOut.Inst.FadeOut(); moveBlock = false; RePlay.Inst.ResetReplayLine();
            player.pointInTime.Insert(0, new PointInTime(player.transform.position, player.transform.rotation)); answerManager.SeraTile(); });
    }

    
    public void EnterReplayMode()
    {
        //리플레이 모드 진입
        StopMoving();
        moveBlock = true;
        TileManager.Inst.LightOffAllTiles();
        FadeInFadeOut.Inst.FadeIn();
        
        player.EnterReplayMode();
        papa.EnterReplayMode();
       
        InGameUIManager.Inst.SpriteChange(false);
        if (!isAnswering)
        {
            InGameUIManager.Inst.StayBtnActive(!isAnswering);
            AnswerManager.Inst.ChangeChracter(!isAnswering);
        }
        
        //임시 Path소환
        PathFind.Inst.NodeSetting();
      
        LightManager.Inst.ResetLights();
        inRelpayMode = true;
        curCharacter = CurCharacter.Papa;
        DOVirtual.DelayedCall(1.75f, () => { FadeInFadeOut.Inst.FadeOut(); moveBlock = false; LightManager.Inst.NonDetectActionFinish(); }) ;
    }

    public void ReplayModeRestart()
    {
        //리플레이 모드에서 죽을때 혹은 리스타트
        LightManager.Inst.ResetLights();
        moveBlock = true;
        FadeInFadeOut.Inst.FadeIn();
        StopMoving();
        DOVirtual.DelayedCall(0.8f,()=>
        {
            player.EnterReplayMode();
            papa.EnterReplayMode();
        });
        TileManager.Inst.LightOffAllTiles();
    
        DOVirtual.DelayedCall(1.75f, () => { FadeInFadeOut.Inst.FadeOut(); moveBlock = false; LightManager.Inst.NonDetectActionFinish(); }) ;
        
    }

    public void StopMoving()
    {
        if (papa != null)
        {
            if (papa.moveCoroutine != null)
                StopCoroutine(papa.moveCoroutine);
            DOVirtual.DelayedCall(0f, () => papa.lineRenderer.positionCount = 0);
        }

        DOVirtual.DelayedCall(0.1f, () => player.lineRenderer.positionCount = 0); 
        moveBlock = true;
      
        player.animator.SetBool("isWalk", false);
        if (player.moveCoroutine != null)
            StopCoroutine(player.moveCoroutine);

      
    }

    public void StartEndCutScene()
    {
        endCutScene.StartCutScene();
    }

    private void Update()
    {
        if (isCutsceneIn)
        {
            if (Input.GetMouseButton(0)) CutSceneSpeedSet(true);

            if (Input.GetMouseButtonUp(0) && Time.timeScale != 1) CutSceneSpeedSet(false);
        }
        else
        {
            if (Time.timeScale != 1) CutSceneSpeedSet(false);
        }
    }

    void CutSceneSpeedSet(bool speedUp) //컷씬 속도 변경
    {
        Time.timeScale = speedUp?3:1;
        Time.fixedDeltaTime = 1 / Time.timeScale * 0.02f;
        InGameUIManager.Inst.CutSceneSpeedUp(speedUp);
    }

    public void StayPapa()
    {
        if(!moveBlock)
            OnlyPlayerReplay(true,false);
    }

    public void OnlyPlayerReplay(bool isPapaStay = false,bool lightFinished = false)
    {
        if (inRelpayMode)
        {
            if (isAnswering) AnswerManager.Inst.PapaTile();
            isInteractionDetect = true;
            RePlay.Inst.ReMove(isPapaStay);
        }
    }

    private void CameraPosReset()
    {
        cam.transform.DOMove(camPos,1f);
        cam.transform.DORotate(new Vector3(camRot.x, camRot.y, camRot.z), 1f);
    }



   
    


}
