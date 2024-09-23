using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class shootLight : MonoBehaviour
{
    [SerializeField] Vector3 shootDir;
    LineRenderer lineRenderer;
    private void OnValidate()
    {
        
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 3;
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
        if (Physics.Raycast(transform.position, shootDir.normalized, out hit, 100))
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point);
            hit.transform.GetComponentInParent<ReciveLight>().GetLight(lineRenderer);
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
        Gizmos.DrawRay(transform.position, shootDir.normalized*100);  
    }
}
