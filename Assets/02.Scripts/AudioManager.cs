using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Inst;
    AudioSource audioSource;
    public AudioClip[] backGroundAudio;
    public AudioClip[] effectAudioClip;

    private void Awake()
    {
        Inst = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void AudioPlay(int index)
    {
        audioSource.clip = backGroundAudio[index];
        audioSource.Play();
    }

    public void AudioEffectPlay(int index)
    {

        audioSource.PlayOneShot(effectAudioClip[index]);
    }

    public void DialogueAudio(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
