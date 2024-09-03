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
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < lampClickTileParent.transform.childCount; i++)
        {
            Debug.Log(lampClickTileParent.transform.GetChild(i).gameObject.name);
            lampClickTiles.Add(lampClickTileParent.transform.GetChild(i).transform.GetComponent<LampClickTile>());
            lampClickTiles[i].gameObject.SetActive(false);
        }


        turnAngle = Mathf.Abs(turnAngle);
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Button]
    public override void AutoLight()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + pos, size / 2, Quaternion.identity, LayerMask.GetMask("Player"));
        if (colliders.Length > 0)
        {
            Vector3 dirToTarget = (colliders[0].transform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(dirToTarget);
            Vector3 targetEulerAngles = new Vector3(0, targetRotation.eulerAngles.y, 0); // Store the Euler angles as a Vector3
            float angle = Mathf.Round(targetEulerAngles.y / 45) * 45;
            Debug.Log(angle);
            Turning(angle);
        }
    }


    public void Turning(float angle)
    {
         Vector3 target = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
         transform.DORotate(target, turnSpeed, RotateMode.FastBeyond360).OnComplete(()=>
         {
             interactiveLight.ChangeTileColor();
             InGameManager.Inst.OnlyPlayerReplay();
             });
    }


    [Button]
    public void GeneralTileAppear(bool appear, Dialouge dia = null)
    {
        Debug.Log(dia);
        foreach(LampClickTile clickTile in lampClickTiles)
        {
            if (Physics.Raycast(clickTile.transform.position, Vector3.down, 10, LayerMask.GetMask("MoveTile")))
                clickTile.AppearTile(appear, dia);
        };
    }
}
