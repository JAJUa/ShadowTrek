using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CollectRelicManager : MonoBehaviour
{
    public static CollectRelicManager Inst;
    [SerializeField] private string[] chapterRelicName;
    [SerializeField] private GameObject[] chapterRelicOBJ;

    [SerializeField] private GameObject uiCanvas;
    bool openUI;


    [SerializeField] private TextMeshProUGUI relicCount, relicName;

    private void Awake()
    {
        Inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<Canvas>().worldCamera = Camera.main;
        GameObject relic = GameObject.FindGameObjectWithTag("Relic");
       
       
    }

    // Update is called once per frame
    void Update()
    {
       

        if (openUI)
        {
            if (Input.GetMouseButtonDown(0))
            {
                uiCanvas.SetActive(false);
                //InGameUIManager.Inst.inGameCanvas.SetActive(true);
            }
        }
    }

    public void Collect(int itemType)
    {
        Debug.Log("sss");
        uiCanvas.SetActive(true);
        StartCoroutine(Delay());
        relicName.text = chapterRelicName[itemType];
        relicCount.text = (GameData.Inst.relicsCurCount[itemType]+1).ToString() + " / " + GameData.Inst.relicsMaxCount[itemType] + " Found";
        chapterRelicOBJ[itemType].transform.DOLocalRotate(new Vector3(0f, 540f, 0f), 1.5f, RotateMode.FastBeyond360);
       // InGameUIManager.Inst.inGameCanvas.SetActive(false);
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.8f);
        openUI = true;
    }
}
