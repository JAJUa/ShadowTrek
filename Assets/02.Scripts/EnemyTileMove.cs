using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyTileMove : MonoBehaviour
{
    PathFind pathFind;
    Animator animator;
    CurCharacter character;
    public List<PointInTime> pointInTime;
    public TileMoveScript tileMove;
    public Vector3Int[] targetTile;
    Vector3Int startPos, targetPos;
    [SerializeField] float moveSpeed,detectDistance;
    [SerializeField] LayerMask playerLayer;
    int index = 0;
    bool playerDetect,moveToPlayer;

    private Coroutine runCoroutine,detectCoroutine;

    private void Awake()
    {
        animator= GetComponent<Animator>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        /*
        pointInTime = new List<PointInTime>();
        tileMove = TileMoveScript.Inst;
        pathFind = new PathFind(tileMove.pathfind);
        startPos = Vector3Int.RoundToInt(transform.position);
        targetPos = targetTile[index];
        pathFind.FindPath(startPos, targetPos);
        Debug.Log("적" + pathFind.FinalNodeList.Count);
        runCoroutine = StartCoroutine(tileMove.MoveAlongPath(gameObject,animator, pathFind, moveSpeed, character,pointInTime));
        StartCoroutine(CheckCorutineFinished());*/
    }

    private void Update()
    {
        /*
        RaycastHit hit;
        if(Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward,out hit, detectDistance, playerLayer) && !moveToPlayer)
        {
            moveToPlayer = true;
            playerDetect = true;
            StopCoroutine(runCoroutine);
            startPos = Vector3Int.RoundToInt(transform.position);
            targetPos = Vector3Int.RoundToInt(hit.transform.position);
            Debug.Log(targetPos);
            pathFind.FindPath(startPos, targetPos);
            float moveSpeed = 40;
         //   StartCoroutine(tileMove.MoveAlongPath(gameObject,animator, pathFind, moveSpeed));
            Debug.Log("플레이어 발견");


        }

        if(playerDetect)
        {
            AttackPlayer();
            
        }
        */
    }

    void AttackPlayer()
    {
        if(Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward, detectDistance / 3, playerLayer))
        {
            Debug.Log("플레이어 죽음");

        }
    }

    /*
    private IEnumerator CheckCorutineFinished()
    {
        
        yield return runCoroutine;

        startPos = Vector3Int.RoundToInt(transform.position);
        if (index < targetTile.Length-1)
        {
            index++; 
        }
        else
        {
            index = 0;
        }
        Debug.Log(index);
        targetPos = targetTile[index];
        pathFind.FindPath(startPos, targetPos);
        runCoroutine = StartCoroutine(tileMove.MoveAlongPath(gameObject,animator, pathFind, moveSpeed,character,pointInTime));
        StartCoroutine(CheckCorutineFinished());
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position +  new Vector3(0,1f,0), transform.forward * detectDistance);
    }
}
