using System;
using UnityEngine;

public class WorldTimeController : MonoBehaviour
{
    [SerializeField] private Panel panel;

    private bool isShow = false;
    private void Update()
    {
        if (InGameManager.Inst.CurState() == GameState.SeraTurn)
        {
            if (Input.GetMouseButton(0))
            {
                FastFoward(true);
            }
            if (Input.GetMouseButtonUp(0))
            {
                FastFoward(false);
            }
        }
        
        if(InGameManager.Inst.CurState() != GameState.SeraTurn && isShow)
            FastFoward(false);
    }

    private void FastFoward(bool _enable)
    {
        Time.timeScale = _enable? 3:1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        if (_enable && !isShow)
        {
            panel.SetPosition(PanelStates.Show, true);
            isShow = true;
        }
        else if (!_enable && isShow)
        {
            panel.SetPosition(PanelStates.Hide, true);
            isShow = false;
        }
    }
}
