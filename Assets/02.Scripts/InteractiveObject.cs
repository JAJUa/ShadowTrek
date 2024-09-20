using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public Vector3 autoLight, autoLightPos;
    public InteractiveLights interactiveLight;

    public virtual void AutoLight() { }



    public virtual void ResetObj() { } //�ٽý��� �� ������ġ

    public virtual void TurnAction() { }

    private void OnDrawGizmos()
    {  
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + autoLightPos, autoLight);
        
    }
}
