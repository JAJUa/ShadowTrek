using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class Lever : InteractiveObject
{
    [SerializeField] GameObject[] turnOnOffLights;
    [SerializeField] bool isTurnOn = false;
    [SerializeField] LayerMask layerMask;
    [SerializeField] bool autoBool;
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        layerMask += LayerMask.GetMask("Player");
        foreach (GameObject light in turnOnOffLights)
        {
            light.gameObject.SetActive(isTurnOn);
            float intensity = isTurnOn ? 1000 : 0;
            light.GetComponent<Light>().intensity = intensity;
        }
        
        animator = GetComponent<Animator>();
    }


    public override void ResetObj()
    {
        TurnLight(false,true);
    }

    public override void AutoLight()
    {
        if (autoBool)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position + autoLightPos, autoLight / 2, Quaternion.identity, layerMask);
            if (colliders.Length > 0)
            {
                if (!isTurnOn)
                    TurnLight(true);
            }
            else
            {
                if (isTurnOn)
                    TurnLight(false);
            }
        }
       
    }

    public void CutSceneTurnLight(bool turnOn)
    {
        TurnLight(turnOn);
    }

    public void TurnLight(bool isResetLight = false)
    {
      OnOff(!isTurnOn, isResetLight);  
      
    }
    public void TurnLight(bool turnOn,bool isResetLight = false)
    {
        OnOff(turnOn, isResetLight);

    }

    public void OnOff(bool turnOn, bool isResetLight = false)
    {
        AudioManager.Inst.AudioEffectPlay(1);
        if (!isResetLight) InGameManager.Inst.OnlyPlayerReplay();
        foreach (GameObject light in turnOnOffLights) light.gameObject.SetActive(true);
        isTurnOn = turnOn;

        if (isTurnOn) animator.SetTrigger("Right");
        else animator.SetTrigger("Left");
        float intensity = isTurnOn ? 1000 : 0;
        DOVirtual.DelayedCall(0.3f, () =>
        {
            foreach (GameObject light in turnOnOffLights)
            {
                light.GetComponent<Light>().DOIntensity(intensity, 0.5f);
                Debug.Log(intensity);
                if (!turnOn) interactiveLight.TileColorDefault();
                DOVirtual.DelayedCall(0.2f, () =>
                {
                    light.SetActive(isTurnOn);
                    if (turnOn) interactiveLight.ChangeTileColor();

                });

            }
        });
    }
}
