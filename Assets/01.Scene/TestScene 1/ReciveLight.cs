using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] float turnSpeed, turnAngle;
    [SerializeField] Transform rotatingObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnValidate()
    {
        float angle = rotIndex * 45;
        Vector3 newRotation = rotatingObj.eulerAngles;
        newRotation.y = angle; // 원하는 각도 값으로 대체
        rotatingObj.eulerAngles = newRotation;
        Vector3 target = new Vector3(rotatingObj.eulerAngles.x, angle, rotatingObj.eulerAngles.z);
       
        if ( rotDirs[rotIndex] != Vector3.zero)
        {
            RaycastHit hit;
            if (Physics.Raycast(point.position, rotDirs[rotIndex], out hit, 100, groundMask))
            {
                gPos = hit.point;
                // Debug.Log("���� ����");

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       
       
    }

    public void CheckLight(LineRenderer laser)
    {
        if (rotDirs[rotIndex] != Vector3.zero)
        {
            RaycastHit hit;
            if (Physics.Raycast(point.position, rotDirs[rotIndex], out hit, 100, groundMask))
            {
                gPos = hit.point;
                Collider[] colliders = Physics.OverlapSphere(gPos, 5, tileMask);
                laser.SetPosition(2,gPos);
                if (colliders.Length > 0)
                {
                    foreach (Collider target in colliders)
                    {
                        Renderer renderer = target.GetComponent<Renderer>();
                        renderer.material = tileLightColor;          
                    }
                }

            }
        }
    }


   
    public void GetLight(LineRenderer laser)
    {
        CheckLight(laser);
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
