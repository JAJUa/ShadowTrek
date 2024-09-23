using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;
using VInspector;

public class ReciveLight : MonoBehaviour
{
    public Vector3 hitPoint, dir,gPos;
    [SerializeField] LayerMask groundMask,tileMask;
    [SerializeField] int rotIndex;
    [SerializeField] Vector3[] rotDirs;
    [SerializeField] Transform point;
    [SerializeField] Material tileLightColor, defaultTileMaterial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnValidate()
    {
        if ( rotDirs[rotIndex] != Vector3.zero)
        {
            RaycastHit hit;
            if (Physics.Raycast(point.position, rotDirs[rotIndex], out hit, 100, groundMask))
            {
                gPos = hit.point;
                // Debug.Log("¶¥¿¡ ´êÀ½");

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       
       
    }

    public void CheckLight()
    {
        if (rotDirs[rotIndex] != Vector3.zero)
        {
            RaycastHit hit;
            if (Physics.Raycast(point.position, rotDirs[rotIndex], out hit, 100, groundMask))
            {
                gPos = hit.point;
                Debug.Log("¶¥¿¡ ´êÀ½");
                Collider[] colliders = Physics.OverlapSphere(gPos, 5, tileMask);

                if (colliders.Length > 0)
                {
                    Debug.Log("°¨ÁöÇÔ");
                    foreach (Collider target in colliders)
                    {
                        Renderer renderer = target.GetComponent<Renderer>();
                        renderer.material = tileLightColor;          
                    }
                }

            }
        }
    }


   
    public void GetLight()
    {
        CheckLight();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (point!= null && rotDirs[rotIndex] != Vector3.zero)
        {
            Gizmos.DrawRay(point.position, rotDirs[rotIndex].normalized*100);
        }

        Gizmos.color = Color.yellow;
        if (gPos != null)
        {
            Gizmos.DrawWireSphere(gPos, 10);
        }
    }
}
