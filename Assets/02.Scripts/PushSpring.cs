using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushSpring : MonoBehaviour
{

    public enum Dir
    {
        front,right,left,back
    }

    public Dir dir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward,out RaycastHit hit, 3f))
        {
            hit.rigidbody.AddForce(transform.forward.normalized * 0.1f,ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position ,transform.forward*3);
    }


}
