using System;
using System.Collections;
using Febucci.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class TitleController : MonoBehaviour
{
    //인 게임의 타이틀을 띄우는 스크립트
    [SerializeField] private Panel panel;
    [SerializeField] TypewriterByCharacter tw_titleText;
    [Tooltip("전체적인 배속")][SerializeField] private float speed;

    private void Start()
    {
        StartCoroutine(TitleDealy());
    }

    IEnumerator TitleDealy()
    {
        yield return new WaitForSeconds(0.5f/speed);
        panel.SetPosition(PanelStates.Show,true,0.8f/speed);
        tw_titleText.ShowText("타이틀");
        yield return new WaitForSeconds(1.5f/speed);
        tw_titleText.StartDisappearingText();
        yield return new WaitForSeconds(1.1f/speed);
        panel.SetPosition(PanelStates.Hide,true,0.8f/speed);
        yield return new WaitForSeconds(1f/speed);
        
        StateChange();
    }
    
    private void StateChange() => InGameManager.Inst.ChangeState(GameState.SeraTurn);
}
