using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class EclipseManager : MonoBehaviour
{
    public GameObject[] stars;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    public void StarFalls()
    {
        foreach(var star in stars)
        {
            float random = Random.Range(0.1f, .8f);
            DOVirtual.DelayedCall(random, () =>
            {
                star.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                star.GetComponent<Renderer>().material.DOColor(Color.black, 1f);
                star.transform.DOMoveY(-60, 5f);
            });
          
        }
    }
}
