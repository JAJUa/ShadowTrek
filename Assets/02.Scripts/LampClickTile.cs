using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampClickTile : MonoBehaviour
{
    [SerializeField] float lampRot;
    MeshRenderer meshRenderer;
    TurnLight turnLight;
    Dialouge dialouge;

    private void Awake()
    {
        meshRenderer = transform.GetComponent<MeshRenderer>();
        turnLight = transform.parent.parent.GetComponentInChildren<TurnLight>();
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        Debug.Log("touch");
        dialouge.InterFade(true);
        turnLight.GeneralTileAppear(false);
        turnLight.Turning(lampRot);
    }

    public void AppearTile(bool appear, Dialouge dia)
    {
       
        Debug.Log(dia);
        dialouge = dia;
        if (appear)
        {
            gameObject.SetActive(true);
            InGameManager.Inst.moveBlock = true;
        }
        
        float fadeSpeed = 0.5f;
        
        float value = appear? 1 : 0;    
        meshRenderer.material.DOFade(value, fadeSpeed);
        if (!appear) DOVirtual.DelayedCall(fadeSpeed, () =>
        {
            gameObject.SetActive(false);
            InGameManager.Inst.moveBlock = false ;

        });
    }
}
