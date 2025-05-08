using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeManager : Singleton<VolumeManager>
{
    
    [SerializeField] private Volume globalVolume;
    [SerializeField] private VolumeProfile defaultVolume, answerVolume,darkVolume;

    private void Start()
    {
       SetDarkVolume();
    }

    public void SetDarkVolume()
    {
        globalVolume.profile = darkVolume;
        RenderSettings.fog = false;
    }

    public void SetDefaultVolume()
    {
        globalVolume.profile = defaultVolume;
        RenderSettings.fog = true;
    }

    public void ChangeGlobalVolume(bool isAnswer)
    {
        globalVolume.profile = isAnswer ? answerVolume : defaultVolume;
    }
    
    
}
