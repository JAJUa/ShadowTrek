using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class ReciveLight : MonoBehaviour
{
    public Vector3 hitPoint, dir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


   
    public void GetPosDir(Vector3 hitPoint,Vector3 dir)
    {
        this.hitPoint = hitPoint;
        this.dir = dir;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (hitPoint!= null && dir != Vector3.zero)
        {
            Gizmos.DrawRay(hitPoint, dir*10);
        }
    }
}
