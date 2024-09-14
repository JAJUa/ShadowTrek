using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;
using VInspector;

public class Dialouge : MonoBehaviour
{
    [Space(10)]
    [Header("-- Dialouge System --")]

  
    public float duration = 0.7f; // 애니메이션 지속 시간
    public Canvas pictureCanvas;
    public bool isTutorial;
    public enum Type {text,takeObj,picture,ClickCutScene,ClickLever,LampRotation};
    public enum ObjType { key };
    public Type type;


    public Image interBox;// 투명해지고 이동할 이미지

    [ShowIfEnum("type", (int)Type.text)]
    public Image dialougeBox; // 투명해지고 이동할 이미지

    [Foldout("Take key")]
    [ShowIfEnum("type", (int)Type.takeObj)]
    public ObjType objType;
    [ShowIfEnum("objType", (int)ObjType.key)]
    [SerializeField] int keyIndex;

    [Foldout("Picture")]
    [ShowIfEnum("type", (int)Type.picture)]
    [SerializeField] Sprite picture;
    [ShowIfEnum("type", (int)Type.picture)]
    [SerializeField][TextArea] string message;
    TextMeshProUGUI inforText;

    [Foldout("ClickCutScene")]
    [ShowIfEnum("type", (int)Type.ClickCutScene)]
    [SerializeField] GameObject cutSceneObj;

    [Foldout("ClickLever")]
    [ShowIfEnum("type", (int)Type.ClickLever)]
    [SerializeField] GameObject lever;

    [Foldout("RampRot")]
    [ShowIfEnum("type", (int)Type.LampRotation)]
    [SerializeField] GameObject lamp;



 


    [Space(10)] [Header("-- Collider --")]

    [SerializeField] Vector3 colliderTrans;
    [SerializeField] Vector3 colliderSize;
    [SerializeField] LayerMask layerMask;

    private RectTransform interTransform,dialoTransform;
    [SerializeField] private bool isInterActiveing, isdialoActiveing, isAnimating, onColider;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position +colliderTrans, colliderSize * 2);
    }

    private void Awake()
    {
       layerMask += LayerMask.GetMask("Papa");
    }

    private void Start()
    {
        
        interTransform = interBox.GetComponent<RectTransform>(); 
        interTransform.anchoredPosition = new Vector2(interTransform.anchoredPosition.x, interTransform.anchoredPosition.y - 1);
        interBox.color = new Color(interBox.color.r, interBox.color.g, interBox.color.b, 0f);
        interBox.transform.rotation = Camera.main.transform.rotation;
        interBox.gameObject.SetActive(false);

        if(type == Type.text)
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
        if (Input.GetMouseButtonDown(0) && !isAnimating)
        {
            if (isdialoActiveing)
            {
                InterFade(true);
                if(type == Type.text) DialoFade(false);

                return;
            }
            else 
            {
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, results);

                foreach (RaycastResult result in results)
                {
                    if (result.gameObject == interBox.gameObject)//InterBox 클릭 했을 때
                    {
                        InterFade(false);
                        if (type == Type.text) DialoFade(true); ;
                        Interact();
                    }
                }
            }  
        }
    }

    private void FixedUpdate() // 클릭했을 때로 바꾸기
    {
        Collider[] hit = Physics.OverlapBox(transform.position+colliderTrans, colliderSize, Quaternion.identity, layerMask);
        if (hit.Length > 0)
        {
            if (!isInterActiveing && !onColider)
            {
                InterFade(true);
                onColider = true;
            }
        }
        else if ( onColider)
        {
            InterFade(false);
            onColider = false;
        }
    }

    public void InterFade(bool isFadeIn)
    {
        int posY = isFadeIn ? 1 : -1;
        interBox.transform.rotation = Camera.main.transform.rotation;
        if (isFadeIn)
        {
            
            interBox.gameObject.SetActive(true);
            
        }
        isAnimating = true;
        isInterActiveing = isFadeIn;
        interBox.DOFade(isFadeIn ? 1f:0f, duration);
        interTransform.DOAnchorPosY(interTransform.anchoredPosition.y +posY, duration).SetEase(Ease.InOutSine).OnComplete(() => 
        { isAnimating = false; interBox.gameObject.SetActive(isFadeIn); });
      
    }

    //이거 수정 필요
    private void LateUpdate()
    {
        interBox.transform.rotation = Camera.main.transform.rotation;
    }

    void Interact()
    {
        switch (type)
        {
            case Type.takeObj:
                transform.parent.gameObject.SetActive(false);
                ObjInterect();
                break;
            case Type.picture:
                PictureInteract();
                break;
            case Type.ClickCutScene:
                cutSceneObj.GetComponent<CutSceneManager>().StartCutScene();
                break;
            case Type.ClickLever:
                lever.GetComponent<Lever>().TurnLight(true);
                break;
            case Type.LampRotation:
                //lamp.GetComponent<TurnLight>().GeneralTileAppear(true, this);
                lamp.GetComponent<TurnLight>().TurnReverse(this);
                break;

        }

        if(isTutorial) TutorialManager.Inst.FinshTutorial();

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

    void PictureInteract()
    {
        pictureCanvas.transform.GetChild(1).GetComponent<Image>().sprite = picture;
        pictureCanvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = message;
        pictureCanvas.enabled = true;
    }

    void ObjInterect()
    {
        switch (objType)
        {
            case ObjType.key:
                InGameManager.Inst.isKey[keyIndex] = true;
                break;
        }
    }
}
