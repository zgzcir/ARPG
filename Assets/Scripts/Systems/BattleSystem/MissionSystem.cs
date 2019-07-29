using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MissionSystem : BaseSystem
 {
    public static MissionSystem Instance;
    public MissionPanel MissionPanel;
    public bool IsIn=false;
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
    {        IsIn = false;
        MissionPanel.SetPanelState(false);
    }
}