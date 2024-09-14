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

    private void Start()
    {
        DOVirtual.DelayedCall(0.05f, () =>
        {
            Debug.Log(GameData.Inst.relicsBool[relicType][relicNum]);
            if (GameData.Inst.relicsBool[relicType][relicNum])
            {

                gameObject.SetActive(false);
            }
            else
                gameObject.SetActive(true);
        });
        
    }


    private void OnMouseDown()
    {
        CollectRelicManager.Inst.Collect(relicType);
        GameData.Inst.GetRelic(relicType, relicNum);
        gameObject.SetActive(false);
    }
}
