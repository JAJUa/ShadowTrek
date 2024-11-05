using Abu;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class TutoFadeManager : MonoBehaviour
{
    [Tab ("FadeInTutorial")]
    [SerializeField] GameObject tutorialFade;
    TutorialFadeImage tutorialFadeImage;
    [SerializeField] TextMeshProUGUI tutoText;
    [Tab("SwipeMouse")]
    [SerializeField] Image backGround;
    [SerializeField] Image[] t_Icon;
    [SerializeField] TextMeshProUGUI[] t_text;
    int swipeIndex;
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        tutorialFadeImage = tutorialFade.GetComponent<TutorialFadeImage>();
        yield return new WaitUntil(() => GameData.Inst);
        if (!GameData.Inst.selectionTuto1) Appear();
    }

    [Button]
    public void Fade()
    {
        if (!GameData.Inst.selectionTuto2)
        {
            Transform parent = tutoText.transform.parent;
            tutorialFade.gameObject.SetActive(true);
            DOVirtual.DelayedCall(0.3f, () =>
            {
                DOTween.To(() => tutorialFadeImage.Smoothness, x => tutorialFadeImage.Smoothness = x, 0.012f, .7f)
                .OnComplete(() => { tutoText.gameObject.SetActive(true); tutoText.transform.DOScale(1, 0.5f); parent.gameObject.SetActive(true); parent.DOScale(1, 0.5f); }); ;
                //������ ���̺� �� �ٽô� ���� �ȵǰ�
                GameData.Inst.selectionTuto2 = true;
                SaveSystem.Inst.SaveData();
            });
  

        }
       
    }

    [Button]
    public void FadeOut()
    {
        Transform parent = tutoText.transform.parent;
        parent.DOScale(0, 0.5f);
        tutoText.transform.DOScale(0, 0.5f).OnComplete(() =>
        {
            
        });
        tutoText.gameObject.SetActive(false);
        parent.gameObject.SetActive(false);
        DOTween.To(() => tutorialFadeImage.Smoothness, x => tutorialFadeImage.Smoothness = x, .7f, 1).OnComplete(() => tutorialFade.gameObject.SetActive(false));

    }

    [Button]
    public void Appear()
    {
        if (!GameData.Inst.selectionTuto1)
        {
            MenuUIManager.Inst.ableSwipe = false;
            Disappear();
            if (swipeIndex < t_Icon.Length)
            {

                backGround.gameObject.SetActive(true);
                Image icon = t_Icon[swipeIndex];
                TextMeshProUGUI text = t_text[swipeIndex];

                icon.gameObject.SetActive(true);
                text.gameObject.SetActive(true);

                icon.DOFade(1, 0.5f);
                text.DOFade(1, 0.5f);
                swipeIndex++;
            }
        }
       
    }

    [Button]
    private void Disappear()
    {
        backGround.gameObject.SetActive(false);
        foreach(Image icon in t_Icon)
        {
            icon.gameObject.SetActive(false);
            icon.DOFade(0, 0.5f);
        }

        foreach (TextMeshProUGUI text in t_text)
        {
            text.gameObject.SetActive(false);
            text.DOFade(0, 0.5f);
        }

        if (swipeIndex >= t_Icon.Length)
        {
            MenuUIManager.Inst.ableSwipe = true;
            GameData.Inst.selectionTuto1 = true;
            SaveSystem.Inst.SaveData();
        }
    }
}
