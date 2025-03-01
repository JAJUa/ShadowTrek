using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeManager : Singleton<VolumeManager>
{
    
    [SerializeField] private Volume globalVolume;
    [SerializeField] private VolumeProfile defaultVolume, answerVolume;
    
    public void ChangeGlobalVolume(bool isAnswer)
    {
        globalVolume.profile = isAnswer ? answerVolume : defaultVolume;
    }
    
    
}
