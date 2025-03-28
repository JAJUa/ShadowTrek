using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using VInspector;
using static UnityEngine.Rendering.DebugUI;

public class TurnLight : InteractiveObject
{
    [SerializeField] float turnSpeed;
    [SerializeField] float turnAngle;
    [SerializeField] GameObject lampClickTileParent;
    public List<LampClickTile> lampClickTiles = new List<LampClickTile>();
    [SerializeField]Quaternion firstRot;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < lampClickTileParent.transform.childCount; i++)
        {
            lampClickTiles.Add(lampClickTileParent.transform.GetChild(i).transform.GetComponent<LampClickTile>());
            lampClickTiles[i].gameObject.SetActive(false);
        }


        turnAngle = Mathf.Abs(turnAngle);
        firstRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Button]
    public override void AutoLight()
    {
        /*
        Collider[] colliders = Physics.OverlapBox(transform.position + pos, size / 2, Quaternion.identity, LayerMask.GetMask("Player"));
        if (colliders.Length > 0)
        {
            Vector3 dirToTarget = (colliders[0].transform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(dirToTarget);
            Vector3 targetEulerAngles = new Vector3(0, targetRotation.eulerAngles.y, 0); // Store the Euler angles as a Vector3
            float angle = Mathf.Round(targetEulerAngles.y / 45) * 45;
            Debug.Log(angle);
            Turning(angle);
        }*/
        Turning(transform.eulerAngles.y + turnAngle, true);
    }

    [Button]
    public override void TurnAction()
    {
        Debug.Log("턴");
        Turning(transform.eulerAngles.y + turnAngle, true);
    }


    [Button]
    public override void ResetObj()
    {
        Debug.Log("리셋");
        turnAngle = Mathf.Abs(turnAngle); 
        Turning(firstRot.eulerAngles.y,true);
    }


    public void Turning(float angle, bool isResetLight = false,bool lightFinished = false,bool isPapaStay = false)
    {
        Debug.Log("돌이감");
        if (isPapaStay) { if (AudioManager.Inst != null) AudioManager.Inst.AudioEffectPlay(4); }
        else if (AudioManager.Inst != null) AudioManager.Inst.AudioEffectPlay(3);
        Vector3 target = new Vector3(transform.eulerAngles.x,angle , transform.eulerAngles.z);
         transform.DORotate(target, turnSpeed, RotateMode.FastBeyond360).OnComplete(()=>
         {
             //interactiveLight.ChangeTileColor();
             if(!isResetLight)InGameManager.Inst.OnlyPlayerReplay(isPapaStay,lightFinished);
             });
    }




    public void TurnReverse()
    {
        turnAngle = -turnAngle;
        Turning(transform.eulerAngles.y + turnAngle,false,true,true);
    }

    /*
    [Button]
    public void GeneralTileAppear(bool appear, Dialouge dia = null)
    {
        Debug.Log(dia);
        foreach(LampClickTile clickTile in lampClickTiles)
        {
            if (Physics.Raycast(clickTile.transform.position, Vector3.down, 10, LayerMask.GetMask("MoveTile")))
                clickTile.AppearTile(appear, dia);
        };
    }*/
}
