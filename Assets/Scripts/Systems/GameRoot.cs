using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;
using UnityEngine.Serialization;

public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance;

    public LodingPanel LoadingPanel;
    public DynamicPanel DynamicPanel;
    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        CommonTool.Log("Link Start!");
        Init();
    }
    private void Init()
    {
        NetSvc net = GetComponent<NetSvc>();
        net.InitSvc();
        ResSvc res = GetComponent<ResSvc>();
        res.InitSvc();
        AudioSvc audio = GetComponent<AudioSvc>();
        audio.InitSvc();
        ViewSvc view = GetComponent<ViewSvc>();
        view.InitSvc();
        TimerSvc timer = GetComponent<TimerSvc>();
        timer.InitSvc();

        LoginSys login = GetComponent<LoginSys>();
        login.InitSys();
        MainSys playerOpratete = GetComponent<MainSys>();
        playerOpratete.InitSys();
        EntoSceneSys entoScene = GetComponent<EntoSceneSys>();
        entoScene.InitSys();

        

        MissionSys mission = GetComponent<MissionSys>();
        mission.InitSys();

        BattleSys battle = GetComponent<BattleSys>();
        battle.InitSys();
        
        InitUIRoot();
        login.EnterLogin();
    }

    private void InitUIRoot()
    {
        Transform canvas = transform.Find("Canvas");
        for (int i = 0; i < canvas.childCount; i++)
        {
            var basePanel = canvas.GetChild(i).GetComponent<BasePanel>();
            basePanel.gameObject.SetActive(false);
            basePanel.IsOpen = false;
            basePanel.Init(); 
        }
        DynamicPanel.SetPanelState();
    }


    
    public static void AddTips(string tips)
    {
        Instance.DynamicPanel.AddTips(tips);
    }

    private PlayerData playerData = null;

    public PlayerData PlayerData => playerData;

    public void SetPlayerData(RspLogin data)
    {
        playerData = data.PlayerData;
    }

    public void SetPlayerName(string name)
    {
        playerData.Name = name;
    }

    public void SetPlayerDataByGuide(RspGuide data)
    {
        playerData.Coin = data.coin;
        playerData.GuideID = data.id;
        playerData.Level = data.lv;
        playerData.Exp = data.exp;
    }

    public void SetPlayerDataByStrengthen(RspStrengthen data)
    {
        playerData.Coin = data.coin;
        playerData.HP = data.hp;
        playerData.Crystal = data.crystal;
        playerData.PA = data.pa;
        playerData.PD = data.pd;
        playerData.SA = data.sa;
        playerData.SD = data.sd;
        playerData.StrenArr = data.strenarr;
    }

    public void SetPlayerDataByTranscation(RspTranscation data)
    {
        playerData.Coin = data.coin;
        playerData.Diamond = data.diamond;
        playerData.Power = data.power;
    }

    public void SetPlayerDataByPower(PshPower data)
    {
        playerData.Power = data.power;
    }

    public void SetPlayerDataByTask(RspTakeTaskReward data)
    {
        playerData.Coin = data.Coin;
        playerData.Level
            = data.Lv;
        playerData.Exp = data.Exp;
        playerData.TaskRewardArr = data.TaskRewardArr;
    }

    public void SetPlayerDataByTaskPrgs(PshTaskPrgs data)
    {
        playerData.TaskRewardArr = data.TaskRewardArr;
    }

    public void SetPlayerDataByMissionStart(RspMission data)
    {
        playerData.Power = data.Power;
    }
}