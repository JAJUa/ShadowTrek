using System;
using System.Collections;
using System.Collections.Generic;
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


    private void Awake()
    {
        clickTile = transform.GetChild(0).transform;
        renderer = clickTile.GetComponent<Renderer>();
        defaultMat = renderer.sharedMaterial;
    }
    
    public void GetLight(bool light)
    {
        isLight = light;
        renderer.material = light?lightMat:defaultMat;
    }
}
