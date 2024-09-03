using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour
{
    [HideInInspector] public PathFind pathFind;
    public List<PointInTime> pointInTime;
    public Animator animator;
    [HideInInspector] public TileMoveScript tileMove;
    public float moveSpeed;
    public CurCharacter curCharacter;
    public bool isLight = false;
    public Coroutine moveCoroutine;
    public Vector3 startPos;
    public LineRenderer lineRenderer;

    // Start is called before the first frame update
    public virtual void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();   
        tileMove = TileMoveScript.Inst;
        pathFind = new PathFind(TileMoveScript.Inst.bottomLeft, TileMoveScript.Inst.topRight, 15, true, true, LayerMask.GetMask("Ground"));
        pathFind.FindPath(Vector3Int.FloorToInt(Vector3Int.RoundToInt(transform.position)), TileMoveScript.Inst.topRight);
        if (animator != null) animator.GetComponent<Animator>();
        pointInTime = new List<PointInTime>();

    }

    public virtual void CharacterMove()
    {
        if (Input.GetMouseButtonDown(0) && IsCharacterTurn() && !InGameManager.Inst.moveBlock)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("MoveTile") && !Physics.Raycast(hit.transform.position, Vector3.up, 3f))  //MoveTile 감지 및 위에 플레이어나 펫이 있는지 감지
                    {
                        Debug.Log("ClickTile");
                        InGameManager.Inst.moveBlock = true;

                        Vector3 tilePosition = hit.collider.transform.position;

                        Vector3Int startPos = Vector3Int.RoundToInt(transform.position);
                        Vector3Int targetPos = Vector3Int.RoundToInt(tilePosition);
                        InGameFXManager.Inst.TileClickParticle(tilePosition);
                        pathFind.FindPath(startPos, targetPos);
                        moveCoroutine =  StartCoroutine(tileMove.MoveAlongPath(gameObject, animator, pathFind, moveSpeed, curCharacter, pointInTime)); 
                    }
                }
            }
            else
            {
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, results);

                foreach (RaycastResult result in results)
                {
                    Debug.Log("Hit " + result.gameObject.name);
                }
            }
        }
    }



    public virtual void InLight() { }

    public bool IsCharacterTurn()//현재 캐릭터의 턴인가
    {
        if (InGameManager.Inst.curCharacter == curCharacter)
        { return true; }
        else
        { return false; }
    } 

    public virtual void CharacterDead()
    {
        if (InGameManager.Inst.noPapaButDetect)
            InGameManager.Inst.GameReStart();
        else InGameManager.Inst.PapaRestart();
    }


}
