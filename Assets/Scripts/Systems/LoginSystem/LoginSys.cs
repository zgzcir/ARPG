using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;

public class LoginSys : BaseSystem
{
    public static LoginSys Instance;
    public LoginPanel loginPanel;
    public CreatePanel CreatePanel;
    private GameObject player;

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
        if (msg.RspLogin.PlayerData.Name == "")
        {
            CreatePanel.SetPanelState();
            loginPanel.SetPanelState(false);
        }
        else
        {
            loginPanel.SetPanelState(false);
            MainCitySys.Instance.EnterPlayerOprate();
        }
    }

    public void RspReName(GameMsg msg)
    {
        GameRoot.Instance.SetPlayerName(msg.RspReName.Name);
        CreatePanel.SetPanelState(false);
        MainCitySys.Instance.EnterPlayerOprate();
    }

    private void LoadPlayer(MapCfg mapData)
    {
        player = resSvc.LoadPrefab(PathDefine.PlayerCity);
        MainCitySys.Instance.InjectPOSysThings(player.GetComponent<PlayerController>(),
            Camera.main.GetComponent<CameraController>());
        MainCitySys.Instance.DisablePlayerControl();
        player.transform.position = mapData.PlayerBornPos;
        player.transform.localEulerAngles = mapData.PlayerBornRote;
        var transform1 = Camera.main.transform;
        transform1.position = mapData.MainCamPos;
        transform1.eulerAngles = mapData.MainCamRote;
    }
}