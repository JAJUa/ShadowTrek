using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChange : MonoBehaviour
{
    public int RoomNum;
    int def_roomNum;

    [Space(10)] [Header("-- New Camera Moving --")]

    public bool PlayerFollow;
    public Vector3 CameraPosition;
    public float CameraSize = 45;

    [Space(10)] [Header("-- Default Camera Moving --")]

    public bool def_PlayerFollow;
    public Vector3 def_CameraPosition;
    public float def_CameraSize = 45;

    private void Start()
    {
        def_roomNum = RoomNum;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerControl"))
        {
            StartCoroutine(roomChange());
        }
        
    }

    IEnumerator roomChange()
    {
        InGameFXManager.Inst.CircleTransition(1, 1, 0);

        yield return new WaitForSeconds(1);

        InGameManager.Inst.Rooms[RoomNum].SetActive(true);
        for (int i = 0; i < InGameManager.Inst.Rooms.Length; i++)
        {
            if (i != RoomNum)
            {
                InGameManager.Inst.Rooms[i].SetActive(false);
            }
        }
        if (RoomNum == def_roomNum)
            InGameManager.Inst.CameraSetting(PlayerFollow, CameraPosition, CameraSize);
        else
            InGameManager.Inst.CameraSetting(def_PlayerFollow, def_CameraPosition, def_CameraSize);

        InGameFXManager.Inst.CircleTransition(1, 0, 1);

        if(RoomNum == def_roomNum)
            RoomNum--;
        else
            RoomNum = def_roomNum;
        //TileMoveScript.Inst.TileResearch();
        yield return null;
    }
}
