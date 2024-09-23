using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class shootLight : MonoBehaviour
{

    private void OnValidate()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        ShootDir();
    }

    [Button]
    public void ShootDir()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
        {
          //  Debug.Log("mirrr");
            Vector3 hitPoint = hit.point;
            Vector3 dir = transform.forward;
            Vector3 reflectDir = Vector3.Reflect(dir, hit.normal);
            hit.transform.GetComponent<ReciveLight>().GetLight();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //ShootDir();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward*100);  
    }
}
