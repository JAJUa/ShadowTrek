using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Serialization;
using VInspector;

public class Dialouge : MonoBehaviour
{
    [SerializeField] private List<Tile> interactPosTiles = new List<Tile>();
    [Space(10)]
    [Header("-- Dialouge System --")]   
    private float duration = 0.7f;
    public bool isTutorial;
    public CharacterRole interactRole;
    

    [SerializeField] Color defaultColor, answerColor;
    public Image interBox;
    
    //public Image dialougeBox; 

  

    [Space(10)]
    [Header("-- Collider --")]

    [SerializeField] Vector3 colliderTrans;
    [SerializeField] Vector3 colliderSize;
     [SerializeField]LayerMask tileLayerMask;

    [SerializeField] private RectTransform interTransform, dialoTransform;
    private bool isInterActiveing, isdialoActiveing;
    private Vector2 interAnchor;
    


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + colliderTrans, colliderSize * 2);
    }

    private void Awake()
    {
       int layer = LayerMask.NameToLayer("MoveTile"); // 레이어 번호 가져오기
       tileLayerMask = 1 << layer; //레이어는 비트마스크 형식
    }

    private void GetInteractPosTile() //감지할 타일 가져옴 한 번만 실행
    {
        Collider[] hit = Physics.OverlapBox(transform.position + colliderTrans, colliderSize, Quaternion.identity, tileLayerMask);
        if (hit.Length > 0)
        {
            foreach (var tile in (hit))
            {
                if(tile.transform.parent.TryGetComponent(out Tile tileCs))
                    interactPosTiles.Add(tileCs);
            }
        }
    }

    private void Start()
    {
        UISetting();
        GetInteractPosTile();
    }

    private void UISetting()
    {
        interTransform = interBox.GetComponent<RectTransform>();
        defaultColor = interBox.color;
        interTransform.sizeDelta = new Vector2(4f, 4f);
        Vector2 anchoredPos = interTransform.anchoredPosition;
        anchoredPos.y -= 1f;
        interTransform.anchoredPosition = anchoredPos;
        interAnchor = anchoredPos;
        Color color = interBox.color;
        color.a = 0f;
        interBox.color = color;
        interBox.gameObject.SetActive(false);
    }

    void Update()
    {
        // 마우스 왼쪽 버튼이 클릭되었을 때
        if (Input.GetMouseButtonDown(0) && isInterActiveing)
        {
            if (isdialoActiveing)
            {
                InterFade(true);
            }
            else
            {
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, results);

                if(results.Any(result => result.gameObject == interBox.gameObject))
                {
                    //InterFade(false);
                    Interact();
                    return;
                }

                // Inter Object Click

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform.TryGetComponent(out InteractiveObject interactObj) && hit.transform == transform.parent)
                    {
                        //InterFade(false);
                        Debug.Log("오브젝트를 클릭함");
                        Interact();
                    }
                }

            }
        }
    }

    private void LateUpdate()
    {
        Quaternion camRotation = Camera.main.transform.rotation;
        interBox.transform.rotation = camRotation;
    }


    [Button]
    public void CharacterInInteractPos()
    {
        Debug.Log("감지해야함");
        foreach (var tile in interactPosTiles)
        {
            if (tile.character && tile.character.role ==interactRole )
            {
                InterFade(true); 
                return;
            }
        }
        InterFade(false);
    }


    public void InterFade(bool isFadeIn)
    {
        Debug.Log($"실행 : {isFadeIn}");
        if (isFadeIn) interBox.gameObject.SetActive(isFadeIn);
        
        isInterActiveing = isFadeIn;
        interBox.DOFade(isFadeIn ? 1f : 0f, duration);
        Vector2 targetAnchor = new Vector2(interAnchor.x,interAnchor.y + (isFadeIn?1:0));
        
        interTransform.DOAnchorPosY(targetAnchor.y, duration).SetEase(Ease.InOutSine).OnComplete(() =>
        { 
            interBox.gameObject.SetActive(isFadeIn);
            interTransform.anchoredPosition = targetAnchor;
        });
    }

    public virtual void Interact()
    {
        
    }

    #region 다이얼로그

    /*
   void DialoFade(bool isFadeIn)
   {
       int posX = isFadeIn ? 8 : -8;
       int posY = isFadeIn ? 3 : -3;
       if(isFadeIn) dialougeBox.gameObject.SetActive(true);
       isAnimating = true;
       isdialoActiveing = isFadeIn;
       dialougeBox.DOFade(isFadeIn? 1f : 0f, duration);
       dialoTransform.DOAnchorPos(new Vector2(dialoTransform.anchoredPosition.x + posX, dialoTransform.anchoredPosition.y + posY), duration).SetEase(Ease.InOutSine);
       dialoTransform.DOScale(isFadeIn? Vector3.one:Vector3.zero, duration).SetEase(Ease.InOutSine).OnComplete(() => { isAnimating = false; dialougeBox.gameObject.SetActive(isFadeIn); });
   }*/


    #endregion
   


  
}
