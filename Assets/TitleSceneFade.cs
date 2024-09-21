using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneFade : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button quitBtn;

    [SerializeField] Animator anim;

    private void Start()
    {
        startBtn.onClick.AddListener(() => StartBtn());
        settingBtn.onClick.AddListener(() => SettingBtn());
        quitBtn.onClick.AddListener(() => QuitBtn());
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            anim.SetTrigger("Skip");
        }
    }

    public void StartBtn()
    {
        startBtn.GetComponent<Image>().DOFade(1, 1f).OnComplete(() =>
        {
            FadeInFadeOut.Inst.NextScene();
        });
    }

    public void SettingBtn()
    {
        settingBtn.GetComponent<Image>().DOFade(1, 1f);
    }

    public void QuitBtn()
    {
        quitBtn.GetComponent<Image>().DOFade(1, 1f).OnComplete(() =>
        {
            FadeInFadeOut.Inst.FadeIn();

            DOVirtual.DelayedCall(1f, () => {
                #if UNITY_EDITOR
                                UnityEditor.EditorApplication.isPlaying = false;
                #else
                                    Application.Quit(); // 어플리케이션 종료
                #endif
            });
        });
    }
}
