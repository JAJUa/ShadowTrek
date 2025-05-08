using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightManager : Singleton<LightManager>
{
    
    [SerializeField] private Transform interactionGimic, interactionLights,interactionBoth;

    [SerializeField]private List<Dialouge> interactionDialogues;
    [SerializeField] private List<illuminant> illuminants;


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
        LightsOff();
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
    
    

    public void CheckDialougePos()
    {
        foreach (var dialouge in interactionDialogues)
        {
            dialouge.CharacterInInteractPos();
        }
    }

    public void LightsOn()
    {
        foreach (var illuminant in illuminants)
        {
            illuminant.LightOn();
        }
    }

    public void LightsOff()
    {
        foreach (var illuminant in illuminants)
        {
            illuminant.LightOff();
        }
    }

    public void FirstMoveLightAction()
    {
        foreach (var _illuminant in illuminants)
        {
            _illuminant.FirstMoveAction();
        }
    }
    
    
}
