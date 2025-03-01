using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Inst;
    public AudioClip[] backGroundMusic;
    public AudioClip[] soundEffect;

    public Scrollbar bgmSlider;
    public Scrollbar sfxSlider;

    public AudioSource bgmAudioSource;
    public AudioMixer audioMixer;

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

    private IEnumerator Start()
    {
        bgmSlider = GameObject.Find("BGMScrollbar").GetComponent<Scrollbar>();
        sfxSlider = GameObject.Find("SoundEffectScrollBar").GetComponent<Scrollbar>();

        bgmAudioSource = GetComponent<AudioSource>();

        yield return new WaitUntil(() => DataManager.Inst);
        SliderSetting();
    }

    public void SliderSetting()
    {
        bgmSlider.value = DataManager.Inst.Data.bgmVolume;
        sfxSlider.value = DataManager.Inst.Data.soundEffectVolume;

        bgmSlider.onValueChanged.AddListener(ChangeBackGroundAudio);
        sfxSlider.onValueChanged.AddListener(ChangeSoundEffectVolume);

        ChangeBackGroundAudio(bgmSlider.value);
        ChangeSoundEffectVolume(sfxSlider.value);
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

    public void ChangeBackGroundAudio(float soundEffectScrollBar)
    {
        if (soundEffectScrollBar == 0)
            audioMixer.SetFloat("BGMParam", -80f);
        else
            audioMixer.SetFloat("BGMParam", Mathf.Log10(soundEffectScrollBar) * 20);

        DataManager.Inst.Data.bgmVolume = soundEffectScrollBar;
    }

    public void ChangeSoundEffectVolume(float soundEffectScrollBar)
    {
        if (soundEffectScrollBar == 0)
            audioMixer.SetFloat("SFXParam", -80f);
        else
            audioMixer.SetFloat("SFXParam", Mathf.Log10(soundEffectScrollBar) * 20);

        DataManager.Inst.Data.soundEffectVolume = soundEffectScrollBar;
    }
}
