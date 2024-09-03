using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Pet : Character
{


    SkinnedMeshRenderer skinnedMeshRenderer;
    Material[] materials,defaultMat;




    [Space(10), Header("Pet Skill")]

    //Invisable Skill
    [SerializeField] Material invisableMat;
    [SerializeField] float InvTime;
    [SerializeField] bool isInv;

    //Scartch Grass
    [SerializeField] GameObject scartchObj;
    List<GameObject> scartchs = new List<GameObject>();
    [SerializeField] bool isSkill;
    float curInvTime = 0;
    
    // Start is called before the first frame update

    private void Awake()
    {
        skinnedMeshRenderer= GetComponentInChildren<SkinnedMeshRenderer>();
        materials= skinnedMeshRenderer.materials;
        defaultMat = skinnedMeshRenderer.materials;
        animator= GetComponent<Animator>();

        for(int i = 0; i < 4; i++)
        {
            GameObject scartch=Instantiate(scartchObj);
            scartchs.Add(scartch);

            scartch.SetActive(false);
        }
    }
    public override void Start()
    {
        base.Start();
        pointInTime = new List<PointInTime>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        DestroyOnGrass();
        DestroyGrass();
        InvisablePet();

        if(curInvTime > 0 && isInv)
        {
            curInvTime -= Time.deltaTime;
        }
        else if(curInvTime <=0 && isInv)
        {
            isInv= false;
            ChangeInvMaterial(isInv);
        }      
    }

    void Move()
    {
        if (Input.GetMouseButtonDown(0) && IsCharacterTurn() && !InGameManager.Inst.moveBlock)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("MoveTile") && !Physics.Raycast(hit.transform.position, Vector3.up, 3f)) 
                    {
                        Time.timeScale = 1;

                        Vector3 tilePosition = hit.collider.transform.position;

                        Vector3Int startPos = Vector3Int.RoundToInt(transform.position);
                        Vector3Int targetPos = Vector3Int.RoundToInt(tilePosition);



                        InGameFXManager.Inst.TileClickParticle(tilePosition);
                        pathFind.FindPath(startPos, targetPos);
                        Debug.Log(pathFind.FinalNodeList.Count);
                        StartCoroutine(tileMove.MoveAlongPath(gameObject,animator, pathFind, moveSpeed,curCharacter,pointInTime));
                    }
                }
            }
            else
            {
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, results);

                foreach (RaycastResult result in results)
                {
                    Debug.Log("Hit " + result.gameObject.name);
                }
            }
        }
    } //이동

    public void InvisablePet() //무적 스킬
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            

                skinnedMeshRenderer.materials = materials;
                curInvTime = InvTime;
                isInv = true;
                ChangeInvMaterial(isInv);

            
        }
      
    }

    void ChangeInvMaterial(bool isInvisable)
    {
        if (isInvisable)
        {
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = invisableMat;
            }
        }
        else
        {
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = defaultMat[i];
            }
        }
        skinnedMeshRenderer.materials = materials;
    } //펫을 투명색으로

   
    public void DestroyOnGrass() // 스크래치 표시 활성화
    { 
        if (Input.GetKeyDown(KeyCode.P) && IsCharacterTurn())
        {
            int scartchIndex = 0;
            Collider[] destroyTile = Physics.OverlapBox(transform.position, new Vector3(22.5f, 22.5f, 22.5f));
            foreach (Collider tile in destroyTile)
            {
                if (tile.CompareTag("DestroyTile"))
                {
                    scartchs[scartchIndex].transform.position = tile.transform.position + new Vector3(0, 10f, 0);
                    scartchs[scartchIndex].gameObject.SetActive(true);
                    scartchIndex++;
                    isSkill = true;
                }
            }
        }
    }


    void DestroyGrass() //클릭 시 풀 사라짐
    {
        if (Input.GetMouseButtonDown(0) && isSkill)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("DestroyTile"))
                {
                    isSkill= false;
                    Destroy(hit.collider.gameObject);       

                    foreach (GameObject scartch in scartchs)
                    {
                        scartch.gameObject.SetActive(false);
                    }

                }
            }
            StartCoroutine(TileMoveScript.Inst.DealayTileResearch());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(45, 45, 45));
    }
}
