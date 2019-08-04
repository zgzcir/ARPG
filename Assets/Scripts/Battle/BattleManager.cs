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


                Camera.main.transform.position = mapCfg.MainCamPos;
                Camera.main.transform.localEulerAngles = mapCfg.MainCamRote;
                

                LoadPlayer(mapCfg);


                //       audioSvc.PlayBgAudio(Constans.BGCityHappy,true);bgm
            }
        );


        Debug.Log("Init BattleManager Done");
    }

    private void LoadPlayer(MapCfg mapCfg)
    {
        GameObject player=resSvc.LoadPrefab(PathDefine.PlayerBattle);
        player.transform.position = mapCfg.PlayerBornPos;
        player.transform.localEulerAngles = mapCfg.PlayerBornRote;
        player.transform.localScale=Vector3.one;
        
        //todo 2019.7.30 18:28
//        player.GetComponent<PlayerController>().Init();
//        playerController.Init();
//        if (Camera.main != null) cameraController = Camera.main.GetComponent<CameraController>();
//        cameraController.Target = playerController.CameraPivot.transform;

    }
}