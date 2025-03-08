using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public enum IlluminantType
{
    always,onAction
}
public class illuminant : MonoBehaviour
{
    [SerializeField]private Light light;
    public IlluminantType illuminantType;
 
    public List<Vector3> targetTileVector = new List<Vector3>();

    protected virtual void Awake()
    {
        if(light == null)
            light = GetComponentInChildren<Light>();
       LightOn(0,0.01f);
    }

    protected virtual void LightOn(float _intensity ,float _duration)
    {
        light.DOIntensity(_intensity, _duration);
    }
    
    public virtual void ResetLight(){}

    protected virtual void GetTargetTileVector(float offset) //빛 밝힐 타일을 받아옴
    {
        targetTileVector = TileFinding.TargetingTiles(transform, offset);
    }

    public virtual void TargetTileLighting(){}
  
    public virtual void TargetTileLighting(bool isLight ,bool action = true)  //action 한 행동으로 판단 할건지 //타일 빛 비추기
    {
        List<Tile> lightTiles =  TileFinding.GetTiles(targetTileVector);
        if(lightTiles.Count==0) Debug.Log("타일이 없음");
        Debug.Log("lamp"+ isLight);
        
        float intensity = isLight ? 1000 : 0;
        foreach (var tile in lightTiles) tile.GetLight(isLight);
        DOVirtual.DelayedCall(0.3f, () =>
        {
            LightOn(intensity,0.5f);
        });
        
    }
    

    public virtual void AllWaysLighting()
    {
        if (illuminantType != IlluminantType.always) return;
     
        
    }
    
}
