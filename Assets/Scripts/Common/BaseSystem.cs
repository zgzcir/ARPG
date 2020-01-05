using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSystem : MonoBehaviour
{
    protected ResSvc resSvc;
    protected AudioSvc audioSvc;
    protected NetSvc netSvc;
    protected TimerSvc timerSvc;

    public virtual void InitSys()
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
        netSvc = NetSvc.Instance;
        timerSvc = TimerSvc.Instance;
    }

    protected virtual void UIRegisterSysEvents()
    {
    }

    public virtual void SwitchPanel(BasePanel panel, int force = 0)
    {
        if (force == 1 && panel.IsOpen)
        {
            return;
        }

        if (force == 2 && !panel.IsOpen)
        {
            return;
        }
    }
    protected void BaseSwitchPanel(BasePanel panel)
    {
        if (panel.IsOpen)
        {
            panel.SetPanelState(false);
        }
        else
        {
            panel.SetPanelState();
        }
    }
}