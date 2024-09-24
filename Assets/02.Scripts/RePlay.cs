using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class RePlay : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;

    public static RePlay Inst;
    public bool isReplayMode;
    bool isBacking;

    private GameObject character;
    private Animator animCharacter;
    Player player;
    public List<PointInTime> pointsInTime;
    [SerializeField]private List<PointInTime> savedPointsInTime;
    private List<PointInTime> savedPointsInLine;
    public List<PointInTime> pointsInLine;


    private void Awake()
    {
        Inst = this;
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (isReplayMode && !isBacking) { LineRenderer(); }

    }

    public void RePlayMode(GameObject character, Animator animCharacter, List<PointInTime> pointsInTime)
    {
        FadeInFadeOut.Inst.FadeIn();
        DOVirtual.DelayedCall(1f, ()=>FadeInFadeOut.Inst.FadeOut());
        pointsInTime.Reverse();
        isReplayMode = true;
        InGameManager.Inst.curCharacter = CurCharacter.Papa;
        InGameManager.Inst.moveBlock = false;

        this.character = character;
        this.animCharacter = animCharacter;
        this.pointsInTime = pointsInTime.ToList();
        this.pointsInLine = pointsInTime.ToList();
        savedPointsInTime = this.pointsInTime.ToList();
        savedPointsInLine = pointsInLine.ToList();
        
        character.transform.position = this.pointsInTime[0].position;
        character.transform.rotation = this.pointsInTime[0].rotation;
        this.pointsInTime.RemoveAt(0);
        pointsInLine.RemoveAt(0);
       // StartCoroutine(back());
    }

    public void ResetReplayMode()
    {
        InGameManager.Inst.moveBlock = false;

        pointsInTime = savedPointsInTime.ToList();
        pointsInLine = savedPointsInLine.ToList();
        character.transform.position = pointsInTime[0].position;
        character.transform.rotation = pointsInTime[0].rotation;
        pointsInTime.RemoveAt(0);
        pointsInLine.RemoveAt(0);
    }



    public void ReMove()
    {
        if (pointsInTime.Count <= 0) return;
        StartCoroutine(MoveToFrontTile());
        pointsInTime.RemoveAt(0);
    }


    public IEnumerator MoveToFrontTile()
    {
      
        InGameManager.Inst.moveBlock = true;
        PointInTime pointInTime = pointsInTime[0];
        Vector3 targetPosition = pointInTime.position;
        animCharacter.SetBool("isWalk", true);
        // Position
        Vector3 startPosition = character.transform.position;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float timeToMove = distance / 30;


        character.transform.rotation = pointInTime.rotation;

        float elapsedTime = 0;

        // Walking
        while (elapsedTime < timeToMove)
        {
            character.transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / timeToMove));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
     
        character.transform.position = targetPosition;
        animCharacter.SetBool("isWalk", false);
        pointsInLine.RemoveAt(0);
        if (InGameManager.Inst.isInteractionDetect)
        {
            TileMoveScript.Inst.DetectLight();
            InGameManager.Inst.isInteractionDetect = false;
            InGameManager.Inst.moveBlock = false;
        }
        player.CheckReplay();
       
    }

    // LineRenderer
    public void LineRenderer()
    {
        lineRenderer.positionCount = (pointsInLine.Count + 1);
        lineRenderer.SetPosition(0, new Vector3(character.transform.position.x, 2.7f, character.transform.position.z));
        for (int i = 0; i < pointsInLine.Count; i++)
        {
            lineRenderer.SetPosition(i + 1, new Vector3(pointsInLine[i].position.x, 2.7f, pointsInLine[i].position.z));
        }
    }
}
