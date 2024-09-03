using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class LuminousObj : MonoBehaviour
{
    [SerializeField] bool isLuminous;
    [SerializeField] GameObject[] lights;
    InteractiveLights interactiveLights;
    // Start is called before the first frame update
    void Start()
    {
        isLuminous = false;
        interactiveLights = GetComponent<InteractiveLights>();
        interactiveLights.enabled = isLuminous;
        foreach(GameObject light in lights)
        {
            light.SetActive(false);
        }
       
    }


    [Button]
    public void LuminousActive()
    {
        isLuminous = true;
        interactiveLights.enabled = isLuminous; 
        if (isLuminous)
        {
            foreach (GameObject light in lights)
            {
                light.SetActive(true);
            }
        }
       
    }
}
