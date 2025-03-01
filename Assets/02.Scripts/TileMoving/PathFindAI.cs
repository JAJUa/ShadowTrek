using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindAI : MonoBehaviour
{
    [SerializeField] private List<Node> finalNodeList;
    [SerializeField] private float speed;
    private Character character;
    private List<PointInTime> pointsInTime;
    private Animator animator;

    public void Init(float _speed,Character _character,List<PointInTime> _pointInTime)
    {
        speed = _speed;
        character = _character;
        pointsInTime = _pointInTime;
        animator = character.animator;
    }
    
     public IEnumerator MoveAlongPath(List<Node> _finalNodeList )
     {
         finalNodeList = _finalNodeList;
         animator.SetBool("isWalk", true);
        for (int passtile = 0; passtile < finalNodeList.Count - 1; passtile++)
        {
            // RePlay 리플레이 모드면 실행
            if (InGameManager.Inst.inRelpayMode)
            {
                RePlay.Inst.ReMove(false);
            }
            
            Debug.Log("이동 실행");
            yield return StartCoroutine(MoveToPosition(new Vector3(finalNodeList[passtile + 1].x, character.transform.position.y, finalNodeList[passtile + 1].z), passtile));
            
        }


      
        animator.SetBool("isWalk", false);

        InGameManager.Inst.moveBlock = false;

    }

    


    private IEnumerator MoveToPosition( Vector3 targetPosition, int passtile)
    {
        Debug.Log("이동 중");
        //TurnAction();
        // Position
        Vector3 startPosition = character.transform.position;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float timeToMove = distance / speed;

        // Rotation
        Vector3 direction = (targetPosition - character.transform.position).normalized;
        Quaternion startRotation = character.transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float elapsedTime = 0;
        if(AudioManager.Inst != null)
            AudioManager.Inst.AudioEffectPlay(0);
        // Walking
        while (elapsedTime < timeToMove)
        {
            LineRenderer(passtile);
            character.transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / timeToMove));
            character.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, (elapsedTime / (timeToMove / 2.5f)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        character.transform.position = targetPosition;
        //이동이 끝났을 때
       
        pointsInTime.Insert(0, new PointInTime(character.transform.position, character.transform.rotation));
        
        LightManager.Inst.ActionFinish();



        

    }
    
    public void LineRenderer(int passtile)
    {
        LineRenderer lineRenderer = character.lineRenderer;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;

      
        if (finalNodeList.Count - passtile > 1)
        {
            lineRenderer.positionCount = finalNodeList.Count - passtile;
            lineRenderer.SetPosition(0, new Vector3(character.transform.position.x, 2.7f, character.transform.position.z));
            lineRenderer.SetPosition(1, new Vector3(finalNodeList[passtile + 1].x, 2.7f, finalNodeList[passtile + 1].z));

            for (int i = 2; i < finalNodeList.Count; i++)
            {
                if (passtile + i < finalNodeList.Count)
                    lineRenderer.SetPosition(i, new Vector3(finalNodeList[passtile + i].x, 2.7f, finalNodeList[passtile + i].z));
            }
        }
    }
}
