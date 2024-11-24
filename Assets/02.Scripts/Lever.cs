using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class Lever : InteractiveObject
{
    

    [SerializeField] private Lamp lamp;
    
    [SerializeField] bool isTurnOn = false;
    [SerializeField] bool autoBool;
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        isTurnOn = false;
      
        
        animator = GetComponent<Animator>();
    }


    public override void ResetObj()
    {
        TurnLight(false,true);
    }
    

    public void CutSceneTurnLight(bool turnOn)
    {
        TurnLight(turnOn);
    }

    public void TurnLight(bool isResetLight = false)
    {
        isTurnOn = !isTurnOn;
        OnOff(isTurnOn, isResetLight);  
      
    }
    public void TurnLight(bool turnOn,bool isResetLight = false)
    {
        OnOff(turnOn, isResetLight);

    }

    public void OnOff(bool turnOn, bool isResetLight = false) //isResetLight 없앨 예정
    {
        Debug.Log("Lever" + isTurnOn);
        if(AudioManager.Inst !=null)
            AudioManager.Inst.AudioEffectPlay(1);
        lamp.TargetTileLighting(turnOn);
        if (isTurnOn) animator.SetTrigger("Right");
        else animator.SetTrigger("Left");
        
        /*
       
        if (!isResetLight) InGameManager.Inst.OnlyPlayerReplay(true,false);
        foreach (GameObject light in turnOnOffLights) light.gameObject.SetActive(true);
        isTurnOn = turnOn;

      
        float intensity = isTurnOn ? 1000 : 0;
        DOVirtual.DelayedCall(0.3f, () =>
        {
            foreach (GameObject light in turnOnOffLights)
            {
                light.GetComponent<Light>().DOIntensity(intensity, 0.5f);
                Debug.Log(isTurnOn);
               // if (!isTurnOn) interactiveLight.TileColorDefault();
                DOVirtual.DelayedCall(0.2f, () =>
                {
                    light.SetActive(isTurnOn);
                 //   if (isTurnOn == true) interactiveLight.ChangeTileColor();

                });

            }
        });
        */
    }
}
