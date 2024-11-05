using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class illuminant : MonoBehaviour
{
    private Light light;
    public List<Vector3> targetTileVector = new List<Vector3>();

    protected virtual void Awake()
    {
        light = GetComponentInChildren<Light>();
    }

    protected virtual void GetTargetTileVector(float offset) //빛 밝힐 타일을 받아옴
    {
        targetTileVector = TileFinding.TargetingTiles(transform, offset);
    }

    public virtual void TargetTileLighting(bool isLight = true)
    {
        List<Tile> lightTiles =  TileFinding.GetTiles(targetTileVector);
        if(lightTiles.Count==0) Debug.Log("타일이 없음");
        
        float intensity = isLight ? 1000 : 0;
        DOVirtual.DelayedCall(0.3f, () =>
        {
            light.DOIntensity(intensity, 0.5f).OnComplete(() => 
            {  
                foreach (var tile in lightTiles) tile.GetLight(isLight);
                InGameManager.Inst.DetectCharacterLight();
            });
        });
        
    }
}
