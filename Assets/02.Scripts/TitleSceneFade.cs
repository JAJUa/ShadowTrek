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
    [SerializeField] private Button settingBackBtn;

    [SerializeField] Animator titleAnim;
    [SerializeField] Animator settingAnim;

    private void Start()
    {
        startBtn.onClick.AddListener(() => StartBtn());
        quitBtn.onClick.AddListener(() => QuitBtn());
        settingBtn.onClick.AddListener(() => SettingBtn(true));
        settingBackBtn.onClick.AddListener(() => SettingBtn(false));
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            titleAnim.SetTrigger("Skip");
        }
    }

    public void StartBtn()
    {
        AllBtnFalse();

        startBtn.GetComponent<Image>().DOFade(1, 1f).OnComplete(() =>
        {
            FadeInFadeOut.Inst.NextScene();
        });
    }

    public void SettingBtn(bool fade)
    {
        settingBtn.GetComponent<Image>().DOFade(fade ? 1f : 0f, 1f);

        settingAnim.SetBool("ActiveExcute", fade);

        if (fade)
        {
            settingBackBtn.gameObject.SetActive(fade);
            settingBackBtn.GetComponent<Image>().DOFade(0.6f, 1f);
         }
        else
        {
            settingBackBtn.GetComponent<Image>().DOFade(0, 1f).OnComplete(() =>
            {
                SaveSystem.Inst.SaveData();
                settingBackBtn.gameObject.SetActive(false);
            });
        }
    }

    public void QuitBtn()
    {
        AllBtnFalse();

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

    public void AllBtnFalse()
    {
        startBtn.interactable = false;
        quitBtn.interactable = false;
        settingBtn.interactable = false;
        settingBackBtn.interactable = false;
    }
}
