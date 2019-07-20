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
        PlayerOprateSys playerOpratete = GetComponent<PlayerOprateSys>();
        playerOpratete.InitSys();
        EntoSceneSys entoScene = GetComponent<EntoSceneSys>();
        entoScene.InitSys();
        
        
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
        playerData.name = name;
    }

    public void SetPlayerDataByGuide(RspGuide data)
    {
        playerData.coin = data.coin;
        playerData.guideid = data.id;
        playerData.level = data.lv;
        playerData.exp = data.exp;
    }

    public void SetPlayerDataByStrengthen(RspStrengthen data)
    {
        playerData.coin = data.coin;
        playerData.hp = data.hp;
        playerData.crystal = data.crystal;
        playerData.pa = data.pa;
        playerData.pd = data.pd;
        playerData.sa = data.sa;
        playerData.sd = data.sd;
        playerData.strenarr = data.strenarr;
    }

    public void SetPlayerDataByTranscation(RspTranscation data)
    {
        playerData.coin = data.coin;
        playerData.diamond = data.diamond;
        playerData.power = data.power;
    }
    public void SetPlayerDataByPower(PshPower data)
    {
        playerData.power = data.power;
    }
}