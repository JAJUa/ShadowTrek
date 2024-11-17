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

    private void Start()
    {
        CollectComponents(interactionGimic,interactionDialogues);
        CollectComponents(interactionBoth,interactionDialogues);
        CollectComponents(interactionLights,illuminants);
        CollectComponents(interactionBoth,illuminants);
        ActionFinish();
    }
    
    void CollectComponents<T>(Transform parent, List<T> components) where T : Component
    {
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
        foreach (var dialouge in interactionDialogues)
        {
            dialouge.CharacterInInteractPos();
        }
        
        DetectCharacterLight();
    }
    
    public void DetectCharacterLight()
    {
        
        foreach (var illuminant in illuminants)
        {
            illuminant.AllWaysLighting();
        }
        TileManager.Inst.SetLightsTile();
        
        InGameManager.Inst.player.InLight();
        if ( InGameManager.Inst.papa != null &&  InGameManager.Inst.papa.gameObject.activeSelf)  InGameManager.Inst.papa.InLight();
    }
}
