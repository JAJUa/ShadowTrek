using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Inst;
    
    [SerializeField] private Volume globalVolume;
    [SerializeField] private VolumeProfile defaultVolume, answerVolume;

    private void Awake()
    {
        Inst = this;
    }

    public void ChangeGlobalVolume(bool isAnswer)
    {
        globalVolume.profile = isAnswer ? answerVolume : defaultVolume;
    }
    
    
}
