using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Inst;
    public AudioSource audio_BGM;
    public AudioSource audio_SFX;
    public AudioClip[] backGroundAudio;
    public AudioClip[] effectAudioClip;

    private void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AudioPlay(int index)
    {
        audio_BGM.clip = backGroundAudio[index];
        audio_BGM.Play();
    }

    public void AudioEffectPlay(int index)
    {
        audio_SFX.PlayOneShot(effectAudioClip[index]);
    }

    public void DialogueAudio(AudioClip audioClip)
    {
        audio_SFX.PlayOneShot(audioClip);
    }
}
