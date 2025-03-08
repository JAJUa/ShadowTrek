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

    public enum Type { text, ClickLever, LampRotation };
    public Type type;

    [SerializeField] Color defaultColor, answerColor;
    public Image interBox;

    [ShowIfEnum("type", (int)Type.text)]
    public Image dialougeBox; 

  

    [Space(10)]
    [Header("-- Collider --")]

    [SerializeField] Vector3 colliderTrans;
    [SerializeField] Vector3 colliderSize;
     [SerializeField]LayerMask tileLayerMask;

    [SerializeField] private RectTransform interTransform, dialoTransform;
    private bool isInterActiveing, isdialoActiveing, isAnimating, onColider;
    private Vector2 interAnchor;
    


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + colliderTrans, colliderSize * 2);
    }

    private void Awake()
    {
       GetInteractPosTile();
       int layer = LayerMask.NameToLayer("MoveTile"); // 레이어 번호 가져오기
       tileLayerMask = 1 << layer; //레이어는 비트마스크 형식
    }

    private void GetInteractPosTile() //감지할 타일 가져옴
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
        defaultColor = interBox.color;
        interTransform = interBox.GetComponent<RectTransform>();
        interTransform.sizeDelta = new Vector2(4, 4);
        interTransform.anchoredPosition = new Vector2(interTransform.anchoredPosition.x, interTransform.anchoredPosition.y - 1);
        interBox.color = new Color(interBox.color.r, interBox.color.g, interBox.color.b, 0f);
        interBox.transform.rotation = Camera.main.transform.rotation;
        interBox.gameObject.SetActive(false);
        interAnchor = interTransform.anchoredPosition;

        if (type == Type.text)
        {
            dialoTransform = dialougeBox.GetComponent<RectTransform>();
            dialoTransform.anchoredPosition = new Vector2(dialoTransform.anchoredPosition.x - 8, dialoTransform.anchoredPosition.y - 3);

            dialougeBox.color = new Color(dialougeBox.color.r, dialougeBox.color.g, dialougeBox.color.b, 0f);
            dialougeBox.transform.localScale = Vector3.zero;
            dialougeBox.gameObject.SetActive(false);
        }

    }

    void Update()
    {
        // 마우스 왼쪽 버튼이 클릭되었을 때
        if (Input.GetMouseButtonDown(0) && isInterActiveing && !isAnimating)
        {
            if (isdialoActiveing)
            {
                InterFade(true);
                if (type == Type.text) DialoFade(false);
                return;
            }
            else
            {
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, results);

                if(results.Any(result => result.gameObject == interBox.gameObject))
                {
                    InterFade(false);
                    if (type == Type.text) DialoFade(true);
                    Interact();
                    return;
                }

                // Inter Object Click

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform.TryGetComponent(out InteractiveObject interactObj) && hit.transform == transform.parent)
                    {
                        InterFade(false);
                        if (type == Type.text) DialoFade(true);
                        Interact();
                        return;
                    }
                }

            }
        }
    }


    [Button]
    public void CharacterInInteractPos()
    {
        Debug.Log("감지해야함");
        foreach (var tile in interactPosTiles)
        {
            if (tile.character && tile.character.role ==interactRole )
            {
                Debug.Log(tile.transform.position);
                InterFade(true); 
                Debug.Log("캐릭터 있음"); 
                return;
            }
            else InterFade(false);
        }
    }


    public void InterFade(bool isFadeIn)
    {
        interBox.color = InGameManager.Inst.isAnswering ? answerColor: defaultColor;
        
        if (isFadeIn) interBox.gameObject.SetActive(isFadeIn);

        isAnimating = true;
        isInterActiveing = isFadeIn;
        interBox.DOFade(isFadeIn ? 1f : 0f, duration);
        Vector2 targetAnchor = new Vector2(interAnchor.x,interAnchor.y + (isFadeIn?1:0));
        interTransform.DOAnchorPosY(targetAnchor.y, duration).SetEase(Ease.InOutSine).OnComplete(() =>
        { 
            isAnimating = false; 
            interBox.gameObject.SetActive(isFadeIn);
            interTransform.anchoredPosition = targetAnchor;
        });
    }

    //이거 수정 필요
    private void LateUpdate()
    {
        interBox.transform.rotation = Camera.main.transform.rotation;
    }

    public virtual void Interact()
    {
      

        if (isTutorial)
            TutorialManager.Inst.FinshTutorial();
        InGameManager.Inst.OnlyPlayerReplay(true);

        if (!InGameManager.Inst.isAnswering)
            DOVirtual.DelayedCall(0.8f, () => InterFade(true));

    }

    public virtual void InteractTutorial()
    {
        isTutorial= true;
    }

    public void AnswerDialogue()
    {
        Debug.Log("인터렉트 하셈!");
        InterFade(true); 
    }

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
    }



  
}
