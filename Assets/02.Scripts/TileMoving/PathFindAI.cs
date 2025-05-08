using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindAI : MonoBehaviour
{
    [SerializeField] private List<Node> finalNodeList;
    [SerializeField] private float speed;
    private Character character;
    private CharacterRole role;
    private Animator animator;
    public Coroutine corutine;

    public void Init(float _speed,Character _character,CharacterRole _characterRole)
    {
        speed = _speed;
        character = _character;
        role = _characterRole;
        animator = character.animator;
    }

    public void StopMoveCor()
    {
        if(corutine != null)
            StopCoroutine(corutine);
    }
    
     public IEnumerator MoveAlongPath(List<Node> _finalNodeList )
     {
         finalNodeList = _finalNodeList;
         animator.SetBool("isWalk", true);
        for (int passtile = 0; passtile < finalNodeList.Count - 1; passtile++)
        { 
            if(InGameManager.Inst.CurState() == GameState.SeraTurn)
                SeraLightPath(passtile);
            corutine= StartCoroutine(MoveToPosition(new Vector3(finalNodeList[passtile + 1].x, character.transform.position.y, finalNodeList[passtile + 1].z), passtile));
            yield return corutine;
        }
        
        animator.SetBool("isWalk", false);

        InGameManager.Inst.moveBlock = false;
    }

     public void SeraLightPath(int index)
     {
         TileManager.Inst.LightOffAllTiles();
         TileManager.Inst.HideAllTiles();
         var path = MapDataManager.Inst.Data.mapData[MapDataManager.Inst.testMapIndex].seraPath;
         List<Vector3> _tiles = new List<Vector3>();
         List<Tile> lightTiles = new List<Tile>();
         _tiles.Add(path[index]);
         int k = 2;
         int l = 1;
         for (int i = 1; i <= k; i++)
         {
             if (index + i < path.Count)
             {
                 _tiles.Add(path[index+i]);
                 if(i<=l)
                    lightTiles.Add(TileFinding.GetOneTile(path[index+i]));
             }

             if (index - i >= 0)
             {
                 _tiles.Add(path[index - i]);
                 if(i<=l)
                     lightTiles.Add(TileFinding.GetOneTile(path[index-i]));
             }
         }
        
         TileManager.Inst.ShowTiles(_tiles);
         foreach (var tile in lightTiles)
         {
             tile.Light(true);
         }
         var curTile =  TileFinding.GetOneTile(path[index]);
         curTile.Light(true);
         
     }


    private IEnumerator MoveToPosition( Vector3 targetPosition, int passtile)
    {
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
            if(role == CharacterRole.Papa)
                LineRenderer(passtile);
            character.transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / timeToMove));
            character.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, (elapsedTime / (timeToMove / 2.5f)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        character.transform.position = targetPosition;
        //이동이 끝났을 때
        
        character.InLight();
        if(InGameManager.Inst.CurState() == GameState.ShadowTurn && role == CharacterRole.Papa)
            LightManager.Inst.CheckDialougePos();
        if (InGameManager.Inst.CurState() == GameState.ShadowTurn && role == CharacterRole.Sera)
        {
            InGameManager.Inst.player.replay.EraseLine();
        }

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
