using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;
using VInspector;

public class Dialouge : MonoBehaviour
{
    [Space(10)]
    [Header("-- Dialouge System --")]   
    public float duration = 0.7f;
    public Canvas pictureCanvas;
    public bool isTutorial;

    public enum Type { text, takeObj, picture, ClickCutScene, ClickLever, LampRotation };
    public enum ObjType { key };
    public Type type;

    [SerializeField] Color defaultColor, answerColor;
    public Image interBox;

    [ShowIfEnum("type", (int)Type.text)]
    public Image dialougeBox; 

    /*
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
    */

    [Space(10)]
    [Header("-- Collider --")]

    [SerializeField] Vector3 colliderTrans;
    [SerializeField] Vector3 colliderSize;
    [SerializeField] LayerMask layerMask;

    [SerializeField] private RectTransform interTransform, dialoTransform;
    [SerializeField] private bool recallInter;
    private bool isInterActiveing, isdialoActiveing, isAnimating, onColider;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + colliderTrans, colliderSize * 2);
    }
    private void Start()
    {
       // layerMask += LayerMask.GetMask("Papa");
        defaultColor = interBox.color;
        interTransform = interBox.GetComponent<RectTransform>();
        interTransform.anchoredPosition = new Vector2(interTransform.anchoredPosition.x, interTransform.anchoredPosition.y - 1);
        interBox.color = new Color(interBox.color.r, interBox.color.g, interBox.color.b, 0f);
        interBox.transform.rotation = Camera.main.transform.rotation;
        interBox.gameObject.SetActive(false);

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

                foreach (RaycastResult result in results)
                {
                    if (result.gameObject == interBox.gameObject)//InterBox 클릭 했을 때
                    {
                        InterFade(false);
                        if (type == Type.text) DialoFade(true);
                       
                        Interact();

                        return;
                    }
                }

                // Inter Object Click

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                Transform current = transform;
                GameObject findObject = null;

                while (current != null)
                {
                    if (current.CompareTag("AutoLight"))
                    {
                        findObject = current.gameObject;
                        break;
                    }
                    current = current.parent;
                }

                if (Physics.Raycast(ray, out hit) && current != null)
                {
                    if (hit.collider.gameObject == findObject)
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

    private void FixedUpdate() // 클릭했을 때로 바꾸기
    {
        if (!InGameManager.Inst.isAnswering)
        {
            Collider[] hit = Physics.OverlapBox(transform.position + colliderTrans, colliderSize, Quaternion.identity, layerMask);
            if (hit.Length > 0)
            {
                //Debug.Log("올라가-1");
                if (!isInterActiveing && !onColider)
                {
                    //Debug.Log("올라가");
                    InterFade(true);
                    onColider = true;
                }
            }
            else if (onColider)
            {
                InterFade(false);
                onColider = false;
            }
        }
       
    }

    public void InterFade(bool isFadeIn)
    {
        interBox.color = InGameManager.Inst.isAnswering ? answerColor: defaultColor;

        int posY = isFadeIn ? 1 : -1;
        interBox.transform.rotation = Camera.main.transform.rotation;
        if (isFadeIn)
        {
            interBox.gameObject.SetActive(true);
        }
        isAnimating = true;
        isInterActiveing = isFadeIn;
        interBox.DOFade(isFadeIn ? 1f : 0f, duration);
        Debug.Log(interTransform);
        interTransform.DOAnchorPosY(interTransform.anchoredPosition.y + posY, duration).SetEase(Ease.InOutSine).OnComplete(() =>
        { isAnimating = false; interBox.gameObject.SetActive(isFadeIn); });
    }

    //이거 수정 필요
    private void LateUpdate()
    {
        interBox.transform.rotation = Camera.main.transform.rotation;
    }

    public virtual void Interact()
    {
        /*
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

        }*/
        if (InGameManager.Inst.isAnswering)
            AnswerManager.Inst.PapaTile();

        if (isTutorial)
            TutorialManager.Inst.FinshTutorial();

        if (!InGameManager.Inst.isAnswering && recallInter)
            DOVirtual.DelayedCall(0.8f, () => InterFade(true));

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
/*
    void PictureInteract()
    {
        pictureCanvas.transform.GetChild(1).GetComponent<Image>().sprite = picture;
        pictureCanvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = message;
        pictureCanvas.enabled = true;
    }*/

  
}
