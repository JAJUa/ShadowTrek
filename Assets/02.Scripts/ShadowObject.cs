using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowObject : MonoBehaviour
{
    public Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interect()
    {
        rigid = transform.parent.GetComponent<Rigidbody>();
        rigid.AddForce(Vector3.right * 3f,ForceMode.Impulse);
        Debug.Log("act");
    }
}
