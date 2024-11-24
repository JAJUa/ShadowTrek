using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CharacterRole
{
    Sera,
    Papa
};
public class Character : MonoBehaviour
{
    public CharacterRole role;
    [HideInInspector] public PathFind pathFind;
    public List<PointInTime> pointInTime;
    public Animator animator;
    [HideInInspector] public TileMoveScript tileMove;
    public float moveSpeed;
    public CurCharacter curCharacter;
    public bool isLight = false;
    public Coroutine moveCoroutine;
    public Vector3 startPos;
    public Quaternion startRot;
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
        startPos = transform.position;
        startRot= transform.rotation;

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
                    if (hit.collider.CompareTag("MoveTile")) 
                    {
                        Debug.Log("ClickTile");
                        InGameManager.Inst.moveBlock = true;
                        Tile tile = TileFinding.GetOneTile(transform.position);
                        tile.character = null;
                        Vector3 tilePosition = hit.collider.transform.position;

                        Vector3Int startPos = Vector3Int.RoundToInt(transform.position);
                        Vector3Int targetPos = Vector3Int.RoundToInt(tilePosition);
                        InGameFXManager.Inst.TileClickParticle(tilePosition);
                        if(AudioManager.Inst != null)
                             AudioManager.Inst.AudioEffectPlay(2);
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
        else InGameManager.Inst.ReplayModeRestart();
    }

    public virtual void ResetCharacter()
    {
        DOVirtual.DelayedCall(0.1f, () => transform.position = startPos);
        DOVirtual.DelayedCall(0.1f, () => transform.rotation = startRot);
        Tile tile = TileFinding.GetOneTile(startPos);
        tile.character = this;
    }


}
