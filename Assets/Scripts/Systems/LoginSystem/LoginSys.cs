using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;

public class LoginSys : BaseSystem
{
    public static LoginSys Instance;
    public LoginPanel loginPanel;
    public CreatePanel CreatePanel;
    private GameObject p;

    public override void InitSys()
    {
        base.InitSys();
        Instance = this;


        CommonTool.Log("LoginSys Connected");
    }

    public void EnterLogin()
    {
     resSvc.AsyncLoadScene(Constans.SceneMain, () =>
            {
                CameraController cameraController = Camera.main.GetComponent<CameraController>();
                cameraController.enabled = false;
                MapCfg mapData = resSvc.GetMapCfgData(Constans.MainCityMapPreId);
                LoadPlayer(mapData);

                ViewSvc.Instance.AdjustDepthFieldFL(60f);
                loginPanel.SetPanelState();
             audioSvc.PlayBgAudio(Constans.BGLogin, true);
                GameRoot.AddTips("欢迎回来");
            }
        );
    }

    public void RspLogin(GameMsg msg)
    {
        GameRoot.Instance.SetPlayerData(msg.RspLogin);
        if (msg.RspLogin.PlayerData.name == "")
        {
            CreatePanel.SetPanelState();
            loginPanel.SetPanelState(false);
        }
        else
        {
            loginPanel.SetPanelState(false);
            PlayerOprateSys.Instance.EntoPLayerControll();
        }
    }


    public void RspReName(GameMsg msg)
    {
        GameRoot.Instance.SetPlayerName(msg.RspReName.Name);
        CreatePanel.SetPanelState(false);
        PlayerOprateSys.Instance.EntoPLayerControll();
    }

    private void LoadPlayer(MapCfg mapData)
    {
        p = resSvc.LoadPrefab(PathDefine.PlayerCity);
        PlayerOprateSys.Instance.InjectPOSysThings(p.GetComponent<PlayerController>(),Camera.main.GetComponent<CameraController>());
        PlayerOprateSys.Instance.DisablePlayerControl();
        p.transform.position = mapData.playerbornpos;
        p.transform.localEulerAngles = mapData.playerbornrote;
        var transform1 = Camera.main.transform;
        transform1.position = mapData.maincampos;
        transform1.eulerAngles = mapData.maincamrote;
    }
}