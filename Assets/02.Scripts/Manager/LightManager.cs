using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static LightManager Inst;
    
    [SerializeField] private Transform interactionGimic, interactionLights,interactionBoth;

    [SerializeField]private List<Dialouge> interactionDialogues;
    [SerializeField] private List<illuminant> illuminants;
    private void Awake()
    {
        Inst = this;
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(()=>InGameManager.Inst && MapPrefabData.Inst);
        MapPrefabData mapPrefabData = MapPrefabData.Inst;
        interactionGimic = mapPrefabData.interactionGimic; 
        interactionLights = mapPrefabData.interactionLights;
        interactionBoth = mapPrefabData.interactionBoth;
        
        CollectComponents(interactionGimic,interactionDialogues);
        CollectComponents(interactionBoth,interactionDialogues);
        CollectComponents(interactionLights,illuminants);
        CollectComponents(interactionBoth,illuminants);
        yield return new WaitUntil(()=>TileManager.Inst.mapTiles.Count>0);
        //ActionFinish();
    }
    
    void CollectComponents<T>(Transform parent, List<T> components) where T : Component
    {
        if (parent.childCount == 0) return;
        
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
        
            if (child.TryGetComponent(out T component))
            {
                components.Add(component);
            }
            else
            {
                T t = child.GetComponentInChildren<T>();
                if(t) components.Add(t);
            }
        }
    }

    public void ResetLights()
    {
        foreach (var illuminant in illuminants)
        {
            illuminant.ResetLight();
        }
    }

    public void ActionFinish() //한 행동이 끝났을 때
    {
        Debug.Log("ActionFinish");
        DetectCharacterLight();
       CheckDialougePos();
        
       
    }
    
    public void NonDetectActionFinish() //한 행동이 끝났을 때 하지만 캐릭터들 InLight 안시킴
    {
        Debug.Log("ActionFinish");
        SetLights();
        CheckDialougePos();
        
       
    }


    public void CheckDialougePos()
    {
        foreach (var dialouge in interactionDialogues)
        {
            dialouge.CharacterInInteractPos();
        }
    }

    private void SetLights()
    {
        Debug.Log("SetLights");
        foreach (var illuminant in illuminants)
        {
            illuminant.AllWaysLighting();
        }
        TileManager.Inst.SetLightsTile();
    }
    
    public void DetectCharacterLight()
    {
        SetLights();
      
        
        InGameManager.Inst.player.InLight();
        if ( InGameManager.Inst.papa != null &&  InGameManager.Inst.papa.gameObject.activeSelf)  InGameManager.Inst.papa.InLight();
    }
}
