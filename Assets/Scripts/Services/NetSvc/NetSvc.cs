using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PENet;
using Protocol;
using UnityEditor;

public class NetSvc : MonoBehaviour
{
    public static NetSvc Instance;
    protected static readonly string obj = "lock";
    private PESocket<ClientSession, GameMsg> client;
    private Queue<GameMsg> msgQue = new Queue<GameMsg>();

    public void InitSvc()
    {
        Instance = this;
        client = new PESocket<ClientSession, GameMsg>();
        client.SetLog(true, (msg, level) =>
            {
                switch (level)
                {
                    case 0:
                        msg = "log:" + msg;
                        Debug.Log(msg);
                        break;
                    case 1:
                        msg = "warn:" + msg;
                        Debug.LogWarning(msg);
                        break;
                    case 2:
                        msg = "error:" + msg;
                        Debug.LogError(msg);
                        break;
                    case 3:
                        msg = "info:" + msg;
                        Debug.Log(msg);
                        break;
                }
            }
        );
        CommonTool.Log("NetSvc Connected");
        client.StartAsClient(SrvCfg.srvIP, SrvCfg.srvPort);
    }

    public void AddNetMsg(GameMsg msg)
    {
        lock (obj)
        {
            msgQue.Enqueue(msg);
        }
    }

    private void Update()
    {
        if (msgQue.Count > 0)
        {
            lock (obj)
            {
                GameMsg msg = msgQue.Dequeue();
                ProcessMsg(msg);
            }
        }
    }

    private void ProcessMsg(GameMsg msg)
    {
        if (msg.err != (int) ErrCode.None)
        {
            switch ((ErrCode) msg.err)
            {
                case ErrCode.ServerDataError:
                    GameRoot.AddTips("客户端数据异常");
                    break;
                case ErrCode.AcctIsOnLine:
                    GameRoot.AddTips("当前账号已上线，请勿重复登陆");
                    break;
                case ErrCode.WrongPass:
                    GameRoot.AddTips("账号或密码错误");
                    break;
                case ErrCode.NameIsExits:
                    GameRoot.AddTips("该名字已存在");
                    break;
                case ErrCode.UpdateDbErr:
                    CommonTool.Log("数据库更新异常", LogType.Error);
                    GameRoot.AddTips("网络不稳定，请重试");
                    break;
                case ErrCode.ClientDataErr:
                    CommonTool.Log("客户端数据异常", LogType.Error);
                    GameRoot.AddTips("网络不稳定，请重试");
                    break;
                case ErrCode.LackCoin:
                    GameRoot.AddTips("金币不足");
                    break;
                case ErrCode.LackCrystal:
                    GameRoot.AddTips("水晶不足");
                    break;
                case ErrCode.LackLevel:
                    GameRoot.AddTips("等级不足");
                    break;
                case ErrCode.LackDiamond:
                    GameRoot.AddTips("钻石不足");
                    break;
                case ErrCode.LackPower:
                    GameRoot.AddTips("体力不足");
                    break;
            }

            return;
        }

        switch ((CMD) msg.cmd)
        {
            case CMD.RspLogin:
                LoginSys.Instance.RspLogin(msg);
                break;
            case CMD.RspReName:
                LoginSys.Instance.RspReName(msg);
                break;
            case CMD.RspGuide:
                MainCitySys.Instance.RspGuide(msg);
                break;
            case CMD.RspStrengthen:
                MainCitySys.Instance.RspStrengthen(msg);
                break;
            case CMD.PshChat:
                MainCitySys.Instance.PshChat(msg);
                break;
            case CMD.RspTranscation:
                MainCitySys.Instance.RspTranscation(msg);
                break;
            case CMD.PshPower:
                MainCitySys.Instance.PshPower(msg);
                break;
            case CMD.RspTakeTaskReward:
                MainCitySys.Instance.RspTakeTaskReward(msg);
                break;
            case CMD.PshTaskPrgs:
                MainCitySys.Instance.PshTaskPrgs(msg);
                break;
            case CMD.RspMission:
                MissionSys.Instance.RspMission(msg);
                break;
        }

        ;
    }

    public void SendMsg(GameMsg msg)
    {
        if (client.session == null)
        {
            GameRoot.AddTips("服务器未连接");
            InitSvc();
        }
        else
        {
            client.session.SendMsg(msg);
        }
    }
}