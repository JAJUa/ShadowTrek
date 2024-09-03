using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Inst;
    public AudioClip[] backGroundMusic;
    public AudioClip[] soundEffect;

    public AudioSource bgmAudioSource;
    public float soundEffectVolume;

    private void Awake()
    {
        if (Inst != null )
        {
            Destroy(gameObject);
        }
        else
        {
            Inst = this;
        }
       
    }
    // Start is called before the first frame update
    void Start()
    {

        bgmAudioSource = GetComponent<AudioSource>();
        bgmAudioSource.volume = GameData.Inst.bgmVolume;

    }

   
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SummonSoundEffect(string effectName)
    {
        foreach(AudioClip sound in soundEffect)
        {
            if(sound.name == effectName)
            {
                bgmAudioSource.PlayOneShot(sound);
            }
        }
    }

    public void ChangeSoundEffectVolume(Scrollbar soundEffectScrollBar)
    {
        soundEffectVolume = soundEffectScrollBar.value;
        GameData.Inst.soundEffectVolume = soundEffectVolume;
    }

    public void ChangeBackGroundAudio()
    {
        GameData.Inst.bgmVolume = bgmAudioSource.volume;
    }
}
