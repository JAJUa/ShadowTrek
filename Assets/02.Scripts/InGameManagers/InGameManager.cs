using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public bool moveBlock = false;
    public bool isInteractionDetect;
    public bool inRelpayMode;
    public CurCharacter curCharacter;
    public bool changeRoom;
    public int curStageNum;
    [SerializeField]Player player;
    [SerializeField]ShadowModePapa papa;
    public CameraMove cam;
    public Vector3 camPos;
    public Vector3 camRot;
    [ShowIf("changeRoom")]
    [Space(10)] [Header("-- Room --")]
    public bool[] isKey;
    public GameObject[] Rooms; // RoomChange Script

  


    void Awake()
    {

        Inst = this;

        GameObject pl = GameObject.FindGameObjectWithTag("PlayerControl");
        player = pl.GetComponent<Player>();
        GameObject pa = GameObject.FindGameObjectWithTag("Papa");
        papa = pa.GetComponent<ShadowModePapa>();

        curCharacter = CurCharacter.Player;      
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>();
        camPos = new Vector3(cam.transform.position.x,cam.transform.position.y, cam.transform.position.z);
        camRot = cam.transform.eulerAngles;
        //Dont Create 2 GameManager
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }

        papa.gameObject.SetActive(false);

    }


    public void GameReStart()
    {
        moveBlock = true;
        FadeInFadeOut.Inst.FadeIn();
        int index = SceneManager.GetActiveScene().buildIndex;
        DOVirtual.DelayedCall(1.5f, () => SceneManager.LoadScene(index));
    }

    public void PapaRestart()
    {
        moveBlock = true;
        FadeInFadeOut.Inst.FadeIn();
        RePlay.Inst.ResetReplayMode();
        CameraPosReset();
        papa.ResetPos();
        player.playerInLight();
        DOVirtual.DelayedCall(1.5f, () => { FadeInFadeOut.Inst.FadeOut(); moveBlock = false; }) ;
        
    }

    public void StayPapa()
    {
        if(!moveBlock)
            OnlyPlayerReplay();
    }

    public void OnlyPlayerReplay()
    {
        if (RePlay.Inst.isReplayMode)
        {
            isInteractionDetect = true;
            RePlay.Inst.ReMove();
          
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
