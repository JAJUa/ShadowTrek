using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using VInspector;

public enum CurCharacter
{
    Player, Papa,Enemy,Pet
}
public enum GameState
{
    SeraTurn,ShadowTurn,IdleTurn
}

public class InGameManager : Singleton<InGameManager>
{
    [Tab("InGame")]
    public GameState gameState
    {
        get;
        private set;
    }
    public bool moveBlock = false;
    public CurCharacter curCharacter;
    public Player player; //중앙제어
    public ShadowModePapa papa; //중앙제어
    
    

    private void Awake()
    {
        gameState = GameState.IdleTurn;
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(()=>MapDataManager.Inst);
        foreach (var spawnCharacter in MapDataManager.Inst.Data.mapData[MapDataManager.Inst.testMapIndex].spawnCharacters)
        {
            switch (spawnCharacter.characterRole)
            {
                //addressable로 바꿔야 함
                case CharacterRole.Sera:
                    player = Instantiate(Resources.Load<Player>("Prefab/RealPlayer"),spawnCharacter.spawnPos,quaternion.identity);
                    break;
                case CharacterRole.Papa:
                    papa = Instantiate(Resources.Load<ShadowModePapa>("Prefab/ShadowPapa"),spawnCharacter.spawnPos,quaternion.identity);
                    break;
            }
        }
      
        
        curCharacter = CurCharacter.Player;

        if (papa)
            papa.gameObject.SetActive(false);
        
    }

    public GameState CurState() => gameState;
    

    public void ChangeState(GameState newState)
    {
        StopMoving();
        gameState = newState;
        switch (gameState)
        {
            case GameState.IdleTurn:
                break;
            case GameState.ShadowTurn:
                EnterReplayMode();
                break;
            case GameState.SeraTurn:
                PlayerMove();
                break;
        }
    }

    public void PlayerMove()=> player.CharacterMove();

    
    


    public void AllRestart() //한 맵 전체 리셋
    {
        moveBlock = true;
        FadeInFadeOut.Inst.FadeIn();
        int index = SceneManager.GetActiveScene().buildIndex;
        DOVirtual.DelayedCall(1.5f, () => SceneManager.LoadScene(index));
    }
    

    
    public void EnterReplayMode() //리플레이 모드 진입 시 한 번만 실행
    {
        //리플레이 모드 진입
        gameState = GameState.ShadowTurn;
        LightManager.Inst.LightsOn();
        InGameUIManager.Inst.SpriteChange(false);
        //리플레이 진입시 패스 소환
        PathFind.Inst.NodeSetting();
        curCharacter = CurCharacter.Papa; //캐릭터 전환
        ResetInReplayMode();
    }

    void ResetInReplayMode()
    {
        FadeInFadeOut.Inst.FadeIn();
        StopMoving();
        LightManager.Inst.ResetLights();//ex) LightSHooter의 회전 재정의
        player.EnterReplayMode();
        papa.EnterReplayMode();
        TileManager.Inst.LightOffAllTiles();
        DOVirtual.DelayedCall(1.75f, () => 
        { 
            FadeInFadeOut.Inst.FadeOut(); 
            moveBlock = false; 
            LightManager.Inst.NonDetectActionFinish(); 
        });
    }

    public void ReplayModeRestart()
    {
        //리플레이 모드에서 죽을때 혹은 리스타트
        ResetInReplayMode();
    }

    public void StopMoving()
    {
        if (papa != null)
        {
            if (papa.moveCoroutine != null)
                StopCoroutine(papa.moveCoroutine);
            DOVirtual.DelayedCall(0f, () => papa.lineRenderer.positionCount = 0);
        }

        DOVirtual.DelayedCall(0.1f, () => player.lineRenderer.positionCount = 0); 
        moveBlock = true;
      
        player.animator.SetBool("isWalk", false);
        if (player.moveCoroutine != null)
            StopCoroutine(player.moveCoroutine);

      
    }
    

    public void StayPapa()
    {
        if(!moveBlock)
            OnlyPlayerReplay(true,false);
    }

    public void OnlyPlayerReplay(bool isPapaStay = false,bool lightFinished = false)
    {
        
    }
    


   
    


}
