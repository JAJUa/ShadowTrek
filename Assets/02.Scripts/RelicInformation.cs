using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicInformation : MonoBehaviour
{
    [Tooltip("유물의 종류")]
    public int relicType;  //relicType 은 호리병/호박/로봇 등 종류, relicNumber는 그 종류 중에서 몇번째 유물인지
    [Tooltip("유물의 종류 n번째")]
    public int relicNum;


    private IEnumerator Start()
    {
        yield return new WaitUntil(() =>  DataManager.Inst);
        
        if (DataManager.Inst.Data.relicsBool[relicType][relicNum])
        {

            gameObject.SetActive(false);
        }
        else
            gameObject.SetActive(true);
    }


    private void OnMouseDown()
    {

        CollectRelicManager.Inst.Collect(relicType);
        DataManager.Inst.GetRelic(relicType, relicNum);
        gameObject.SetActive(false);
       
    }
}
