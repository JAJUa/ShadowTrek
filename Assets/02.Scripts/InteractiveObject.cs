using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public Vector3 size, pos;
    public InteractiveLights interactiveLight;

    public virtual void AutoLight() { }



    public virtual void ResetObj() { }

    private void OnDrawGizmos()
    {  
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + pos, size);
        
    }
}
