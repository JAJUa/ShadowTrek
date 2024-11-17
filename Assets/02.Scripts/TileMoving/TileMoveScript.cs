using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;


#region Node Class
[System.Serializable]
public class Nodee
{
    public Nodee(bool _isWall, int _x, int _z) { isWall = _isWall; x = _x; z = _z; }

    public bool isWall;
    public bool isCliff;
    public bool isPlayer;
    public Nodee ParentNode;

    public int x, z, G, H;
    public int F { get { return G + H; } }
}
#endregion

public class TileMoveScript : MonoBehaviour
{
    #region Variable settings
    public static TileMoveScript Inst;
    [HideInInspector]public bool checkWall;
    PathFind pathfind;

    [Space(10)]
    [Header("-- GridSetting --")]
    public Vector3Int bottomLeft, topRight;
    private List<GameObject> tiles = new List<GameObject>(); //A* 타일임 

    [FormerlySerializedAs("interactionLight")] [FormerlySerializedAs("interactions")] [SerializeField] private Transform interactionGimic;
    [SerializeField]private List<Dialouge> interactionDialogues;

    #endregion
    private void Awake()
    {
        Inst = this;
        pathfind = new PathFind(bottomLeft, topRight, 15, true, true, LayerMask.GetMask("Ground"));
        pathfind.FindPath(bottomLeft, topRight);

        for (int i = 0; i < interactionGimic.childCount; i++)
        {
            Dialouge _dialouge = interactionGimic.GetChild(i).GetComponentInChildren<Dialouge>();
            if (_dialouge)interactionDialogues.Add(_dialouge);
        }
        
     

    }     
    

    private void Start()
    {

        checkWall = true;

    }

   

    public void TileResearch()
    {
        checkWall = false;
        pathfind.FindPath(bottomLeft, topRight);
        Debug.Log(pathfind.NodeArray.Length);
        int i = 0;
        foreach (Nodee node in pathfind.NodeArray)
        {

            if (node.isWall)
            {
                tiles[i].SetActive(false);
            }             
            else
                tiles[i].SetActive(true);
            i++;
        }
        checkWall = true;
    }


    public IEnumerator DealayTileResearch()
    {
        yield return new WaitForSeconds(0.05f);
        TileResearch();
    }

    #region PlayerFunction


    // LineRenderer
    public void LineRenderer(GameObject character,PathFind pathfind,int passtile)
    {
        List<Nodee> FinalNodeList = pathfind.FinalNodeList;
        GameObject mainCharacter = character;
        LineRenderer lineRenderer;
        lineRenderer = mainCharacter.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;

      
        if (FinalNodeList.Count - passtile > 1)
        {
            lineRenderer.positionCount = FinalNodeList.Count - passtile;
            lineRenderer.SetPosition(0, new Vector3(mainCharacter.transform.position.x, 2.7f, mainCharacter.transform.position.z));
            //Debug.Log(lineRenderer);
            lineRenderer.SetPosition(1, new Vector3(FinalNodeList[passtile + 1].x, 2.7f, FinalNodeList[passtile + 1].z));

            for (int i = 2; i < FinalNodeList.Count; i++)
            {
                if (passtile + i < FinalNodeList.Count)
                    lineRenderer.SetPosition(i, new Vector3(FinalNodeList[passtile + i].x, 2.7f, FinalNodeList[passtile + i].z));
            }
        }
    }

    public IEnumerator MoveAlongPath(GameObject character,Animator animator,PathFind pathFind, float moveSpeed,CurCharacter characterRole,List<PointInTime> pointsInTime)
    {
       
        List<Nodee> FinalNodeList = pathFind.FinalNodeList;
        animator.SetBool("isWalk", true);
        for (int passtile = 0; passtile < FinalNodeList.Count - 1; passtile++)
        {
            // RePlay 리플레이 모드면 실행
            if (InGameManager.Inst.inRelpayMode)
            {
                RePlay.Inst.ReMove(false);
            }

            if (characterRole == CurCharacter.Papa)
            {
                if (FinalNodeList[passtile + 1].isPlayer)   //플레이어 발견 디버깅
                {
                    Debug.Log("플레이어 발견");
                    GameObject player = GameObject.FindGameObjectWithTag("PlayerControl");
                    Transform trans = player.transform.Find("PetTransform").GetComponent<Transform>();
                    yield return StartCoroutine(MoveToPosition(character,new Vector3(trans.position.x, character.transform.position.y, trans.position.z), moveSpeed,pathFind,passtile,characterRole,pointsInTime));
                }
                else
                    yield return StartCoroutine(MoveToPosition(character,new Vector3(FinalNodeList[passtile + 1].x, character.transform.position.y, FinalNodeList[passtile + 1].z),moveSpeed,pathFind,passtile,characterRole,pointsInTime));
            }         
            else
            {
                yield return StartCoroutine(MoveToPosition(character, new Vector3(FinalNodeList[passtile + 1].x, character.transform.position.y, FinalNodeList[passtile + 1].z), moveSpeed, pathFind, passtile, characterRole, pointsInTime));
                
            }
               

        }


      
        animator.SetBool("isWalk", false);

        InGameManager.Inst.moveBlock = false;

    }

    


    private IEnumerator MoveToPosition(GameObject character, Vector3 targetPosition, float moveSpeed, PathFind pathFind, int passtile,CurCharacter characterRole,List<PointInTime> pointsInTime)
    {
        //TurnAction();
        // Position
        Vector3 startPosition = character.transform.position;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float timeToMove = distance / moveSpeed;

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
            if(name != "enemy")
                LineRenderer(character, pathFind,passtile);
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
    #endregion

    



}