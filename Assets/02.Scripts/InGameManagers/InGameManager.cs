using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
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
    public bool noPapaButDetect;
    public bool isAnswering;
    public CurCharacter curCharacter;
    public int curStageNum;
    [SerializeField]Player player;
    [SerializeField]ShadowModePapa papa;
    [SerializeField] bool onlyPlayer;
    [SerializeField] CutSceneManager endCutScene;
    AnswerManager answerManager;

    
    public bool changeRoom;
    public CameraMove cam;
    private Vector3 camPos, camRot;
    [HideInInspector] public bool isCutsceneIn;

    [Tab("ETC")]
    [SerializeField] Volume globalVolume;
    [SerializeField] VolumeProfile defaultVolume, answerVolume;

    [ShowIf("changeRoom")]
    [Space(10)] [Header("-- Room --")]
    public bool[] isKey;
    public GameObject[] Rooms; // RoomChange Script

   

  


    void Awake()
    {
       
        answerManager = GetComponent<AnswerManager>();
        GameObject pl = GameObject.FindGameObjectWithTag("PlayerControl");
        player = pl.GetComponent<Player>();
        if (!onlyPlayer)
        {
            GameObject pa = GameObject.FindGameObjectWithTag("Papa");
            papa = pa.GetComponent<ShadowModePapa>();
        }

        curCharacter = CurCharacter.Player;      
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>();
        camPos = new Vector3(cam.transform.position.x,cam.transform.position.y, cam.transform.position.z);
        camRot = cam.transform.eulerAngles;
        //Dont Create 2 GameManager
        if (Inst != null && Inst != this)
        {
            Destroy(Inst.gameObject);
            Inst = this;
        }
        else
        {
            Inst = this;
        }
    }

    public void ChangeGlobalVolume(bool isAnswer)
    {
        globalVolume.profile = isAnswer ? answerVolume : defaultVolume;
    }

    private void Start()
    {
        if (!onlyPlayer)
            papa.gameObject.SetActive(false);
    }


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
       
        DOVirtual.DelayedCall(0.8f, () => { papa.ResetPos();player.ResetPos(); inRelpayMode = false;  isAnswering = false; 
                                            player.pointInTime = new List<PointInTime>();
        });
        papa.gameObject.SetActive(false);
        player.playerInLight();
        TileMoveScript.Inst.ResetLight();
        DOVirtual.DelayedCall(1.75f, () => { FadeInFadeOut.Inst.FadeOut(); moveBlock = false; RePlay.Inst.ResetReplayLine();
            player.pointInTime.Insert(0, new PointInTime(player.transform.position, player.transform.rotation)); answerManager.SeraTile(); });
    }

    public void PapaRestart()
    {
        endCutScene.StopCutScene();
        StopAllCoroutines();
        moveBlock = true;
        FadeInFadeOut.Inst.FadeIn();
       
        CameraPosReset();
        StopMoving();
        DOVirtual.DelayedCall(0.8f,()=> papa.ResetPos());
        RePlay.Inst.RestartReplayMode();
        player.playerInLight();
        TileMoveScript.Inst.ResetLight();
        DOVirtual.DelayedCall(1.75f, () => { FadeInFadeOut.Inst.FadeOut(); moveBlock = false; }) ;
        
    }

    public void StopMoving()
    {
        if (papa != null)
        {
            if (papa.moveCoroutine != null)
                StopCoroutine(papa.moveCoroutine);

            Debug.Log("213123");
            DOVirtual.DelayedCall(0f, () => papa.lineRenderer.positionCount = 0);
        }

        DOVirtual.DelayedCall(0.1f, () => player.lineRenderer.positionCount = 0); 
        moveBlock = true;
      
        player.animator.SetBool("isWalk", false);
        Debug.Log(player.lineRenderer.positionCount);
        if (player.moveCoroutine != null)
            StopCoroutine(player.moveCoroutine);

      
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
            OnlyPlayerReplay();

        
    }

    public void OnlyPlayerReplay(bool isPapaStay = false,bool lightFinished = false)
    {
        if (InGameManager.Inst.inRelpayMode)
        {
            if (isAnswering) AnswerManager.Inst.PapaTile();
            isInteractionDetect = true;
            if(!lightFinished)TileMoveScript.Inst.TurnAction();
            RePlay.Inst.ReMove(isPapaStay);
          
        }
    }

    public void CameraPosReset()
    {
        cam.transform.DOMove(camPos,1f);
        cam.transform.DORotate(new Vector3(camRot.x, camRot.y, camRot.z), 1f);
    }

    public void CheckReplay()
    {
        player.CheckReplay();
    }

    public void DetectCharacterLight()
    {
        player.InLight();
        if (papa != null) papa.InLight();
    }



    public void CameraSetting(bool PlayerFollow, Vector3 CameraPosition = default(Vector3), float cameraSize = 45)
    {
        cam.CameraSetting(PlayerFollow, CameraPosition, cameraSize);
    }


}
