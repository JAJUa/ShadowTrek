using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WoodGimic : MonoBehaviour
{
    public Transform woodTrans;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            woodTrans.DOLocalRotate(new Vector3(0, 0, -26), 2f).SetEase(Ease.OutBounce);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            woodTrans.DOLocalRotate(new Vector3(0, 0, 8.8f), 2f).SetEase(Ease.OutSine);
        }
    }
}
