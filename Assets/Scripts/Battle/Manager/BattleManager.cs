using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private ResSvc resSvc;
    private AudioSvc audioSvc;


    private StateManager stateManager;
    private SkillManager skillManager;
    private MapManager mapManager;


    private PlayerController playerController;
    private CameraController cameraController;
    private Entityplayer entitySelfplayer;

    public void InitManager(int mid)
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
        stateManager = gameObject.AddComponent<StateManager>();
        stateManager.InitManager();
        skillManager = gameObject.AddComponent<SkillManager>();
        skillManager.InitManager();
        MapCfg mapCfg = resSvc.GetMapCfgData(mid);
        resSvc.AsyncLoadScene(mapCfg.SceneName, () =>
            {
                GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
                mapManager = map.GetComponent<MapManager>();

                map.transform.localPosition = Vector3.zero;
                map.transform.localScale = Vector3.one;

                if (Camera.main != null) cameraController = Camera.main.GetComponent<CameraController>();
                cameraController.transform.position
                    = mapCfg.MainCamPos;
                cameraController.transform.localEulerAngles = mapCfg.MainCamRote;

                LoadPlayer(mapCfg);
                entitySelfplayer.Idle();
                GameRoot.AddTips("四风试炼场");
                BattleSys.Instance.SetBattlePanelState();


                //       audioSvc.PlayBgAudio(Constans.BGCityHappy,true);bgm
            }
        );

        Debug.Log("Init BattleManager Done");
    }

    private void LoadPlayer(MapCfg mapCfg)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.PlayerBattle);
        player.transform.localPosition = mapCfg.PlayerBornPos;
        player.transform.localEulerAngles = mapCfg.PlayerBornRote;
        player.transform.localScale = Vector3.one;

        //todo 2019.7.30 18:28
        playerController = player.GetComponent<PlayerController>();
        playerController.Init();

        entitySelfplayer = new Entityplayer()
        {
            StateManager = stateManager,
            Controller = playerController,
            SkillManager = skillManager,
            BattleManager = this
        };

        //ttt   
        cameraController.Target = playerController.CameraPivot.transform;
        cameraController.enabled = true;
        // 
    }

    public void SetSelfPlayerMoveDir(Vector2 dir)
    {
        if (!entitySelfplayer.canControl) return;
        if (dir == Vector2.zero)
        {
            entitySelfplayer.Idle();
        }
        else
        {
            entitySelfplayer.Move();
            //
            entitySelfplayer.SetDir(dir);
        }
    }

    public void ReqReleaseSkill(int index)
    {
        switch (index)
        {
            case 0:
                ReleaseNormalAtk();
                break;
            case 1:
                ReleaseSkill1();
                break;
            case 2:
                ReleaseSkill2();
                break;
            case 3:
                ReleaseSkill3();
                break;
        }
    }

    private void ReleaseNormalAtk()
    {
        Debug.Log("Normal Atk");
    }

    private void ReleaseSkill1()
    {
        entitySelfplayer.Attack(101);
    }

    private void ReleaseSkill2()
    {
        Debug.Log("2 Atk");
    }

    private void ReleaseSkill3()
    {
        Debug.Log("3 Atk");
    }


    public Vector2 GetDirInput()
    {
        return BattleSys.Instance.GetDirInput();
    }
}