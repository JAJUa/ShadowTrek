using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camearDir : MonoBehaviour
{
    public float shadowAngle = 45;
    public float length, size;
    public GameObject mainCam,box;
    Vector3 offset2, direction;

    LineRenderer lineRenderer;
    public List<Vector3> pos = new List<Vector3>();

    private void Awake()
    {
       // Detect();
        lineRenderer = GetComponent<LineRenderer>();
   
    }
    // Start is called before the first frame update
    void Start()
    {
        
      
    }

    // Update is called once per frame
    void Update()
    {
        float angle2 = mainCam.transform.rotation.eulerAngles.y;
        offset2 = new Vector3(Mathf.Cos(-angle2 * Mathf.Deg2Rad), 0, Mathf.Sin(-angle2 * Mathf.Deg2Rad)) * 3f;  // 원형 궤도에서의 위치
        box.transform.position = transform.position+ direction*length/2;
        box.transform.eulerAngles = mainCam.transform.eulerAngles;
        if (Input.GetKeyDown(KeyCode.V))
        {
           // Detect();
        }
       // SetLine();
    }

    void SetLine()
    {
        lineRenderer.positionCount= 0;
        pos.Clear();
        pos.Add(transform.position + offset2);
        pos.Add(transform.position +offset2 +direction * length);
        pos.Add(transform.position - offset2 + direction * length);
        pos.Add(transform.position - offset2);
        lineRenderer.positionCount = pos.Count;

        lineRenderer.SetPositions(pos.ToArray());  // !! 배열로 받아들이는게 가능!

    }

    //  void Detect()
    private void OnValidate()
    {
        direction = Quaternion.Euler(0, shadowAngle, 0) * mainCam.transform.forward;
        Vector3 halfExtents = new Vector3(size / 2, 4f / 2, length / 2);
        float angle2 = mainCam.transform.rotation.eulerAngles.y;
        offset2 = new Vector3(Mathf.Cos(-angle2 * Mathf.Deg2Rad), 0, Mathf.Sin(-angle2 * Mathf.Deg2Rad)) * 3f;  // 원형 궤도에서의 위치
        box.transform.position = transform.position + direction * length / 2;
        box.transform.eulerAngles = mainCam.transform.eulerAngles;
        // TileMoveScript.Inst.TileResearch();
    }
    
 

    private void OnDrawGizmos()
    {
        // Gizmos 색상을 빨간색으로 설정
        Gizmos.color = Color.red;

        // 카메라의 현재 회전 방향에 따라 방향 벡터를 계산
        direction = Quaternion.Euler(0, shadowAngle, 0) * mainCam.transform.forward;
        Vector3 direction2 = Quaternion.Euler(0, -shadowAngle, 0) * mainCam.transform.forward;

    
        Vector3 startPosition = transform.position ;

        // 두 광선을 그리기
        Gizmos.DrawRay(startPosition+offset2, direction * length);
        Gizmos.DrawRay(startPosition- offset2, direction2 * length);




        Quaternion rotation = Quaternion.LookRotation(direction);
        Matrix4x4 originalMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(startPosition + direction * (length / 2), rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(size , 4f, length));
        Gizmos.matrix = originalMatrix;


    }

}
