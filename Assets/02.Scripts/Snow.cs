using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : MonoBehaviour
{
   public Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce(Vector3.down * 50f, ForceMode.Impulse);
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
