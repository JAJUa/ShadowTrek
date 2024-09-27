using UnityEngine;
using System.Linq;
using VInspector;
using System.Collections.Generic;

public class InteractiveLights : MonoBehaviour
{
    public enum LightType {RotLight,StayLight,OnOffLight }
    public LightType lightType;
    public Light spotlight;
    public LayerMask detectionLayerMask;
    [SerializeField] Material tileLightColor, defaultTileMaterial;
    [SerializeField] private bool isDetectCollider, startTurnOn;
    [SerializeField] float detectTileCount;
    float detectLength;
    [SerializeField] private List<Collider> tiles= new List<Collider>();
    [ShowIf("isDetectCollider")]
    [SerializeField] Vector3 colliderSize, colliderPos;

    [Header("테스트")]
    Vector3 dir;
    float length;

    private void Awake()
    {
        detectLength = 13 * ++detectTileCount;
        detectionLayerMask = LayerMask.GetMask("Player");
       detectionLayerMask += LayerMask.GetMask("Papa");
    }

    private void Start()
    {
        transform.gameObject.SetActive(startTurnOn);
        if (startTurnOn) ChangeTileColor();
    }
  

    [Button]
    public void Detect()
    {

        if (transform.gameObject.activeSelf)
        {
            if (!isDetectCollider)
            {
                Collider[] targetsInViewRadius = Physics.OverlapSphere(spotlight.transform.position, spotlight.range, detectionLayerMask);
                if (targetsInViewRadius.Length > 0)
                {
                    foreach (Collider target in targetsInViewRadius)
                    {
                       

                        Vector3 dirToTarget = (target.transform.position - spotlight.transform.position);
                        float angleBetween = Vector3.Angle(spotlight.transform.forward, dirToTarget);
                        // Spotlight 타겟 시야각 범위 감지
                        if (angleBetween < spotlight.spotAngle / 2 && CheckAngle(target,dirToTarget,angleBetween))
                        {
                            if(!Physics.Raycast(transform.position,target.transform.position - transform.position,Vector3.Distance(transform.position, target.transform.position), LayerMask.GetMask("Wall")))
                            {
                                dir = target.transform.position - transform.position;
                                length = Vector3.Distance(transform.position, target.transform.position);
                                if (target.TryGetComponent(out Character character))
                                {
                                    character.isLight = true;
                                    Debug.Log($"Object {target.name} is within the spotlight's range and angle.");
                                }
                            }
                       

                            Debug.DrawLine(spotlight.transform.position, target.transform.position, Color.red);
                        }
                    }
                }
                ChangeTileColor();

            }
            else
            {

                Collider[] colliders = Physics.OverlapBox(transform.position + colliderPos, colliderSize / 2, Quaternion.identity, detectionLayerMask);
                if (colliders.Length > 0)
                {
                    foreach (Collider collider in colliders)
                    {
                        collider.GetComponent<Character>().isLight = true;


                    }
                }
            }
            //ChangeTileColor();

        }
    
    }

    public void TileColorDefault()
    {
        if (tiles.Count > 0)
        {
            Debug.Log("타일 정상화");
            foreach (Collider tile in tiles)
            {
                Renderer renderer = tile.GetComponent<Renderer>();
                renderer.material = defaultTileMaterial;
            }
        }
    }

    public void ChangeTileColor()
    {
        if (!isDetectCollider)
        {
             TileColorDefault();
            tiles.Clear();
            Collider[] colliders = Physics.OverlapSphere(spotlight.transform.position, spotlight.range, LayerMask.GetMask("MoveTile"));
          
            if(colliders.Length > 0)
            {
                foreach (Collider target in colliders)
                {
                    // Spotlight 타겟 방향 계산
                    Vector3 dirToTarget = (target.transform.position - spotlight.transform.position);
       

                    // Spotlight 타겟 각도 계산
                    float angleBetween = Vector3.Angle(spotlight.transform.forward, dirToTarget);
                 
                    // Spotlight 타겟 시야각 범위 감지
                    if (angleBetween < spotlight.spotAngle / 2 && CheckAngle(target,dirToTarget,angleBetween))
                    {
                        dir = target.transform.position - transform.position;
                        length = Vector3.Distance(transform.position, target.transform.position);
                        if (!Physics.Raycast(transform.position, target.transform.position - transform.position, Vector3.Distance(transform.position, target.transform.position), LayerMask.GetMask("Wall")))
                        {
                            Renderer renderer = target.GetComponent<Renderer>();
                            if (renderer.material.color != tileLightColor.color) //나중에 수정 예정
                            {
                                tiles.Add(target);
                            }
                           
                           
                            renderer.material = tileLightColor;
                        }
                        //Debug.DrawLine(spotlight.transform.position, target.transform.position, Color.red);
                    }
                }
            }
        }
        else
        {
            if (gameObject.activeSelf)
            {
                tiles.Clear();
                Collider[] colliders = Physics.OverlapBox(transform.position + colliderPos, colliderSize / 2, Quaternion.identity, LayerMask.GetMask("MoveTile"));
                if(colliders.Length> 0)
                    tiles.AddRange(colliders);
                if (tiles.Count > 0)
                {
                    int i = 1;
                    foreach (Collider collider in tiles)
                    {
                        Renderer renderer = collider.GetComponent<Renderer>();
                        renderer.material = tileLightColor;
                        i++;
                    }
                    Debug.Log("켜짐");
                }
            }
            else
            {
                foreach (Collider collider in tiles)
                {
                    Debug.Log(" 사라짐");
                    Renderer renderer = collider.GetComponent<Renderer>();
                    renderer.material = defaultTileMaterial;
                }
            }
           
        }
       

        
    }

    bool CheckAngle(Collider target, Vector3 dirToTarget, float angleBetweenSpotlightAndTarget)
    {
        bool check = true;
        // Spotlight 타겟 방향 계산
       // Debug.Log(Vector3.Angle(spotlight.transform.forward, dirToTarget));
        if (Vector3.Distance(target.transform.position, spotlight.transform.position) > detectLength)
        {
           // Debug.Log("넘치는 length");
            check = false;
        }

        // Spotlight 타겟 각도 계산
        if (angleBetweenSpotlightAndTarget >= 36)
        {
          //  Debug.Log(angleBetweenSpotlightAndTarget);
          //  Debug.Log("넘치는 Angle");
            check = false;
        }
        return check;
    }

    
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, dir * length);
        if (isDetectCollider)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position + colliderPos, colliderSize);
        }
        else
        {
            Gizmos.DrawRay(transform.position, transform.forward * detectLength);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spotlight.transform.position, spotlight.range);
        }
          
    }
}
