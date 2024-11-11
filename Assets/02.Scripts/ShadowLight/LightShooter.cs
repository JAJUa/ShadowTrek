using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using VInspector;

public class LightShooter : illuminant
{
   [Serializable]
   public struct TargetTileStruct
   {
       public TargetTileStruct(List<Vector3> targetTileVector,int rot)
       {
           this.targetTileVector = targetTileVector;
           this.rot = rot;
       }
       public List<Vector3> targetTileVector;
       public int rot;
   }
   [SerializeField] private List<int> detectIndex = new List<int> { 1, 3, 3 };

   public List<TargetTileStruct> targetTileStruct = new List<TargetTileStruct>();

   [SerializeField] private int testIndex;
    void Start()
    {
        OTT();
    }

    public void OTT()
    {
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
            targetTileStruct.Add(new TargetTileStruct(tileList, Mathf.RoundToInt(yRot)));
            rot += 45;
        }

      
    }

    [Button]
    public void Test()
    {
        transform.DORotate(new Vector3(transform.rotation.eulerAngles.x, targetTileStruct[testIndex].rot, transform.rotation.eulerAngles.z),0.3f).SetEase(Ease.Linear);
        List<Tile> tiles = TileFinding.GetTiles(targetTileStruct[testIndex].targetTileVector);
        Debug.Log(tiles.Count);
        foreach (var tile in tiles)
        {
            tile.GetLight(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
