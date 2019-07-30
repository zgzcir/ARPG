using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;
using UnityEngine.Serialization;

public class MissionSys : BaseSystem
{
    public static MissionSys Instance;
    public MissionPanel MissionPanel;
    public bool IsIn = false;

    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
        CommonTool.Log("BattleSystem Connected");
    }

    public void EnterMission()
    {
        IsIn = true;
        OpenMissionPanel();
    }

    public void OpenMissionPanel()
    {
        MissionPanel.SetPanelState();
    }

    public void ExitMission()
    {
        IsIn = false;
        MissionPanel.SetPanelState(false);
    }

    public void RspMission(GameMsg msg)
    {
        var data = msg.RspMission;
        GameRoot.Instance.SetPlayerDataByMissionStart(data);

        ExitMission();
        MainCitySys.Instance.ExitMainCity();
        BattleSys.Instance.EntoBattle(data.MID);
    }
}