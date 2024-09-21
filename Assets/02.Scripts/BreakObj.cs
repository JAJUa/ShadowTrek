using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class BreakObj : MonoBehaviour
{
    [SerializeField] Transform parent;
    [SerializeField] List<BoxCollider> fragmentsCol;
    [SerializeField]List<Rigidbody> fragments;
    
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i< parent.childCount; i++)
        {
            fragments.Add(parent.GetChild(i).GetComponent<Rigidbody>());
            fragmentsCol.Add(parent.GetChild(i).GetComponent<BoxCollider>());
        }
    }

    public void GetGravity()
    {
        foreach (Rigidbody rigid in fragments)
        {
            rigid.useGravity= true;
        }
        
        foreach (Collider col in fragmentsCol)
        {
            col.isTrigger= false;
        }

    }

    [Button]
   public void Break()
    {
        GetGravity();
        foreach(Rigidbody rigid in fragments)
        {
            rigid.AddForce(Vector3.down  *3f, ForceMode.Impulse);
        }
    }
}
