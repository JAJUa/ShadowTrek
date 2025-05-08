using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using VInspector;

public class LightShooter : illuminant
{
   [Serializable]
   public struct TargetTileStruct
   {
       public TargetTileStruct(List<Tile> targetTile,int rot)
       {
           this.targetTile = targetTile;
           this.rot = rot;
       } 
       public List<Tile> targetTile;
       public int rot;
   }
   
   [SerializeField] private List<int> detectIndex = new List<int> { 1, 3, 3 };

   public List<TargetTileStruct> targetTileStruct = new List<TargetTileStruct>();
   
   [SerializeField] private int curIndex;
   [Tooltip("45도로 돌아가는 시간")][SerializeField] private float rotSpeed = 1f;
   private int beginIndex = 0;
   private bool clockWise = true;
   private Coroutine turnCor;


   private void Start()
   {
       LightOff();
   }
   
   public override void LightOn()
   {
       beginIndex = curIndex;
       illuminantType = IlluminantType.always;
       base.LightOn();
       Setting();
   }

    public void Setting()
    {
        if(TileManager.Inst.mapTiles.Count==0) Debug.Log("타일매니저에 타일이 없음");
        int rot = 0;
        while (rot < 360)
        {
            List<Vector3> tileList = new List<Vector3>();
            bool isRightAngle; //직각인가 => 90도 인가
            float yRot = rot;// 현재 오브젝트의 Y축 회전값
           
            isRightAngle = Mathf.RoundToInt(yRot) % 90 == 0 ? true : false;
            float distance = isRightAngle? 15f :15f* Mathf.Sqrt(2) ; 
            
            float radian = yRot * Mathf.Deg2Rad;  // 각도를 라디안으로 변환
            Vector3 forwardDirection = new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));  // 회전된 방향 벡터

            float rotAngle = isRightAngle ? 90 : 45;
            Vector3 leftDirection = Quaternion.Euler(0, -rotAngle, 0) * forwardDirection; 
            Vector3 rightDirection = Quaternion.Euler(0, rotAngle, 0) * forwardDirection;
            Vector3Int targetPosition;
            Vector3Int leftPosition,rightPosition;
            if (!isRightAngle)
            {
                targetPosition =  Vector3Int.RoundToInt(transform.position + forwardDirection * (distance));
                for (int i = 1; i < 2+1; i++)
                {
                    
                    leftPosition = Vector3Int.RoundToInt(targetPosition + leftDirection *  (15*i) );
                    rightPosition = Vector3Int.RoundToInt(targetPosition + rightDirection * (15*i) );
                    tileList.Add(leftPosition);
                    tileList.Add(rightPosition);
                    tileList.Add(targetPosition + forwardDirection * (distance*(i-1)));
                }

            }
            else
            {
                for (int i = 0; i < detectIndex.Count; i++)
                {
                    targetPosition = Vector3Int.RoundToInt(transform.position + forwardDirection * (distance*(i+1)));
                    tileList.Add(targetPosition);
                    if (detectIndex[i] >= 3)
                    {
                        int halfIndex = detectIndex[i] / 2;
                        for (int j = 0; j < halfIndex; j++)
                        {
                            leftPosition = Vector3Int.RoundToInt(targetPosition + leftDirection * (distance*(j+1))); // 왼쪽 한 칸 위치 계산
                            rightPosition = Vector3Int.RoundToInt(targetPosition + rightDirection * (distance*(j+1))); // 오른쪽 한 칸 위치 계산
                            tileList.Add(leftPosition);
                            tileList.Add(rightPosition);
                        }
                    }
                }
            }
            targetTileStruct.Add(new TargetTileStruct(TileFinding.GetTiles(tileList), Mathf.RoundToInt(yRot)));
            rot += 45;
        }

      
    }

    [Button]
    public void ChangeDir()
    {
        DOTween.Kill(transform);
        if(turnCor!=null) StopCoroutine(turnCor);
        clockWise = !clockWise;
        turnCor =  StartCoroutine(Turning());
    }
    public override void FirstMoveAction()
    {
        turnCor =  StartCoroutine(Turning());
    }

    public IEnumerator Turning()
    {
        while (true)
        {
            TargetTileLighting();
            yield return new WaitForSeconds(rotSpeed + 0.051f);
        }
    }

    [Button]
    public override void TargetTileLighting() //빛 돌아가게 하는거
    {
        if(clockWise)
            curIndex = ++curIndex >= targetTileStruct.Count ? 0 : curIndex;
        else
            curIndex = --curIndex < 0 ? targetTileStruct.Count-1 : curIndex;
        int lastIndex = 0;
        if(clockWise) lastIndex =curIndex-1<0 ? targetTileStruct.Count-1:curIndex-1; //최근 인덱스
        else  lastIndex =curIndex+1>=targetTileStruct.Count ? 0:curIndex +1;
        
        // 회전 각도 계산
        float deltaAngle = Mathf.DeltaAngle(transform.rotation.eulerAngles.y,targetTileStruct[curIndex].rot); // -180 ~ 180 범위
        float distance = Mathf.Abs(deltaAngle);
    
        // 전체 45도 기준 회전 시간 비례 계산
        float duration = rotSpeed * (distance / 45f);
       // Debug.Log(duration);
        transform.DORotate(new Vector3(transform.rotation.eulerAngles.x, targetTileStruct[curIndex].rot, transform.rotation.eulerAngles.z),duration).SetEase(Ease.Linear)
            .OnComplete(()=>
        {
            List<Tile> lastTiles =  targetTileStruct[lastIndex].targetTile;
            List<Tile> tiles = targetTileStruct[curIndex].targetTile;
            HashSet<Tile> allTileSet = new HashSet<Tile>(lastTiles);
            allTileSet.UnionWith(tiles);
            foreach (var lastTile in lastTiles)
            {
                lastTile.GetLight(false);
            }

            foreach (var tile in tiles)
            {
                tile.GetLight(true);
              
            }

            DOVirtual.DelayedCall(0.01f, () =>
            {
                Debug.Log("딜레이");
                foreach (var tile in allTileSet)
                {
                    tile.SetLight();
                }

            });
            
        });
       
        
    }

    public override void ResetLight()
    {
        AllTileLightOff();
        clockWise = true;
        if(turnCor!=null)
            StopCoroutine(turnCor);
        DOTween.Kill(transform);
        curIndex = beginIndex;
        transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, targetTileStruct[curIndex].rot,
            transform.rotation.eulerAngles.z);
    }

    private void AllTileLightOff()
    {
        foreach (var _targetTileStruct in targetTileStruct)
        {
            foreach (var tile in _targetTileStruct.targetTile)
            {
                tile.GetLight(false);
            }
        }
    }

    public override void AllWaysLighting()
    {
        base.AllWaysLighting();
        //TargetTileLighting();
    }
}
