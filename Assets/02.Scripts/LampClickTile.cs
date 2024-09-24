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
       // turnLight.GeneralTileAppear(false);
        turnLight.Turning(lampRot);
    }

  
}
