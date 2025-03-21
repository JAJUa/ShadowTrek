using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using VInspector;
using UnityEngine.Events;

public class MenuUIManager : MonoBehaviour
{
    public static MenuUIManager Inst;
    [Tab("아이콘")]
    public float turnTimeRate, range;
    private float turnRate;
    public Ease iconsMoveEase;
    public float iconUpDownSpeed;
    [SerializeField] GameObject iconExcuteBackButton, clickIcon;
    [SerializeField] private bool[] excuteIconActive;
    public UnityEvent tutorial;
    [Header("아이콘 관련")]
    public TMP_Text iconNameTxt;
    public GameObject[] iconsObj;
    public List<GameObject> excuteObj = new List<GameObject>();
    private List<GameObject> instanceIcon = new List<GameObject>();
    [SerializeField] Transform spawnCenter;
    [HideInInspector][SerializeField] int curIconNum;
    private Tween[] iconTween;

    [Tab("기타 관련")]
    [SerializeField] GameObject fog;
    public int curLanguageNum;

    public TMP_Text languageName_Text;
    
    [SerializeField] Scrollbar bgmScrollBar, soundEffectScrollBar;

    [HideInInspector][SerializeField] bool isTurning,isSelected;

   
    [SerializeField] GameObject relicsContent, skinsContent;
    public List<GameObject> relicScrollbarImage, skinScrollbarImage = new List<GameObject>();


    Vector2 startTouchPos, endTouchPos,swipeDelta;
    [SerializeField] private float touchInterval;
    public bool isSwiping,ableSwipe;

    [Tab("로컬라이즈")]
    [SerializeField] LocalizedString[] iconLocalizeName,clothSelectedLocalizeName;
    [SerializeField] LocalizeStringEvent iconLocalizeStringEvent;
    [SerializeField] LocalizeStringEvent[] clothSelectedLocalize_SE;

    private void Awake()
    {
        if(Inst != null && Inst != this)
        {
            Destroy(Inst);
            Inst = this;
        }
        else
        {
            Inst = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        ableSwipe = true;
        curLanguageNum = DataManager.Inst.Data.localizationNum;
        iconTween = new Tween[iconsObj.Length];
        Spawn();
        ChangeIconName();
        GetScrollbarImages();
        instanceIcon[curIconNum - 1].transform.DOScale(1.4f, 0.5f);
        AnimationIcon();
        StartCoroutine(waitLocalization());
        turnRate = 360 / iconsObj.Length;
        foreach (var icon in instanceIcon)
        {
            Vector3 dir = Camera.main.transform.position - icon.transform.position;
            icon.transform.LookAt(dir);
        }
    }

    #region 아이콘 관련

    void Spawn()
    {
        for (int i = 0; i < iconsObj.Length; i++)
        {
            float angle = i * Mathf.PI * 2 / iconsObj.Length;
            float x = Mathf.Cos(angle) * range;
            float z = Mathf.Sin(angle) * range;
            float angleDegress = angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, angleDegress, 0);
            GameObject spawnObj = Instantiate(iconsObj[i], spawnCenter.position - new Vector3(z, 0, x), Quaternion.identity);
            spawnObj.transform.parent = spawnCenter.transform;
            spawnObj.SetActive(true);
            instanceIcon.Add(spawnObj);

            if (excuteObj[i] == null)
                excuteObj[i] = spawnObj;

            StartCoroutine(waitIconUpDownDelay(i));
        }
    }

    IEnumerator waitIconUpDownDelay(int i)
    {
        yield return new WaitForSeconds((float)(0.5 + i*0.1f));
      //  iconTween[i] =  instanceIcon[i].transform.DOLocalMoveY(instanceIcon[i].transform.position.y - 0.5f, iconUpDownSpeed).SetLoops(-1, LoopType.Yoyo).SetEase(iconsMoveEase);
      // 위 아래로 조금씩 움직이는 코드인데 이거 사용하면 위치가 이상해짐
    }

    public void TurnIcon(bool isRight)
    {
        iconNameTxt.DOFade(0, 0.3f);
        if (!isTurning && !isSelected)
        {

            Vector3 rot = spawnCenter.eulerAngles;
            if (isRight)
            {
                spawnCenter.DORotate(new Vector3(0, rot.y + turnRate, 0), turnTimeRate);
                if (curIconNum == 1)
                    curIconNum = iconsObj.Length;
                else
                    curIconNum--;
            }
            else
            {
                spawnCenter.DORotate(new Vector3(0, rot.y - turnRate, 0), turnTimeRate);
                if (curIconNum == iconsObj.Length)
                    curIconNum = 1;
                else
                    curIconNum++;
              
            }

            isTurning = true;
            StartCoroutine(waitTurnTime());
        }

        for (int i = 0; i < instanceIcon.Count; i++)
        {
            instanceIcon[i].transform.DOScale(1f, 0.5f);
        }


    }

    public void ExcuteIcon(bool isOpen)
    {
        iconExcuteBackButton.SetActive(isOpen);
        clickIcon.SetActive(!isOpen);
        fog.SetActive(!isOpen);
        if (!isOpen)
        {
          
            instanceIcon[curIconNum - 1].SetActive(true);
            instanceIcon[curIconNum - 1].transform.DOScale(1f, 0.5f);
            excuteObj[curIconNum - 1].GetComponent<Animator>().SetBool("ActiveExcute", false);
            for (int i = 0; i < iconsObj.Length; i++)
            {
                if (i == curIconNum - 1)
                    iconTween[i].Kill();
               
                 instanceIcon[i].transform.DOMoveY(1, 1f);
                
            }

            StartCoroutine(waitTimeForIconAnim());
        }
        else
        {
            instanceIcon[curIconNum - 1].transform.DOScale(1.4f, 0.5f);
            excuteObj[curIconNum - 1].GetComponent<Animator>().SetBool("ActiveExcute", true);
            GetSkinData();
            GetRelicInformationFromGameData();
            //DOTween.KillAll();
            for (int i = 0; i < iconsObj.Length; i++)
            {
                if (i == curIconNum - 1)
                {
                    instanceIcon[i].SetActive(excuteIconActive[i]);
                    print(i);

                }                 
                else
                {
                    iconTween[i].Kill();
                    instanceIcon[i].transform.DOMoveY(instanceIcon[i].transform.position.y + 10f, 1f);
                  //  iconTween[i].Kill();
                }
            }
            isSelected = true;
            if(curIconNum -1 == 0)
            {
                if (!DataManager.Inst.Data.selectionTuto2)
                {
                    DOVirtual.DelayedCall(0.3f,()=> ExcuteSpecialEvent());
                }
            }
        }
       
    }

    public void ExcuteSpecialEvent()
    {
        if (tutorial != null)
            tutorial.Invoke();
    }

    IEnumerator waitTimeForIconAnim()
    {
        yield return new WaitForSeconds(0.5f);
        isSelected = false;
        for (int i = 0; i < iconsObj.Length; i++)
            StartCoroutine(waitIconUpDownDelay(i));
    }

    public void AnimationIcon()
    {
        Debug.Log(instanceIcon[curIconNum - 1].GetComponentInChildren<Animator>().gameObject.name);
        instanceIcon[curIconNum - 1].GetComponentInChildren<Animator>().SetTrigger("Active");
    }

    IEnumerator waitTurnTime()
    {

        yield return new WaitForSeconds(turnTimeRate);
        isTurning = false;
        instanceIcon[curIconNum - 1].transform.DOScale(1.4f, 0.5f);//아이콘 크기 키우기;


        ChangeIconName();
        AnimationIcon();

    }

    void ChangeIconName()
    {
        iconNameTxt.DOFade(1, 0.3f);
       // iconNameTxt.text = iconName[curIconNum - 1];
        iconLocalizeStringEvent.StringReference = iconLocalizeName[curIconNum - 1];
    }

    public void URLButton(string name)
    {

        switch (name)
        {
            case "Instagram":
                Application.OpenURL("https://www.instagram.com/shadowtrek/");
                break;
            case "YouTube":
                Application.OpenURL("https://www.youtube.com/channel/UCDJkmsYGagk295RpGPVe0Ng");
                break;

        }

    }
    #endregion


    // Update is called once per frame
    void Update()
    {

        

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("SNS"))
                {
                    URLButton(hit.collider.gameObject.name);
                }
            }

        }

        if (isTurning)
        {
            foreach (var icon in instanceIcon)
            {
                Vector3 dir = Camera.main.transform.position - icon.transform.position;
                icon.transform.LookAt(dir);

            }
        }

        if (ableSwipe)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTouchPos = Input.mousePosition;
                isSwiping = true;
            }

            if (Input.GetMouseButtonUp(0) && isSwiping)
            {
                endTouchPos = Input.mousePosition;
                swipeDelta = startTouchPos - endTouchPos;
                if (swipeDelta.magnitude > touchInterval) // 최소 스와이프 길이 설정
                {
                    DetectSwipe(swipeDelta);
                }

                isSwiping = false; // 스와이프 동작 끝
            }

        }

    }

    public void DetectSwipe(Vector2 swipeDelta)
    {

        if (swipeDelta.x > 0)
            TurnIcon(true);
        else
            TurnIcon(false);
        
    }

   
    public void GetSkinData()
    {
        for(int i = 1; i < skinScrollbarImage.Count - 1; i++)
        {
            Debug.Log(i);
            if (i == DataManager.Inst.Data.skinNum + 1)
                clothSelectedLocalize_SE[i].StringReference = clothSelectedLocalizeName[0];     
            else
                clothSelectedLocalize_SE[i].StringReference = clothSelectedLocalizeName[1];
        }
    }

    void GetRelicInformationFromGameData()
    {
        for(int i = 1;i<relicScrollbarImage.Count-1;i++)
        {
            Debug.Log(relicScrollbarImage[i].gameObject.name);
            GameObject countTxt =  relicScrollbarImage[i].transform.Find("Count").gameObject;
            countTxt.GetComponent<TextMeshProUGUI>().text = DataManager.Inst.Data.relicsCurCount[i-1].ToString() + " / " +DataManager.Inst.Data.relicsMaxCount[i-1].ToString() + " Found";
        }
    }

   
    #region SettingCanvas

    public void ChangeLanguage(bool right)
    {
        if(right)
        {
            if (curLanguageNum == 3)
                curLanguageNum = 1;
            else
                curLanguageNum++;
        }
        else
        {
            if (curLanguageNum == 1)
                curLanguageNum = 3;
            else
                curLanguageNum--;
        }
        UserLcoalization();
       
    }

    void GetScrollbarImages()
    {
        for (int i = 0; i < relicsContent.transform.childCount; i++)
        {
            relicScrollbarImage.Add(relicsContent.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < skinsContent.transform.childCount; i++)
        {
            skinScrollbarImage.Add(skinsContent.transform.GetChild(i).gameObject);
        }

    }

    public void UserLcoalization()
    {
        //languageName_Text.text = languageText[curLanguageNum-1];
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[curLanguageNum-1];
        DataManager.Inst.ChangeLocalization(curLanguageNum);
    }
    #endregion


    IEnumerator waitLocalization()
    {
        yield return new WaitForSeconds(0.5f);
        int localizedIndex = DataManager.Inst.Data.localizationNum > 0 ? DataManager.Inst.Data.localizationNum - 1 : DataManager.Inst.Data.localizationNum;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localizedIndex];
    }

   

}
