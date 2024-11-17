using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isLight;
    public Character character;
    public bool isEndTile;

    [SerializeField] private Material lightMat;
    private Material defaultMat;

    private Transform clickTile;
    private Renderer renderer;
    private List<bool> isLightArray = new List<bool>(); 


    private void Awake()
    {
        clickTile = transform.GetChild(0).transform;
        renderer = clickTile.GetComponent<Renderer>();
        defaultMat = renderer.sharedMaterial;
    }
    
    public void GetLight(bool light)
    {
        isLightArray.Add(light);
       // isLight = light; //나중에 수정
        //renderer.material = light?lightMat:defaultMat;
    }

    public void SetLight()
    {
        isLight = isLightArray.Any(l => l == true) ? true : false;
        renderer.material = isLight?lightMat:defaultMat;
        isLightArray.Clear();
    }
}
