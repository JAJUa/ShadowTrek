using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class Lever : InteractiveObject
{
    [SerializeField] GameObject[] turnOnOffLights;
    [SerializeField] bool isTurnOn = false;
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject light in turnOnOffLights)
        {
            light.gameObject.SetActive(isTurnOn);
            float intensity = isTurnOn ? 1000 : 0;
            light.GetComponent<Light>().intensity = intensity;
        }
        
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public override void AutoLight()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + pos, size / 2, Quaternion.identity, LayerMask.GetMask("Player"));
        if (colliders.Length > 0)
        {
            if(!isTurnOn)
                 TurnLight(true);
        }
        else
        {
            if(isTurnOn)
                TurnLight(false);
        }
    }

    [Button]
    public void TurnLight(bool turnOn)
    {
        AudioManager.Inst.AudioEffectPlay(1);
        InGameManager.Inst.OnlyPlayerReplay();
        foreach (GameObject light in turnOnOffLights)  light.gameObject.SetActive(true); 
        isTurnOn = !isTurnOn;

        if (isTurnOn) animator.SetTrigger("Right");
        else animator.SetTrigger("Left");
        float intensity = isTurnOn ? 1000 : 0;
        DOVirtual.DelayedCall(0.3f, () =>
        {
            foreach (GameObject light in turnOnOffLights)
            {
                light.GetComponent<Light>().DOIntensity(intensity,0.5f);
                if (!turnOn) interactiveLight.TileColorDefault();
                DOVirtual.DelayedCall( 0.5f,()=>
                {
                    light.SetActive(isTurnOn);
                    if (turnOn) interactiveLight.ChangeTileColor();
                   
                });

            }
        });
      
    }
}
