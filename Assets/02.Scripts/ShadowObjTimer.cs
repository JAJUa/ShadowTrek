using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShadowObjTimer : MonoBehaviour
{
    float shadowTime,limitShadowTime;
    bool timerStart;
    [SerializeField] Image timeImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timerStart &&limitShadowTime > 0)
        {
            limitShadowTime -= Time.deltaTime;
            timeImage.fillAmount = limitShadowTime / shadowTime;
        }
        else if(limitShadowTime <= 0)
        {
            Destroy(gameObject);
        }
        
    }

    public void GetTime(float time)
    {
        shadowTime= time;
        limitShadowTime= time;
        timerStart= true;
    }
}
