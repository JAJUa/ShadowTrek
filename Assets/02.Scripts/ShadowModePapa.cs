using DG.Tweening;
using DissolveExample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShadowModePapa : Character
{

    public DissolveChilds dissolve;
    private bool firstMove = false;

    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
        startPos = transform.position;
    }
    

    // Update is called once per frame
    void Update()
    {
        CharacterMove();
   
    }

    public override void CharacterMove()
    {
        if (Input.GetMouseButtonDown(0) && !InGameManager.Inst.moveBlock)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("MoveTile")) 
                    {
                        if (!firstMove)
                        {
                            firstMove = true;
                            InGameManager.Inst.FirstMoveAction();
                        }
                        InGameManager.Inst.moveBlock = true;
                        Tile tile = TileFinding.GetOneTile( Vector3Int.RoundToInt(transform.position));
                        tile.character = null;
                        
                        Vector3 tilePosition = hit.collider.transform.position;
                        Vector3Int _startPos = Vector3Int.RoundToInt(transform.position);
                        Vector3Int _targetPos = Vector3Int.RoundToInt(tilePosition);
                        
                        InGameFXManager.Inst.TileClickParticle(tilePosition);
                        if(AudioManager.Inst != null)
                            AudioManager.Inst.AudioEffectPlay(2);
                        
                        var finalNodeList =  pathFind.PathFinding(_startPos, _targetPos);
                        moveCoroutine =  StartCoroutine(pathFindAI.MoveAlongPath(finalNodeList)); 
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

    public override void  ResetCharacter()
    {
        if (moveCoroutine != null) 
            StopCoroutine(moveCoroutine);
        base.ResetCharacter();
        firstMove = false;
        isLight= false;
       
        DOVirtual.DelayedCall(1f, () => dissolve.DIssolvessad(false));
    }

    public override void EnterReplayMode()
    {
        ResetCharacter();
        gameObject.SetActive(true);
    }

    public override void InLight()
    {
        
        Tile tile = TileFinding.GetOneTile(transform.position);
        tile.character = this;
        if (tile.isLight)
        {
            Debug.Log("papaDead");
            InGameManager.Inst.moveBlock = true;
            dissolve.DIssolvessad(true);
            CharacterDead();
            DOVirtual.DelayedCall(0.5f, () => dissolve.DIssolvessad(false));
        }
    }
}
