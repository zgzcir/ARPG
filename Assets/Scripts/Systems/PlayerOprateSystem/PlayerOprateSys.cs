using System;
using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Serialization;

public class PlayerOprateSys : BaseSystem
{
    public static PlayerOprateSys Instance;
    public bool IsPlayerControll;
    public ChaInfoPanel ChaInfoPanel;
    public TasksPanel TasksPanel;
    public MainPanel MainPanel;
    public DialogPanel DialogPanel;
    public StrengthenPanel StrengthenPanel;
    public TransactionsPanel TransactionsPanel;
    public ChatPanel ChatPanel;
    private bool isChaInfoOpen;
    private bool isTasksopen;
    private PlayerController playerController;
    private CameraController cameraController;
    private CharacterController characterController;
    private Transform chaCameraTrans;
    private int nowOpenCursorPanel;
    private Vector3 chaCameraRotationOrigin;
    private Vector3 chaCameraPositionOrigin;

    private GuideCfg curGuideData;

    private NavMeshAgent navMeshAgent;


    public void InjectPOSysThings(PlayerController pc, CameraController cc)
    {
        playerController = pc;
        cameraController = cc;
        navMeshAgent = playerController.GetComponent<NavMeshAgent>();
        characterController = playerController.GetComponent<CharacterController>();
    }

    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
        CommonTool.Log("PlayerOperateSys Connected");
    }

    public void SwitchPanel(BasePanel panel, int force = 0)
    {
        if (force == 1 && panel.IsOpen)
        {
            return;
        }

        if (force == 2 && !panel.IsOpen)
        {
            return;
        }

        if (panel.IsOpen)
        {
            nowOpenCursorPanel--;
            panel.SetPanelState(false);
            if (nowOpenCursorPanel == 0)
            {
                SetMainCamreraRotateState();
                ViewSvc.Instance.SetCursorState(false);
            }
        }
        else
        {
            nowOpenCursorPanel++;
            panel.SetPanelState();
            SetMainCamreraRotateState(false);
            ViewSvc.Instance.SetCursorState();
        }
    }

    public void OnSwitchChaInfoPanel(bool isOPen = true)
    {
        chaCameraTrans.gameObject.SetActive(isOPen);
    }

    private void Update()
    {
        if (IsPlayerControll)
        {
            if (Input.GetKeyDown(PlayerCfg.ChaInfoPanel))
            {
                SwitchPanel(ChaInfoPanel);
            }

            if (Input.GetKeyDown(PlayerCfg.TasksPanel))
            {
                SwitchPanel(TasksPanel);
            }

            if (Input.GetKeyDown(PlayerCfg.StrengthenPanel))
            {
                SwitchPanel(StrengthenPanel);
            }

            if (Input.GetKeyDown(PlayerCfg.TranscationsPanelPower))
            {
                TransactionsPanel.SetType(0);

                SwitchPanel(TransactionsPanel);
            }

            if (Input.GetKeyDown(PlayerCfg.TranscationsPanelCoin))
            {
                TransactionsPanel.SetType(1);

                SwitchPanel(TransactionsPanel);
            }

         
        }
        if (Input.GetKeyDown(PlayerCfg.EntoMission))
        {
            SwitchMissionSystem();
        }
        if (isNavigate)
        {
            DetectIsArriveNavPos();
        }
    }


    public void SetMainCamreraRotateState(bool can = true)
    {
        cameraController.SetRotateState(can);
    }

    public void DisablePlayerControl()
    {
        IsPlayerControll = false;

        playerController.enabled = false;
        cameraController.enabled = false;
    }

    public void EnablePlayerControl()
    {
        IsPlayerControll = true;

        playerController.enabled = true;
        cameraController.enabled = true;
    }


    public void InitDefault()
    {
        //playerctrl
        playerController.Init();
        if (Camera.main != null) cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.Target = playerController.CameraPivot.transform;


        if (chaCameraTrans == null)
        {
            chaCameraTrans = GameObject.FindWithTag("ChaCamera").transform;
        }

        chaCameraTrans.gameObject.SetActive(false);
        SetStartChaCameraTrans();
    }

    public void SetPlayerMoveMobile(Vector2 dir)
    {
        playerController.InputDir = dir;
    }

    public void SetChaCameraRotate(float rotate)
    {
        var cameraPivot = playerController.ChaCameraRotatePivot.transform;
        chaCameraTrans.transform.RotateAround(cameraPivot.position, cameraPivot.up, rotate);
    }

    public void SetStartChaCameraTrans()
    {
        chaCameraRotationOrigin = chaCameraTrans.transform.localEulerAngles;
        chaCameraPositionOrigin = chaCameraTrans.transform.localPosition;
    }

    public void ResetChaCameraTrans()
    {
        chaCameraTrans.localEulerAngles = chaCameraRotationOrigin;
        chaCameraTrans.localPosition = chaCameraPositionOrigin;
    }


    private bool isNavigate;

    public bool IsNavigate => isNavigate;

    public void NavGuide(GuideCfg cfg)
    {
        if (cfg != null)
        {
            curGuideData = cfg;
        }

        navMeshAgent.enabled = true;
        if (curGuideData.NpcID != -1)
        {
            float dis = Vector3.Distance(playerController.transform.position,
                curMapBaseInfo.NpcPosTrans[cfg.NpcID].position);
            if (dis <= 0.5f)
            {
                isNavigate = false;
            }
            else
            {
                characterController.enabled = false;
                isNavigate = true;
                navMeshAgent.speed = Constans.PLyerMoveSpeed;
                navMeshAgent.SetDestination(curMapBaseInfo.NpcPosTrans[cfg.NpcID].position);
            }
        }
    }

    public void CancelNavGuide()
    {
        GameRoot.AddTips("导航取消");
        StopNavSet();
    }

    private MapBaseInfo curMapBaseInfo;

    private void DetectIsArriveNavPos()
    {
        float dis = Vector3.Distance(playerController.transform.position,
            curMapBaseInfo.NpcPosTrans[curGuideData.NpcID].position);
        if (dis <= 0.5f)
        {
            StopNavSet();
            SwitchPanel(DialogPanel);
        }
    }

    private void StopNavSet()
    {
        isNavigate = false;
        characterController.enabled = true;
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;
    }

    public void FreshMapBaseInfo()
    {
        curMapBaseInfo = GameObject.FindWithTag("MapRoot").GetComponent<MapBaseInfo>();
    }

    private void SwitchMissionSystem(int force = 0)
    {
        if (isNavigate)
        {
            CancelNavGuide();
        }

        MissionSystem missionSystem = MissionSystem.Instance;
        if (force == 1 && missionSystem.IsIn)
        {
            return;
        }

        if (force == 2 && !!missionSystem.IsIn)
        {
            return;
        }

        if (MissionSystem.Instance.IsIn)
        {
            EnablePlayerControl();
            ViewSvc.Instance.SetCursorState(false);
            missionSystem.ExitMission();
        }
        else
        {
            DisablePlayerControl();
            ViewSvc.Instance.SetCursorState(true);
            missionSystem.EnterMission();
        }
    }

    public void RspGuide(GameMsg msg)
    {
        RspGuide data = msg.RspGuide;
        GameRoot.AddTips(Constans.Color("金币+", TxtColor.Red)
                         + curGuideData.Coin + "  经验+" + curGuideData.Exp);
        GameRoot.Instance.SetPlayerDataByGuide(data);
        MainPanel.FreshPanel();
        switch (curGuideData.ActID)
        {
            case 0:
                break;
            case 1:
                SwitchMissionSystem(1);
                break;
            case 2:
                SwitchPanel(StrengthenPanel,1);
                break;
            case 3:
                TransactionsPanel.SetType(0);
                SwitchPanel(TransactionsPanel,1);
                break;
            case 4:
                TransactionsPanel.SetType(1);
                SwitchPanel(TransactionsPanel,1);
                break;
            case 5:
                SwitchPanel(ChatPanel,1);
                break;
        }
    }

    public void EnterPlayerOprate()
    {
        IsPlayerControll = true;
        MainPanel.SetPanelState();
        ChatPanel.SetPanelState();
        InitDefault();
        EnablePlayerControl();
        ViewSvc.Instance.SetCursorState(false);
        ViewSvc.Instance.AdjustDepthFieldFL(8f);

        FreshMapBaseInfo();
        //读取地图信息     TODO || Temp
        audioSvc.PlayBgAudio(Constans.BGCityHappy, true);
    }

    public GuideCfg GetCurGuideData()
    {
        return curGuideData;
    }

    public void RspStrengthen(GameMsg msg)
    {
        int combatPowerPre = CommonTool.CalcuEvaluation(GameRoot.Instance.PlayerData);
        GameRoot.Instance.SetPlayerDataByStrengthen(msg.RspStrengthen);
        int combatPowerNow = CommonTool.CalcuEvaluation(GameRoot.Instance.PlayerData);
        StrengthenPanel.FreshPanel();
        MainPanel.FreshPanel();
        audioSvc.PlayUIAudio(Constans.UISsuccess);
        GameRoot.AddTips(Constans.Color("战斗力", TxtColor.Green) + "提升了" +
                         Constans.Color((combatPowerNow - combatPowerPre).ToString(), TxtColor.White));
    }

    #region chat

    public void PshChat(GameMsg msg)
    {
        ChatPanel.AddChatMsg(msg);
    }

    public void RspTranscation(GameMsg msg)
    {
        GameRoot.AddTips("购买成功!");
        GameRoot.Instance.SetPlayerDataByTranscation(msg.RspTranscation);
        TransactionsPanel.btnSure.interactable = true;
        ChaInfoPanel.FreshPanel();
        StrengthenPanel.FreshPanel();
        MainPanel.FreshPanel();

        if (msg.PshTaskPrgs != null)
        {
            PshTaskPrgs(msg);
        }
    }

    #endregion

    #region power

    public void PshPower(GameMsg msg)
    {
        GameRoot.Instance.SetPlayerDataByPower(msg.PshPower);
        MainPanel.FreshPanel();
    }

    #endregion

    #region task

    public void RspTakeTaskReward(GameMsg msg)
    {
        var data = msg.RspTakeTaskReward;
        GameRoot.Instance.SetPlayerDataByTask(data);
        TasksPanel.FreshPanel();
        MainPanel.FreshPanel();
    }

    public void PshTaskPrgs(GameMsg msg)
    {
        var data = msg.PshTaskPrgs;
        GameRoot.Instance.SetPlayerDataByTaskPrgs(data);

        if (TasksPanel.IsOpen)
            TasksPanel.FreshPanel();
    }

    #endregion
}